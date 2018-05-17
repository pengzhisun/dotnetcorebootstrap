namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    internal static class TaskConstants
    {
        public const string TaskCategory = "Task";

        /// <summary>
        /// The task command help message.
        /// </summary>
        public const string TaskCommandHelpMessage =
        @"`tp task` command line arguments:
   new       :      new task command, `tp task new -h` for more detail.
-----------------------------------------------------------
general arguments:
   -h, --help:      show `tp task` command help message.
";

        /// <summary>
        /// The task command help message.
        /// </summary>
        public const string TaskNewCommandHelpMessage =
        @"`tp task new` command line arguments:
   -n,   --name:         [required] task name.
   -d,   --description:  [optional] task description.
   -pid, --parent-id:    [optional] parent task id, should only use either parent id or parent name.
   -pn,  --parent-name:  [optional] parent task name, should only use either parent id or parent name.
-----------------------------------------------------------
general arguments:
   -h,   --help:         show `tp task new` command help message.
";
    }
}