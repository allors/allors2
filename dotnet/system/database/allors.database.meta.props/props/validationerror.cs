// <copyright file="ValidationError.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ValidationError type.</summary>

namespace Allors.Database.Meta
{
    /// <summary>
    /// An error that occurred during validation.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Initializes a new state of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        /// <param name="kind">The kind .</param>
        /// <param name="members">The members.</param>
        public ValidationError(string message, object source, ValidationKind kind, string[] members)
        {
            this.Source = source;
            this.Members = members;
            this.Kind = kind;
            this.Message = message;
        }

        /// <summary>
        /// Gets the kind of validation.
        /// </summary>
        /// <value>The kind of validation.</value>
        public ValidationKind Kind { get; private set; }

        /// <summary>
        /// Gets the validated members.
        /// </summary>
        /// <value>The validated members.</value>
        public string[] Members { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the object that contains the member.
        /// </summary>
        /// <value>The source.</value>
        public object Source { get; private set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() => this.Message;
    }
}
