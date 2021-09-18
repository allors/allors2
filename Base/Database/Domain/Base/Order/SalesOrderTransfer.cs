// <copyright file="SalesOrderTransfer.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class SalesOrderTransfer
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (changeSet.HasChangedRole(this, this.Meta.From))
                {
                    iteration.AddDependency(this.From, this);
                    iteration.Mark(this.From);
                }

                if (changeSet.HasChangedRole(this, this.Meta.To))
                {
                    iteration.AddDependency(this.To, this);
                    iteration.Mark(this.To);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var session = this.Session();

            if (this.ExistFrom && this.ExistInternalOrganisation && !this.ExistTo)
            {
                var acl = new DatabaseAccessControlLists(session.GetUser())[this.From];
                if (!acl.CanExecute(M.SalesOrder.DoTransfer))
                {
                    derivation.Validation.AddError(this, this.Meta.To, "No rights to transfer salesorder");
                }
                else
                {
                    this.To = this.From.Clone(this.From.Meta.SalesOrderItems);
                    this.To.TakenBy = this.InternalOrganisation;

                    // TODO: Make sure 'from' customer is also a customer in 'to' internal organisation
                    if (!this.To.TakenBy.ActiveCustomers.Contains(this.To.BillToCustomer))
                    {
                        new CustomerRelationshipBuilder(this.Strategy.Session)
                            .WithInternalOrganisation(this.To.TakenBy)
                            .WithCustomer(this.To.BillToCustomer)
                            .Build();
                    }

                    //TODO: ShipToCustomer

                    this.From.SalesOrderState = new SalesOrderStates(this.strategy.Session).Transferred;
                }
            }
        }
    }
}
