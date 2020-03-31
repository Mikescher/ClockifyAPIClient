
CREATE VIEW data_view AS
SELECT 
  users.name AS [user], projects.name AS [project], tasks.name AS [task],
  entries.description, entries.billable, entries.interval_start AS [start], entries.interval_end AS [end], entries.interval_duration AS [duration], 
  workspaces.name AS [workspace]
FROM [entries] 
LEFT JOIN [workspaces] ON entries.workspace_id = workspaces.workspace_id
LEFT JOIN [projects]   ON entries.project_id   = projects.project_id
LEFT JOIN [tasks]      ON entries.task_id      = tasks.task_id
LEFT JOIN [users]      ON entries.user_id      = users.user_id

ORDER BY entries.interval_start DESC
