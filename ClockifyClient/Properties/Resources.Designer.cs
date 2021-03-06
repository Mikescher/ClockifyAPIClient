﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClockifyAPIClient.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClockifyAPIClient.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE &quot;workspaces&quot; (
        ///	&quot;workspace_id&quot;   TEXT NOT NULL,
        ///	&quot;name&quot;           TEXT NOT NULL,
        ///
        ///	PRIMARY KEY(&quot;workspace_id&quot;)
        ///);
        ///
        ///CREATE TABLE &quot;users&quot; (
        ///	&quot;user_id&quot;   TEXT    NOT NULL,
        ///	&quot;email&quot;     TEXT    NOT NULL,
        ///	&quot;name&quot;      TEXT    NOT NULL,
        ///
        ///	PRIMARY KEY(&quot;user_id&quot;)
        ///);
        ///
        ///CREATE TABLE &quot;map_users_workspaces&quot; (
        ///	&quot;user_id&quot;       TEXT NOT NULL,
        ///	&quot;workspace_id&quot;  TEXT NOT NULL
        ///);
        ///
        ///CREATE TABLE &quot;clients&quot; (
        ///	&quot;client_id&quot;     TEXT    NOT NULL,
        ///	&quot;name&quot;          TEXT    NOT NULL,
        ///	&quot;archived&quot;      [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string schema {
            get {
                return ResourceManager.GetString("schema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///CREATE VIEW data_view AS
        ///SELECT 
        ///  users.name AS [user], projects.name AS [project], tasks.name AS [task],
        ///  entries.description, entries.billable, entries.interval_start AS [start], entries.interval_end AS [end], entries.interval_duration AS [duration], 
        ///  workspaces.name AS [workspace]
        ///FROM [entries] 
        ///LEFT JOIN [workspaces] ON entries.workspace_id = workspaces.workspace_id
        ///LEFT JOIN [projects]   ON entries.project_id   = projects.project_id
        ///LEFT JOIN [tasks]      ON entries.task_id      = tasks. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string views {
            get {
                return ResourceManager.GetString("views", resourceCulture);
            }
        }
    }
}
