// -----------------------------------------------------------------------
// <copyright file="CommandLineException.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CommandLineException : Exception
    {
        private readonly string message;

        public CommandLineException()
        {
            this.message = null;
        }

        public CommandLineException(string message)
            : base(message)
        {
            this.message = message;
        }

        public CommandLineException(string message, Exception inner)
            : base(message, inner)
        {
            this.message = message;
        }

        public CommandLineException(
            CommandLineErrorCode errorCode,
            string messageFormat,
            params object[] messageFormatArgs)
        {
            this.ErrorCode = errorCode;
            this.message = messageFormat.FormatInvariant(messageFormatArgs);
        }

        protected CommandLineException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            this.ErrorCode =
                (CommandLineErrorCode)info.GetValue(
                    nameof(this.ErrorCode),
                    typeof(CommandLineErrorCode));
        }

        public CommandLineErrorCode ErrorCode { get; set; }

        public override string Message
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.message))
                {
                    return this.message;
                }

                string messageFormat =
                    ExceptionMessages.DefaultCommandLineExceptionMessage;
                return messageFormat.FormatInvariant(this.ErrorCode);
            }
        }

        public override void GetObjectData(
            SerializationInfo info,
            StreamingContext context)
        {
            info.AddValue(nameof(this.ErrorCode), this.ErrorCode);
        }

        public override string ToString()
            => $"[{this.GetType().Name}] {nameof(this.ErrorCode)}: {this.ErrorCode}({(int)this.ErrorCode}), {nameof(this.Message)}: {this.Message}";
    }
}