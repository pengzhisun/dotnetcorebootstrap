namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    using System;
    using DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions;

    [Category(TaskConstants.TaskCategory, typeof(TaskActionType))]
    internal static class TaskCategoryDefinition
    {
        [Action(TaskActionType.Default)]
        public static void DefaultAction(TaskActionArgument actionArg)
        {
            ShowHelpMessage(TaskConstants.TaskCommandHelpMessage);
        }

        [Action(TaskActionType.New)]
        public static void NewAction(TaskNewActionArgument actionArg)
        {
            if (!actionArg.IsValid() || actionArg.HelpSwtichEnabled)
            {
                ShowHelpMessage(TaskConstants.TaskNewCommandHelpMessage);
                return;
            }

            Console.WriteLine($"new task: name='{actionArg.Name}', desc='{actionArg.Description}'");
        }

        private static void ShowHelpMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}