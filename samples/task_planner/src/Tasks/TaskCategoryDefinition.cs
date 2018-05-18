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

            TaskDbEntity entity = TaskDbRepository.CreateTask(actionArg);
            Console.WriteLine($"task entity created: '{entity}'");
        }

        [Action(TaskActionType.Set)]
        public static void SetAction(TaskSetActionArgument actionArg)
        {
            if (!actionArg.IsValid() || actionArg.HelpSwtichEnabled)
            {
                ShowHelpMessage(TaskConstants.TaskNewCommandHelpMessage);
                return;
            }

            TaskDbEntity entity = TaskDbRepository.UpdateTask(actionArg);
            Console.WriteLine($"task entity created: '{entity}'");
        }

        private static void ShowHelpMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}