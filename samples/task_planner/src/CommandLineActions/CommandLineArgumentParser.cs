// -----------------------------------------------------------------------
// <copyright file="CommandLineArgumentParser.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class CommandLineArgumentParser
    {
        private enum ParserState
        {
            Init,
            CategoryAndActionBegin,
            ActionParamBegin,
            ActionParamEnd
        }

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
                context.SetCategoryAndAction();
            }

            return new CommandLineArgument(
                context.Category,
                context.Action,
                context.ActionParams);
        }

        private static void ProcessInitState(ref ParserContext context)
        {
            if (context.CurrentArgHasDashPrefix())
            {
                context.SetParamBeginState();
            }
            else
            {
                context.SetCategoryAndActionState();
            }
        }

        private static void ProcessCategoryAndActionBeginState(
            ref ParserContext context)
        {
            if (!context.CurrentArgHasDashPrefix())
            {
                context.UpdateCategoryAndActionState();
                return;
            }

            context.SetCategoryAndAction();

            context.SetParamBeginState();
        }

        private static void ProcessActionParamBeginState(
            ref ParserContext context)
        {
            if (context.CurrentArgHasDashPrefix())
            {
                context.SetParamBeginState();
            }
            else
            {
                context.SetParamEndState();
            }
        }

        private static void ProcessActionParamEndState(
            ref ParserContext context)
        {
            if (!context.CurrentArgHasDashPrefix())
            {
                throw new CommandLineException(
                    CommandLineErrorCode.CommandLineArgsParseFailed,
                    ExceptionMessages.InvalidCommandLineArguments,
                    context.CurrentArg,
                    context.ArgsString);
            }

            context.SetParamBeginState();
        }

        private sealed class ParserContext
        {
            private readonly string[] args;

            private int lastNoDashPrefixArgIndex;

            private string currentParamName;

            public ParserContext(string[] args)
            {
                this.args = args;
                this.ActionParams = new Dictionary<string, string>();
                this.CurretState = ParserState.Init;
                this.lastNoDashPrefixArgIndex = -1;
            }

            public string ArgsString => string.Join(' ', this.args);

            public string Category { get; set; }

            public string Action { get; set; }

            public IDictionary<string, string> ActionParams { get; private set; }

            public ParserState CurretState { get; set; }

            public int CurrentArgIndex { get; set; }

            public string CurrentArg => this.args[this.CurrentArgIndex];

            public bool CurrentArgHasDashPrefix()
            {
                const char DashSymbol = '-';

                return this.CurrentArg.StartsWith(DashSymbol);
            }

            public void SetParamBeginState()
            {
                this.CurretState = ParserState.ActionParamBegin;
                this.SetActionParameter(this.CurrentArg, null);
                this.currentParamName = this.CurrentArg;
            }

            public void SetParamEndState()
            {
                this.CurretState = ParserState.ActionParamEnd;
                this.SetActionParameter(this.currentParamName, this.CurrentArg);
                this.currentParamName = null;
            }

            public void SetCategoryAndActionState()
            {
                this.CurretState = ParserState.CategoryAndActionBegin;
                this.lastNoDashPrefixArgIndex = this.CurrentArgIndex;
            }

            public void UpdateCategoryAndActionState()
            {
                this.lastNoDashPrefixArgIndex = this.CurrentArgIndex;
            }

            public void SetCategoryAndAction()
            {
                const string CategorySeparator = " ";

                if (this.lastNoDashPrefixArgIndex == 0)
                {
                    this.Category = this.args[0];
                }
                else
                {
                    this.Category =
                        string.Join(
                            CategorySeparator,
                            this.args.Take(this.lastNoDashPrefixArgIndex));
                    this.Action = this.args[this.lastNoDashPrefixArgIndex];
                }
            }

            private void SetActionParameter(string paramName, string paramValue)
                => this.ActionParams[paramName.ToLower()] = paramValue;
        }
    }
}