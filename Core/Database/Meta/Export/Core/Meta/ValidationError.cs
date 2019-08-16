//------------------------------------------------------------------------------------------------- 
// <copyright file="ValidationError.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ValidationError type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.Meta
{
    /// <summary>
    /// An error that occurred during validation.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The kind of validation.
        /// </summary>
        private readonly ValidationKind kind;

        /// <summary>
        /// The validated member.
        /// </summary>
        private readonly string[] members;

        /// <summary>
        /// The error message.
        /// </summary>
        private readonly string message;

        /// <summary>
        /// The object that contains the member.
        /// </summary>
        private readonly object source;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        /// <param name="kind">The kind .</param>
        /// <param name="members">The members.</param>
        public ValidationError(string message, object source, ValidationKind kind, string[] members)
        {
            this.source = source;
            this.members = members;
            this.kind = kind;
            this.message = message;
        }

        /// <summary>
        /// Gets the kind of validation.
        /// </summary>
        /// <value>The kind of validation.</value>
        public ValidationKind Kind
        {
            get { return this.kind; }
        }

        /// <summary>
        /// Gets the validated members.
        /// </summary>
        /// <value>The validated members.</value>
        public string[] Members
        {
            get { return this.members; }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// Gets the object that contains the member.
        /// </summary>
        /// <value>The source.</value>
        public object Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.message;
        }
    }
}
