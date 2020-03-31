CREATE TABLE "workspaces" (
	"workspace_id"   TEXT NOT NULL,
	"name"           TEXT NOT NULL,

	PRIMARY KEY("workspace_id")
);

CREATE TABLE "users" (
	"user_id"   TEXT    NOT NULL,
	"email"     TEXT    NOT NULL,
	"name"      TEXT    NOT NULL,

	PRIMARY KEY("user_id")
);

CREATE TABLE "map_users_workspaces" (
	"user_id"       TEXT NOT NULL,
	"workspace_id"  TEXT NOT NULL
);

CREATE TABLE "clients" (
	"client_id"     TEXT    NOT NULL,
	"name"          TEXT    NOT NULL,
	"archived"      INTEGER NOT NULL,
	"workspace_id"  TEXT    NOT NULL,

	PRIMARY KEY("client_id")
);

CREATE TABLE "projects" (
	"project_id"     TEXT    NOT NULL,
	"name"           TEXT    NOT NULL,
	"archived"       INTEGER NOT NULL,
	"workspace_id"   TEXT    NOT NULL,
	"client_id"      TEXT        NULL,

	PRIMARY KEY("project_id")
);

CREATE TABLE "tasks" (
	"task_id"        TEXT NOT NULL,
	"name"           TEXT NOT NULL,
	"workspace_id"   TEXT NOT NULL,
	"project_id"     TEXT NOT NULL,

	PRIMARY KEY("task_id")
);

CREATE TABLE "entries" (
	"entry_id"             TEXT    NOT NULL,
	"user_id"              TEXT    NOT NULL,
	"description"          TEXT        NULL,
	"workspace_id"         TEXT    NOT NULL,
	"project_id"           TEXT        NULL,
	"task_id"              TEXT        NULL,
	"billable"             INTEGER NOT NULL,
	"interval_start"       TEXT    NOT NULL,
	"interval_end"         TEXT    NOT NULL,
	"interval_duration"    INTEGER NOT NULL,

	PRIMARY KEY("entry_id")
);