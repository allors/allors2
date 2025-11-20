// <copyright file="Security.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Security
    {
        public void GrantOperations(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.OperationsId, objectType, operations);

        public void GrantOperations(ObjectType objectType, IOperandType operandType, params Operations[] operations) => this.Grant(Roles.OperationsId, objectType, operandType, operations);

        public void GrantProcurement(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.ProcurementId, objectType, operations);

        public void GrantProcurement(ObjectType objectType, IOperandType operandType, params Operations[] operations) => this.Grant(Roles.ProcurementId, objectType, operandType, operations);

        public void GrantSales(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.SalesId, objectType, operations);

        public void GrantSales(ObjectType objectType, IOperandType operandType, params Operations[] operations) => this.Grant(Roles.SalesId, objectType, operandType, operations);

        private void CustomOnPostSetup()
        {
            // Default access policy
            var security = new Security(this.session);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            foreach (ObjectType @class in this.session.Database.MetaPopulation.Classes)
            {
                security.GrantAdministrator(@class, full);
                security.GrantCreator(@class, full);
                security.GrantGuest(@class, Operations.Read);
            }
        }

        private void CustomOnPreSetup()
        {
        }
    }
}
