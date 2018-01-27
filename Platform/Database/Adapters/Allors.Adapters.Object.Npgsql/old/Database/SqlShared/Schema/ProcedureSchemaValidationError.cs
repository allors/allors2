// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcedureSchemaValidationError.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    public class ProcedureSchemaValidationError : ISchemaValidationError
    {
        private readonly SchemaProcedure schemaProcedure;
        private readonly SchemaValidationErrorKind kind;
        private readonly string message;

        public ProcedureSchemaValidationError(SchemaProcedure schemaProcedure, SchemaValidationErrorKind kind, string message)
        {
            this.schemaProcedure = schemaProcedure;
            this.kind = kind;
            this.message = message;
        }

        public SchemaProcedure SchemaProcedure
        {
            get
            {
                return this.schemaProcedure;
            }
        }

        public SchemaValidationErrorKind Kind
        {
            get
            {
                return this.kind;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public override string ToString()
        {
            return this.SchemaProcedure.ToString();
        }
    }
}