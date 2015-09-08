// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisation.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class InternalOrganisation
    {
        // TODO: Cascading delete

        //public override void RemovePaymentMethod(PaymentMethod value)
        //{
        //    if (value.Equals(this.DefaultPaymentMethod))
        //    {
        //        this.RemoveDefaultPaymentMethod();
        //    }

        //    base.RemovePaymentMethod(value);
        //}
        public void AppsOnDeriveCurrentContacts(IDerivation derivation)
        {
            this.RemoveCurrentContacts();
        }

        public void AppsOnDeriveInactiveContacts(IDerivation derivation)
        {
            this.RemoveInactiveContacts();
        }

        public void AppsOnDeriveCurrentOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveCurrentPartyContactMechanisms();
        }

        public void AppsOnDeriveInactiveOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveInactivePartyContactMechanisms();
        }

        public void AppsOnDeriveCurrentPartyContactMechanisms(IDerivation derivation)
        {
            this.RemoveCurrentPartyContactMechanisms();
        }

        public void AppsOnDeriveInactivePartyContactMechanisms(IDerivation derivation)
        {
            this.RemoveInactivePartyContactMechanisms();
        }

        public int DeriveNextSubAccountNumber()
        {
            var next = this.SubAccountCounter.NextElfProefValue();
            return next;
        }

        public string DeriveNextPurchaseInvoiceNumber()
        {
            var purchaseInvoiceNumber = this.PurchaseInvoiceCounter.NextValue();
            return string.Concat(this.PurchaseInvoiceNumberPrefix, purchaseInvoiceNumber);
        }

        public string DeriveNextShipmentNumber()
        {
            var shipmentNumber = this.IncomingShipmentCounter.NextValue();
            return string.Concat(this.IncomingShipmentNumberPrefix, shipmentNumber);
        }

        public string DeriveNextPurchaseOrderNumber()
        {
            var purchaseOrderNumber = this.PurchaseInvoiceCounter.NextValue();
            return string.Concat(this.PurchaseOrderNumberPrefix, purchaseOrderNumber);
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseInvoiceCounter)
            {
                this.PurchaseInvoiceCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistQuoteCounter)
            {
                this.QuoteCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistPurchaseOrderCounter)
            {
                this.PurchaseOrderCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistIncomingShipmentCounter)
            {
                this.IncomingShipmentCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistSubAccountCounter)
            {
                this.SubAccountCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistDoAccounting)
            {
                this.DoAccounting = false;
            }

            if (!this.ExistInvoiceSequence)
            {
                this.InvoiceSequence = new InvoiceSequences(this.Strategy.Session).RestartOnFiscalYear;
            }

            if (!this.ExistFiscalYearStartMonth)
            {
                this.FiscalYearStartMonth = 1;
            }

            if (!this.ExistFiscalYearStartDay)
            {
                this.FiscalYearStartDay = 1;
            }
            
            if (this.Strategy.Session.Extent<TemplatePurpose>().Count > 0 &&
                this.Strategy.Session.Extent<StringTemplate>().Count > 0)
            {
                if (!this.ExistPurchaseOrderTemplates)
                {
                    var templates = this.ExistLocale ? this.Locale.StringTemplatesWhereLocale : Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale;
                    var template = templates.FirstOrDefault(t => t.ExistTemplatePurpose && t.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).PurchaseOrder));
                    this.AddPurchaseOrderTemplate(template);
                }

                if (!this.ExistQuoteTemplates)
                {
                    var templates = this.ExistLocale ? this.Locale.StringTemplatesWhereLocale : Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale;
                    var template = templates.FirstOrDefault(t => t.ExistTemplatePurpose && t.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).Quote));
                    this.AddQuoteTemplate(template);
                }

                if (!this.ExistPickListTemplates)
                {
                    var templates = this.ExistLocale ? this.Locale.StringTemplatesWhereLocale : Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale;
                    var template = templates.FirstOrDefault(t => t.ExistTemplatePurpose && t.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).PickList));
                    this.AddPickListTemplate(template);
                }

                if (!this.ExistPackagingSlipTemplates)
                {
                    var templates = this.ExistLocale ? this.Locale.StringTemplatesWhereLocale : Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale;
                    var template = templates.FirstOrDefault(t => t.ExistTemplatePurpose && t.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).PackagingSlip));
                    this.AddPackagingSlipTemplate(template);
                }

                if (!this.ExistPurchaseShipmentTemplates)
                {
                    var templates = this.ExistLocale ? this.Locale.StringTemplatesWhereLocale : Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale;
                    var template = templates.FirstOrDefault(t => t.ExistTemplatePurpose && t.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).PurchaseShipment));
                    this.AddPurchaseShipmentTemplate(template);
                }
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistClientRelationshipsWhereClient)
                {
                    foreach (ClientRelationship relationship in this.ClientRelationshipsWhereInternalOrganisation)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (CustomerRelationship relationship in this.CustomerRelationshipsWhereInternalOrganisation)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (DistributionChannelRelationship relationship in this.DistributionChannelRelationshipsWhereInternalOrganisation)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (Employment relationship in this.EmploymentsWhereEmployer)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (Partnership relationship in this.PartnershipsWhereInternalOrganisation)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (SalesRepRelationship relationship in this.SalesRepRelationshipsWhereInternalOrganisation)
                    {
                        derivation.AddDependency(relationship, this);
                    }

                    foreach (SupplierRelationship relationship in this.SupplierRelationshipsWhereInternalOrganisation)
                    {
                        derivation.AddDependency(relationship, this);
                    }
                }

                foreach (PaymentMethod paymentMethod in this.PaymentMethods)
                {
                    derivation.AddDependency(paymentMethod, this);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyName = this.Name;

            if (this.ExistPreviousCurrency)
            {
                derivation.Log.AssertAreEqual(this, InternalOrganisations.Meta.PreferredCurrency, InternalOrganisations.Meta.PreviousCurrency);
            }
            else
            {
                this.PreviousCurrency = this.PreferredCurrency;
            }

            this.BillingAddress = null;
            this.BillingInquiriesFax = null;
            this.BillingInquiriesPhone = null;
            this.CellPhoneNumber = null;
            this.GeneralFaxNumber = null;
            this.GeneralPhoneNumber = null;
            this.HeadQuarter = null;
            this.HomeAddress = null;
            this.InternetAddress = null;
            this.OrderAddress = null;
            this.OrderInquiriesFax = null;
            this.OrderInquiriesPhone = null;
            this.PersonalEmailAddress = null;
            this.SalesOffice = null;
            this.ShippingAddress = null;
            this.ShippingInquiriesFax = null;
            this.ShippingAddress = null;

            foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
            {
                if (partyContactMechanism.UseAsDefault)
                {
                    if (partyContactMechanism.ContactPurpose.IsBillingAddress)
                    {
                        this.BillingAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsBillingInquiriesFax)
                    {
                        this.BillingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsBillingInquiriesPhone)
                    {
                        this.BillingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsCellPhoneNumber)
                    {
                        this.CellPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralFaxNumber)
                    {
                        this.GeneralFaxNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsGeneralPhoneNumber)
                    {
                        this.GeneralPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsHeadQuarters)
                    {
                        this.HeadQuarter = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsHomeAddress)
                    {
                        this.HomeAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsInternetAddress)
                    {
                        this.InternetAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderAddress)
                    {
                        this.OrderAddress = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderInquiriesFax)
                    {
                        this.OrderInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsOrderInquiriesPhone)
                    {
                        this.OrderInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsPersonalEmailAddress)
                    {
                        this.PersonalEmailAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsSalesOffice)
                    {
                        this.SalesOffice = partyContactMechanism.ContactMechanism;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingAddress)
                    {
                        this.ShippingAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingInquiriesFax)
                    {
                        this.ShippingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                        continue;
                    }

                    if (partyContactMechanism.ContactPurpose.IsShippingInquiriesPhone)
                    {
                        this.ShippingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }
                }
            }

            if (this.ExistDefaultPaymentMethod && !this.PaymentMethods.Contains(this.DefaultPaymentMethod))
            {
                this.AddPaymentMethod(this.DefaultPaymentMethod);
            }

            if (!this.ExistDefaultPaymentMethod && this.PaymentMethods.Count == 1)
            {
                this.DefaultPaymentMethod = this.PaymentMethods.First;
            }

            if (!this.ExistOwnerSecurityToken)
            {
                var securityToken = new SecurityTokenBuilder(this.Strategy.Session).Build();
                this.OwnerSecurityToken = securityToken;

                this.AddSecurityToken(this.OwnerSecurityToken);
                this.AddSecurityToken(Singleton.Instance(this.Strategy.Session).AdministratorSecurityToken);
            }

            this.DeriveEmployeeUserGroups(derivation);
        }

        public void AppsOnDeriveEmployeeUserGroups(IDerivation derivation)
        {
            var employeeUserGroupsByName = new Dictionary<string, UserGroup>();
            foreach (UserGroup userGroup in this.UserGroupsWhereParty)
            {
                employeeUserGroupsByName.Add(userGroup.Name, userGroup);
            }

            foreach (Role role in this.EmployeeRoles)
            {
                var userGroupName = string.Format("{0} for {1})", role.Name, this.Name);

                if (!employeeUserGroupsByName.ContainsKey(userGroupName))
                {
                    var userGroup = new UserGroupBuilder(this.Strategy.Session)
                        .WithName(userGroupName)
                        .WithParty(this)
                        .WithParent(role.UserGroupWhereRole)
                        .Build();

                    new AccessControlBuilder(this.Strategy.Session).WithRole(role).WithSubjectGroup(userGroup).WithObject(this.OwnerSecurityToken).Build();
                }
            }
        }

        public void AppsStartNewFiscalYear()
        {
            if (this.ExistActualAccountingPeriod && this.ActualAccountingPeriod.Active)
            {
                return;
            }

            int year = DateTime.UtcNow.Year;
            if (this.ExistActualAccountingPeriod)
            {
                year = this.ActualAccountingPeriod.FromDate.Date.Year + 1;
            }

            var fromDate = DateTimeFactory.CreateDate(year, this.FiscalYearStartMonth, this.FiscalYearStartDay).Date;

            var yearPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Year)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddYears(1).AddSeconds(-1).Date)
                .Build();

            var semesterPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Semester)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddMonths(6).AddSeconds(-1).Date)
                .WithParent(yearPeriod)
                .Build();

            var trimesterPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Trimester)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddMonths(3).AddSeconds(-1).Date)
                .WithParent(semesterPeriod)
                .Build();

            var monthPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Month)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddMonths(1).AddSeconds(-1).Date)
                .WithParent(trimesterPeriod)
                .Build();

            this.AddAccountingPeriod(yearPeriod);
            this.AddAccountingPeriod(semesterPeriod);
            this.AddAccountingPeriod(trimesterPeriod);
            this.AddAccountingPeriod(monthPeriod);

            this.ActualAccountingPeriod = monthPeriod;
        }

        private int NextValidElevenTestNumer(int previous)
        {
            var candidate = previous.ToString();
            var valid = false;

            while (!valid)
            {
                candidate = previous.ToString();
                var sum = 0;
                for (var i = candidate.Length; i > 0; i--)
                {
                    sum += int.Parse(candidate.Substring(candidate.Length - i, 1)) * i;
                }
                
                valid = (sum % 11 == 0);
                
                if (!valid)
                {
                    previous++;
                }
            }

            return int.Parse(candidate);
        }

        public bool IsPerson {
            get
            {
                return false;
            }
        }

        public bool IsOrganisation {
            get
            {
                return false;
            }
        }

        public List<string> Roles
        {
            get
            {
                return new List<string>() {"Internal organisation"};
            }
        }
    }
}