namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class CommandLineArgumentParser
    {
        private const char DashSymbol = '-';

        public static CommandLineArgument Parse(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return new CommandLineArgument();
            }

            ParserContext context = new ParserContext(args);
            for (int idx = 0; idx < args.Length; ++idx)
            {
                context.CurrentArgIndex = idx;

                switch (context.CurretState)
                {
                    case ParserState.Init:
                        ProcessInitState(ref context);
                        break;
                    case ParserState.CategoryAndActionBegin:
                        ProcessCategoryAndActionBeginState(ref context);
                        break;
                    case ParserState.ActionParamBegin:
                        ProcessActionParamBeginState(ref context);
                        break;
                    case ParserState.ActionParamEnd:
                        ProcessActionParamEndState(ref context);
                        break;
                }
            }

            if (context.CurretState == ParserState.CategoryAndActionBegin)
            {
                SetCategoryAndAction(ref context);
            }

            return new CommandLineArgument(
                context.Category,
                context.Action,
                context.ActionParams);
        }

        private static void ProcessInitState(ref ParserContext context)
        {
            if (context.CurrentArg.HasDashPrefix())
            {
                context.CurretState = ParserState.ActionParamBegin;
                context.ActionParams[context.CurrentArg] = null;
                context.CurrentParamName = context.CurrentArg;
            }
            else
            {
                context.CurretState = ParserState.CategoryAndActionBegin;
                context.LastNoDashPrefixArgIndex = context.CurrentArgIndex;
            }
        }

        private static void ProcessCategoryAndActionBeginState(
            ref ParserContext context)
        {
            if (!context.CurrentArg.HasDashPrefix())
            {
                context.LastNoDashPrefixArgIndex = context.CurrentArgIndex;
                return;
            }

            SetCategoryAndAction(ref context);

            context.CurretState = ParserState.ActionParamBegin;
            context.ActionParams[context.CurrentArg] = null;
            context.CurrentParamName = context.CurrentArg;
        }

        private static void SetCategoryAndAction(ref ParserContext context)
        {
            const string CategorySeparator = " ";

            if (context.LastNoDashPrefixArgIndex == 0)
            {
                context.Category = context.Args[0];
            }
            else
            {
                context.Category =
                    string.Join(
                        CategorySeparator,
                        context.Args.Take(context.LastNoDashPrefixArgIndex));
                context.Action = context.Args[context.LastNoDashPrefixArgIndex];
            }
        }

        private static void ProcessActionParamBeginState(
            ref ParserContext context)
        {
            if (context.CurrentArg.HasDashPrefix())
            {
                context.CurretState = ParserState.ActionParamBegin;
                context.ActionParams[context.CurrentArg] = null;
                context.CurrentParamName = context.CurrentArg;
            }
            else
            {
                context.CurretState = ParserState.ActionParamEnd;
                string paramName = context.CurrentParamName;
                context.ActionParams[paramName] = context.CurrentArg;
                context.CurrentParamName = null;
            }
        }

        private static void ProcessActionParamEndState(
            ref ParserContext context)
        {
            if (!context.CurrentArg.HasDashPrefix())
            {
                throw new InvalidOperationException(
                    $"The input arguments contains invalid value: '{context.CurrentArg}', full arguments: '{context.ArgsString}'");
            }

            context.CurretState = ParserState.ActionParamBegin;
            context.ActionParams[context.CurrentArg] = null;
            context.CurrentParamName = context.CurrentArg;
        }

        private static bool HasDashPrefix(this string arg)
            => arg.StartsWith(DashSymbol);

        private sealed class ParserContext
        {
            public ParserContext(string[] args)
            {
                this.Args = args;
                this.ActionParams = new Dictionary<string, string>();
                this.CurretState = ParserState.Init;
                this.LastNoDashPrefixArgIndex = -1;
            }

            public string[] Args { get; private set; }

            public string ArgsString => string.Join(' ', this.Args);

            public string Category { get; set; }

            public string Action { get; set; }

            public IDictionary<string, string> ActionParams { get; private set; }

            public ParserState CurretState { get; set; }

            public int CurrentArgIndex { get; set; }

            public string CurrentArg => this.Args[this.CurrentArgIndex];

            public int LastNoDashPrefixArgIndex { get; set; }

            public string CurrentParamName { get; set; }
        }

        private enum ParserState
        {
            Init,
            CategoryAndActionBegin,
            ActionParamBegin,
            ActionParamEnd
        }
    }
}