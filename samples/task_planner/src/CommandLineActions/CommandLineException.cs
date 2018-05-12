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

    /// <summary>
    /// Defines the command line exception class.
    /// </summary>
    [Serializable]
    public class CommandLineException : Exception
    {
        /// <summary>
        /// The internal command line exception message.
        /// </summary>
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineException"/>
        /// class.
        /// </summary>
        public CommandLineException()
        {
            this.message = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineException"/>
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CommandLineException(string message)
            : base(message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineException"/>
        /// class with a specified error message and a reference to the inner
        ///  exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null
        /// reference if no inner exception is specified.
        /// </param>
        public CommandLineException(string message, Exception inner)
            : base(message, inner)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineException"/>
        /// class with a specified <see cref="CommandLineErrorCode"/> and an
        /// error message format template and arguments.
        /// </summary>
        /// <param name="errorCode">
        /// The specified <see cref="CommandLineErrorCode"/> for the exception.
        /// </param>
        /// <param name="messageFormat">
        /// A composite exception message format string.
        /// </param>
        /// <param name="messageFormatArgs">
        /// An object array that contains zero or more objects to the error
        /// message format.
        /// </param>
        public CommandLineException(
            CommandLineErrorCode errorCode,
            string messageFormat,
            params object[] messageFormatArgs)
        {
            this.ErrorCode = errorCode;
            this.message = messageFormat.FormatInvariant(messageFormatArgs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineException"/>
        /// class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual
        /// information about the source or destination.
        /// </param>
        protected CommandLineException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            this.ErrorCode =
                (CommandLineErrorCode)info.GetValue(
                    nameof(this.ErrorCode),
                    typeof(CommandLineErrorCode));
            this.message = info.GetString(nameof(this.message));
        }

        /// <summary>
        /// Gets or sets the <see cref="CommandLineErrorCode"/> for the
        /// current exception.
        /// </summary>
        public CommandLineErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
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

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> with information about the
        /// exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual
        /// information about the source or destination.
        /// </param>
        public override void GetObjectData(
            SerializationInfo info,
            StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(this.ErrorCode), this.ErrorCode);
            info.AddValue(nameof(this.message), this.message);
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        [ExcludeFromCoverage(@"Only used for debugging purpose.")]
        public override string ToString()
            => $"[{this.GetType().Name}] {nameof(this.ErrorCode)}: {this.ErrorCode}({(int)this.ErrorCode}), {nameof(this.Message)}: {this.Message}";
    }
}