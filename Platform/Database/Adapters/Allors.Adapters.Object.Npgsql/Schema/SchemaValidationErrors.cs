//------------------------------------------------------------------------------------------------- 
// <copyright file="SchemaValidationErrors.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the SchemaValidationErrors type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.Adapters.Database.Sql
{
    using System.Collections.Generic;

    using Allors.Meta;

    /// <summary>
    /// Holds the <see cref="ISchemaValidationError"/>s that occured during the validation
    /// of the <see cref="MetaPopulation"/> against the database schema.
    /// </summary>
    public class SchemaValidationErrors
    {
        /// <summary>
        /// The errors that occured during validation of the <see cref="MetaPopulation"/> against the Sql schema.
        /// </summary>
        private readonly List<ISchemaValidationError> errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidationErrors"/> class.
        /// </summary>
        public SchemaValidationErrors()
        {
            this.errors = new List<ISchemaValidationError>();
        }

        /// <summary>
        /// Gets the schema validation errors.
        /// </summary>
        /// <value>The errors.</value>
        public ISchemaValidationError[] Errors
        {
            get { return this.errors.ToArray(); }
        }

        public TableSchemaValidationError[] TableErrors
        {
            get
            {
                var tableErrors = new List<TableSchemaValidationError>();
                foreach (var error in this.errors)
                {
                    if (error is TableSchemaValidationError)
                    {
                        tableErrors.Add((TableSchemaValidationError)error);
                    }
                }

                return tableErrors.ToArray();
            }
        }

        public ProcedureSchemaValidationError[] ProcedureErrors
        {
            get
            {
                var procedureErrors = new List<ProcedureSchemaValidationError>();
                foreach (var error in this.errors)
                {
                    if (error is TableSchemaValidationError)
                    {
                        procedureErrors.Add((ProcedureSchemaValidationError)error);
                    }
                }

                return procedureErrors.ToArray();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get { return this.errors.Count > 0; }
        }

        /// <summary>
        /// Gets the validation error messages.
        /// </summary>
        /// <value>The validation error messages.</value>
        public string[] Messages
        {
            get
            {
                var messages = new string[this.errors.Count];
                for (int i = 0; i < messages.Length; i++)
                {
                    messages[i] = this.errors[i].Message;
                }

                return messages;
            }
        }

        /// <summary>
        /// Adds a new schema validation error.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <param name="relationType">The relation type.</param>
        /// <param name="roleType">The role .</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="kind">The kind of validation error.</param>
        /// <param name="message">The validation error message.</param>
        public void AddTableError(IObjectType objectType, IRelationType relationType, IRoleType roleType, string tableName, string columnName, SchemaValidationErrorKind kind, string message)
        {
            this.errors.Add(new TableSchemaValidationError(objectType, relationType, roleType, tableName, columnName, kind, message));
        }

        public void AddProcedureError(SchemaProcedure schemaProcedure, SchemaValidationErrorKind kind, string message)
        {
            this.errors.Add(new ProcedureSchemaValidationError(schemaProcedure, kind, message));
        }
    }
}