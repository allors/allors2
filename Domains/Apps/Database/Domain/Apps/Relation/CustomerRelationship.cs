// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerRelationship.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class CustomerRelationship
    {
        public int? PaymentNetDays
        {
            get
            {
                int? customerPaymentNetDays = null;
                foreach (Agreement agreement in this.RelationshipAgreements)
                {
                    foreach (AgreementTerm term in agreement.AgreementTerms)
                    {
                        if (term.TermType.Equals(new TermTypes(this.Strategy.Session).PaymentNetDays))
                        {
                            int netDays;
                            if (int.TryParse(term.TermValue, out netDays))
                            {
                                customerPaymentNetDays = netDays;
                            }

                            return customerPaymentNetDays;
                        }
                    }
                }

                return null;
            }
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            if (this.ExistCustomer)
            {
                this.YTDRevenue = 0;
                this.LastYearsRevenue = 0;

                foreach (PartyRevenue partyRevenue in this.Customer.PartyRevenuesWhereParty)
                {
                    if (partyRevenue.InternalOrganisation.Equals(this.InternalOrganisation))
                    {
                        if (partyRevenue.Year == DateTime.UtcNow.Year)
                        {
                            this.YTDRevenue += partyRevenue.Year;
                        }

                        if (partyRevenue.Year == DateTime.UtcNow.AddYears(-1).Year)
                        {
                            this.LastYearsRevenue += partyRevenue.Year;
                        }
                    }
                }
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistInternalOrganisation)
            {
                this.InternalOrganisation = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!this.ExistSubAccountNumber)
            {
                this.SubAccountNumber = this.InternalOrganisation.DeriveNextSubAccountNumber();
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistCustomer)
            {
                derivation.AddDependency(this.Customer, this);
            
                var customer = this.Customer as Organisation;
                if (customer != null)
                {
                    foreach (OrganisationContactRelationship contactRelationship in customer.OrganisationContactRelationshipsWhereOrganisation)
                    {
                        derivation.AddDependency(contactRelationship, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var customerRelationships = this.InternalOrganisation.CustomerRelationshipsWhereInternalOrganisation;
            customerRelationships.Filter.AddEquals(M.CustomerRelationship.SubAccountNumber, this.SubAccountNumber);
            if (customerRelationships.Count == 1)
            {
                if (!customerRelationships[0].Equals(this))
                {
                    derivation.Validation.AddError(new DerivationErrorUnique(derivation.Validation, this, M.CustomerRelationship.SubAccountNumber));
                }
            }
            else if (customerRelationships.Count > 1)
            {
                derivation.Validation.AddError(new DerivationErrorUnique(derivation.Validation, this, M.CustomerRelationship.SubAccountNumber));
            }

            this.AppsOnDeriveInternalOrganisationCustomer(derivation);
            this.AppsOnDeriveMembership(derivation);

            this.AppsOnDeriveAmountDue(derivation);
            this.AppsOnDeriveAmountOverDue(derivation);
            this.AppsOnDeriveRevenue(derivation);

            this.Parties = new[] { this.Customer, this.InternalOrganisation };

            if (!this.ExistCustomer || !this.ExistInternalOrganisation)
            {
                this.Delete();
            }
        }

        public void AppsOnDeriveInternalOrganisationCustomer(IDerivation derivation)
        {
            if (this.ExistCustomer && this.ExistInternalOrganisation)
            {
                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    if (!this.Customer.ExistInternalOrganisationWhereCustomer)
                    {
                        this.InternalOrganisation.AddCustomer(this.Customer);
                    }
                }

                if (this.FromDate > DateTime.UtcNow || (this.ExistThroughDate && this.ThroughDate < DateTime.UtcNow))
                {
                    if (this.Customer.ExistInternalOrganisationWhereCustomer)
                    {
                        this.InternalOrganisation.RemoveCustomer(this.Customer);
                    }
                }
            }
        }

        public void AppsOnDeriveMembership(IDerivation derivation)
        {
            if (this.ExistCustomer && this.ExistInternalOrganisation)
            {
                var customerOrganisation = this.Customer as Organisation;
                if (customerOrganisation != null && customerOrganisation.ExistContactsUserGroup)
                {
                    foreach (Person contact in customerOrganisation.ContactsUserGroup.Members)
                    {
                        customerOrganisation.ContactsUserGroup.RemoveMember(contact);
                    }

                    if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                    {
                        foreach (Person currentContact in customerOrganisation.CurrentContacts)
                        {
                            customerOrganisation.ContactsUserGroup.AddMember(currentContact);
                        }
                    }
                }
            }
        }

        public void AppsOnDeriveAmountDue(IDerivation derivation)
        {
            this.AmountDue = 0;

            if (this.ExistCustomer)
            {
                foreach (SalesInvoice salesInvoice in this.Customer.SalesInvoicesWhereBillToCustomer)
                {
                    if (salesInvoice.BilledFromInternalOrganisation.Equals(this.InternalOrganisation)
                        && !salesInvoice.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Paid))
                    {
                        if (salesInvoice.AmountPaid > 0)
                        {
                            this.AmountDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
                        }
                        else
                        {
                            foreach (SalesInvoiceItem invoiceItem in salesInvoice.InvoiceItems)
                            {
                                if (!invoiceItem.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).Paid))
                                {
                                    if (invoiceItem.ExistTotalIncVat)
                                    {
                                        this.AmountDue += invoiceItem.TotalIncVat - invoiceItem.AmountPaid;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AppsOnDeriveAmountOverDue(IDerivation derivation)
        {
            this.AmountOverDue = 0;

            if (this.ExistCustomer)
            {
                foreach (SalesInvoice salesInvoice in this.Customer.SalesInvoicesWhereBillToCustomer)
                {
                    if (salesInvoice.BilledFromInternalOrganisation.Equals(this.InternalOrganisation)
                        && !salesInvoice.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Paid))
                    {
                        var gracePeriod = salesInvoice.Store.PaymentGracePeriod;

                        if (salesInvoice.DueDate.HasValue)
                        {
                            var dueDate = salesInvoice.DueDate.Value.AddDays(gracePeriod);

                            if (DateTime.UtcNow > dueDate)
                            {
                                this.AmountOverDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
                            }
                        }
                    }
                }
            }
        }
    }
}