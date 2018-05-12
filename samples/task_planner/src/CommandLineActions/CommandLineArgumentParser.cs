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
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Defines the command line argument parser class.
    /// </summary>
    internal static class CommandLineArgumentParser
    {
        /// <summary>
        /// Defines the parser state enumeration.
        /// </summary>
        private enum ParserState
        {
            /// <summary>
            /// The initial state.
            /// </summary>
            Initial,

            /// <summary>
            /// The getting category and action state.
            /// </summary>
            CategoryAndAction,

            /// <summary>
            /// The getting action parameter begin state.
            /// </summary>
            ActionParamBegin,

            /// <summary>
            /// The getting action paramter end state.
            /// </summary>
            ActionParamEnd
        }

        /// <summary>
        /// Parses the command line arguments to <see cref="CommandLineArgument"/>
        /// instance.
        /// </summary>
        /// <param name="args">The original command line arguments.</param>
        /// <returns>
        /// The parsed <see cref="CommandLineArgument"/> instance.
        /// </returns>
        /// <exception cref="CommandLineException">
        /// Thrown if the given command line arguments are invalid.
        /// </exception>
        /// <examples>
        /// Supported arguments format:
        /// 1. nothing
        /// 2. {category} {action?} ({-param_name} {-param_value?})[0-n]
        /// Not supported arguments format:
        /// 1. {category} {action?} ({-param_name} {-param_value}[2-])[0-n]
        /// </examples>
        public static CommandLineArgument Parse(IReadOnlyList<string> args)
        {
            if (args == null || args.Count == 0)
            {
                return new CommandLineArgument();
            }

            ParserContext context = new ParserContext(args);
            while (context.GoNext())
            {
                switch (context.CurretState)
                {
                    case ParserState.Initial:
                        ProcessInitialState(ref context);
                        break;
                    case ParserState.CategoryAndAction:
                        ProcessCategoryAndActionState(ref context);
                        break;
                    case ParserState.ActionParamBegin:
                        ProcessActionParamBeginState(ref context);
                        break;
                    case ParserState.ActionParamEnd:
                        ProcessActionParamEndState(ref context);
                        break;
                }
            }

            // {category} {action?}
            if (context.CurretState == ParserState.CategoryAndAction)
            {
                context.SetCategoryAndAction();
            }

            return new CommandLineArgument(
                context.Category,
                context.Action,
                context.ActionParams);
        }

        /// <summary>
        /// Processes the initial state context, if the current argument value
        /// is a parameter name then switch to parameter begin state, else to
        /// category and action begin state.
        /// </summary>
        /// <param name="context">The parser context reference.</param>
        private static void ProcessInitialState(ref ParserContext context)
        {
            if (context.CurrentArgIsParamName())
            {
                context.SetParamBeginState();
            }
            else
            {
                context.SetCategoryAndActionState();
            }
        }

        /// <summary>
        /// Process the category and action state, if the current argument value
        /// is a parameter name then set the category and action value and swith
        /// to parameter begin state, else update the last category and action
        /// index with current argument index.
        /// </summary>
        /// <param name="context">The parser context reference.</param>
        private static void ProcessCategoryAndActionState(
            ref ParserContext context)
        {
            if (!context.CurrentArgIsParamName())
            {
                context.UpdateCategoryAndActionState();
                return;
            }

            context.SetCategoryAndAction();

            context.SetParamBeginState();
        }

        /// <summary>
        /// Process the action parameter begin state, if the current argument is
        /// another parameter name then refresh the parameter begin state with
        /// new parameter name, else switch to parameter end state.
        /// </summary>
        /// <param name="context">The parser context reference.</param>
        private static void ProcessActionParamBeginState(
            ref ParserContext context)
        {
            if (context.CurrentArgIsParamName())
            {
                context.SetParamBeginState();
            }
            else
            {
                context.SetParamEndState();
            }
        }

        /// <summary>
        /// Process the action parameter end state, if the current argument is
        /// parameter name then set the current parameter value and refresh the
        /// parameter begin state with new parameter name, else throw invalid
        /// argument exception.
        /// </summary>
        /// <exception cref="CommandLineException">
        /// Thrown if the current argument is invalid, e.g. not support more
        /// than one parameter value arguments after one parameter name argument.
        /// </exception>
        /// <param name="context">The parser context reference.</param>
        private static void ProcessActionParamEndState(
            ref ParserContext context)
        {
            if (!context.CurrentArgIsParamName())
            {
                throw new CommandLineException(
                    CommandLineErrorCode.CommandLineArgsParseFailed,
                    ExceptionMessages.InvalidCommandLineArguments,
                    context.CurrentArg,
                    context.FullArgumentsString);
            }

            context.SetParamBeginState();
        }

        /// <summary>
        /// Defines the internal parser context class, it only provides atom
        /// state changes operation methods, and the process methods in the
        /// parser should define the state switching logic.
        /// </summary>
        /// <remarks>
        /// This context class is not thread safe and should only be used inside
        /// the parser class.
        /// </remarks>
        private sealed class ParserContext
        {
            /// <summary>
            /// The original command line arguments.
            /// </summary>
            private readonly IReadOnlyList<string> args;

            /// <summary>
            /// The current argument index.
            /// </summary>
            private int currentArgIndex;

            /// <summary>
            /// The current parameter name.
            /// </summary>
            private string currentParamName;

            /// <summary>
            /// The internal action parameters dictionary.
            /// </summary>
            private IDictionary<string, string> actionParams;

            /// <summary>
            /// The last category and action argument index.
            /// </summary>
            private int lastCategoryAndActionArgIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParserContext"/>
            /// class with original command line arguments.
            /// </summary>
            /// <param name="args">The original command line arguments.</param>
            public ParserContext(IReadOnlyList<string> args)
            {
                Debug.Assert(args.Count > 0, @"The given arguments shouldn't be empty.");

                this.args = args;
                this.currentArgIndex = -1;
                this.CurretState = ParserState.Initial;
                this.actionParams = new Dictionary<string, string>();
                this.lastCategoryAndActionArgIndex = -1;
            }

            /// <summary>
            /// Gets the full arguments string.
            /// </summary>
            public string FullArgumentsString => string.Join(' ', this.args);

            /// <summary>
            /// Gets the parsed category value.
            /// </summary>
            public string Category { get; private set; }

            /// <summary>
            /// Gets the parsed action value.
            /// </summary>
            public string Action { get; private set; }

            /// <summary>
            /// Gets the parsed action parameters dictionary.
            /// </summary>
            public IReadOnlyDictionary<string, string> ActionParams
                => this.actionParams as IReadOnlyDictionary<string, string>;

            /// <summary>
            /// Gets the current state.
            /// </summary>
            public ParserState CurretState { get; private set; }

            /// <summary>
            /// Gets the current argument.
            /// </summary>
            public string CurrentArg => this.args[this.currentArgIndex];

            /// <summary>
            /// Goes to the next argument.
            /// </summary>
            /// <returns>
            /// Return false if iterated all arguments.
            /// </returns>
            public bool GoNext()
            {
                if (this.currentArgIndex == this.args.Count - 1)
                {
                    return false;
                }

                this.currentArgIndex++;

                return true;
            }

            /// <summary>
            /// Checks the current argument is a parameter name or not.
            /// </summary>
            /// <returns>
            /// Return true if the current argument has a '-' prefix, otherwise
            /// return false.
            /// </returns>
            public bool CurrentArgIsParamName()
            {
                const char DashSymbol = '-';

                return this.CurrentArg.StartsWith(DashSymbol);
            }

            /// <summary>
            /// Sets the current state to category and action.
            /// </summary>
            public void SetCategoryAndActionState()
            {
                Debug.Assert(
                    this.CurretState == ParserState.Initial,
                    @"Should switch state from initial.");

                this.CurretState = ParserState.CategoryAndAction;
                this.lastCategoryAndActionArgIndex = this.currentArgIndex;
            }

            /// <summary>
            /// Updates the category and action state to refresh the last
            /// category and action index with the current argument index.
            /// </summary>
            public void UpdateCategoryAndActionState()
            {
                this.lastCategoryAndActionArgIndex = this.currentArgIndex;
            }

            /// <summary>
            /// Sets the parsed values to category and action property.
            /// </summary>
            public void SetCategoryAndAction()
            {
                Debug.Assert(
                    this.lastCategoryAndActionArgIndex >= 0
                    && this.lastCategoryAndActionArgIndex < this.args.Count,
                    @"The last category and action argument index should be valid index.");
                Debug.Assert(
                    this.CurretState == ParserState.CategoryAndAction,
                    @"Current state should be category and action or action parameter begin.");

                const string CategorySeparator = " ";

                if (this.lastCategoryAndActionArgIndex == 0)
                {
                    // {category} {-params}
                    this.Category = this.args[this.lastCategoryAndActionArgIndex];
                }
                else
                {
                    // {category:1-n} {action} {params}
                    this.Category =
                        string.Join(
                            CategorySeparator,
                            this.args.Take(this.lastCategoryAndActionArgIndex));
                    this.Action = this.args[this.lastCategoryAndActionArgIndex];
                }
            }

            /// <summary>
            /// Sets the current state to parameter begin and add an action
            /// parameter item with current argument as key and null value in
            /// the action parameters dictionary.
            /// </summary>
            public void SetParamBeginState()
            {
                Debug.Assert(
                    this.CurrentArgIsParamName(),
                    @"Current argument should be parameter name.");

                this.CurretState = ParserState.ActionParamBegin;
                this.SetActionParameter(this.CurrentArg, null);
                this.currentParamName = this.CurrentArg;
            }

            /// <summary>
            /// Sets the current state to parameter end and update the action
            /// parameter item for current parameter name with current argument
            /// as value in the action parameters dictionary.
            /// </summary>
            public void SetParamEndState()
            {
                Debug.Assert(
                    this.CurretState == ParserState.ActionParamBegin,
                    @"Should change state from parameter begin.");
                Debug.Assert(
                    !this.CurrentArgIsParamName(),
                    @"Current argument shouldn'o't be parameter name.");

                this.CurretState = ParserState.ActionParamEnd;
                this.SetActionParameter(this.currentParamName, this.CurrentArg);
                this.currentParamName = null;
            }

            /// <summary>
            /// Sets the action parameter item.
            /// </summary>
            /// <param name="paramName">The parameter name.</param>
            /// <param name="paramValue">The parameter value.</param>
            private void SetActionParameter(string paramName, string paramValue)
                => this.actionParams[paramName] = paramValue;
        }
    }
}