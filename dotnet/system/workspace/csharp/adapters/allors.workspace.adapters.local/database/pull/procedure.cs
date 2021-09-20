// <copyright file="LocalPullInstantiate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>

namespace Allors.Workspace.Adapters.Local
{
    using System;
    using System.Collections.Generic;
    using Database;
    using Database.Derivations;
    using Database.Security;

    public class Procedure : IProcedureContext
    {
        private readonly Database.Data.Procedure procedure;

        private List<IValidation> errors;

        public Procedure(ITransaction transaction, Database.Data.Procedure procedure, IAccessControl acls)
        {
            this.Transaction = transaction;
            this.procedure = procedure;
            this.AccessControl = acls;
        }

        public ITransaction Transaction { get; }

        public IAccessControl AccessControl { get; }

        public void AddError(IValidation validation)
        {
            this.errors ??= new List<IValidation>();
            this.errors.Add(validation);
        }

        public void Execute(Pull pullResponse)
        {
            if (this.procedure.Pool != null)
            {
                foreach (var kvp in this.procedure.Pool)
                {
                    var @object = kvp.Key;
                    var version = kvp.Value;

                    if (!@object.Strategy.ObjectVersion.Equals(version))
                    {
                        pullResponse.AddVersionError(@object.Id);
                    }
                }

                if (pullResponse.HasErrors)
                {
                    return;
                }
            }

            var procedures = this.Transaction.Database.Services.Get<IProcedures>();
            var proc = procedures.Get(this.procedure.Name);
            if (proc == null)
            {
                pullResponse.ErrorMessage = $"Missing procedure {this.procedure.Name}";
                return;
            }

            var input = new ProcedureInput(this.Transaction.Database.ObjectFactory, this.procedure);

            proc.Execute(this, input, pullResponse);

            if (this.errors?.Count > 0)
            {
                foreach (var derivationResult in this.errors)
                {
                    pullResponse.AddDerivationErrors(derivationResult.Errors);
                }
            }
        }
    }
}
