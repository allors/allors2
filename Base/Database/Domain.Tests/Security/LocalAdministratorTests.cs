namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Allors;
    using Xunit;

    public class LocalAdministratorTests : DomainTest
    {
        [Fact]
        public void UserGroup()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorUserGroup);

            organisation.RemoveLocalAdministratorUserGroup();
            this.Session.Derive();

            Assert.True(organisation.ExistLocalAdministratorUserGroup);
        }

        [Fact]
        public void AccessControl()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorAccessControl);
            Assert.Equal(new Roles(this.Session).LocalAdministrator, organisation.LocalAdministratorAccessControl.Role);
            Assert.Contains(organisation.LocalAdministratorUserGroup, organisation.LocalAdministratorAccessControl.SubjectGroups);

            organisation.RemoveLocalAdministratorAccessControl();

            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorAccessControl);
            Assert.Equal(new Roles(this.Session).LocalAdministrator, organisation.LocalAdministratorAccessControl.Role);
            Assert.Contains(organisation.LocalAdministratorUserGroup, organisation.LocalAdministratorAccessControl.SubjectGroups);
        }

        [Fact]
        public void SecurityToken()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            Assert.True(organisation.ExistLocalAdministratorSecurityToken);
            Assert.Contains(organisation.LocalAdministratorAccessControl, organisation.LocalAdministratorSecurityToken.AccessControls);
        }

        [Fact]
        public void LocalAdministrators()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").WithIsInternalOrganisation(true).Build();
            this.Session.Derive(true);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localAdmin")
                .WithFirstName("blue-collar")
                .WithLastName("localAdmin")
                .Build();

            organisation.AddLocalAdministrator(localAdmin);

            this.Session.Derive(true);

            Assert.Contains(localAdmin, organisation.LocalAdministratorUserGroup.Members);
        }
    }

    public class LocalAdministratorSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WorkEffortInventoryAssignmentOwnInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();

            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            this.Session.Derive(true);
            this.Session.Commit();

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workTask)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            Assert.Equal(new WorkEffortStates(this.Session).Created, workTask.WorkEffortState);

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(inventoryAssignment, localAdmin);
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.True(acl.CanWrite(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.Quantity));
            Assert.True(acl.CanWrite(M.WorkEffortInventoryAssignment.Quantity));
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.True(acl.CanWrite(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.UnitSellingPrice));
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.True(acl.CanWrite(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.True(acl.CanRead(M.WorkEffortInventoryAssignment.UnitPurchasePrice));
        }

        [Fact]
        public void WorkEffortInventoryAssignmentOtherInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var otherInternalOrganisation = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true).WithName("other internalOrganisation").Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(otherInternalOrganisation).Build();

            this.Session.Derive();

            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workTask)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);
            this.Session.Commit();

            Assert.Equal(new WorkEffortStates(this.Session).Created, workTask.WorkEffortState);

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(inventoryAssignment, localAdmin);
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.InventoryItem));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.Quantity));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.Quantity));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.BillableQuantity));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.UnitSellingPrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.UnitSellingPrice));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice));
            Assert.False(acl.CanRead(M.WorkEffortInventoryAssignment.UnitPurchasePrice));
            Assert.False(acl.CanWrite(M.WorkEffortInventoryAssignment.UnitPurchasePrice));
        }

        [Fact]
        public void WorkTaskOwnInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(workTask, localAdmin);
            Assert.True(acl.CanRead(M.WorkTask.WorkDone));
            Assert.True(acl.CanWrite(M.WorkTask.WorkDone));
        }

        [Fact]
        public void WorkTaskOtherInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var otherInternalOrganisation = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true).WithName("other internalOrganisation").Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(otherInternalOrganisation).Build();

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(workTask, localAdmin);
            Assert.False(acl.CanRead(M.WorkTask.WorkDone));
            Assert.False(acl.CanWrite(M.WorkTask.WorkDone));
        }

        [Fact]
        public void TimeEntryOwnInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(this.Session.Now())
                .WithWorkEffort(workTask)
                .Build();

            localAdmin.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(timeEntry, localAdmin);
            Assert.True(acl.CanRead(M.TimeEntry.ThroughDate));
            Assert.True(acl.CanWrite(M.TimeEntry.ThroughDate));
        }

        [Fact]
        public void TimeEntryOtherInternalOrganisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var otherInternalOrganisation = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true).WithName("other internalOrganisation").Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(otherInternalOrganisation).Build();

            var timeEntry = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(this.Session.Now())
                .WithWorkEffort(workTask)
                .Build();

            localAdmin.TimeSheetWhereWorker.AddTimeEntry(timeEntry);

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(timeEntry, localAdmin);
            Assert.False(acl.CanRead(M.TimeEntry.ThroughDate));
            Assert.False(acl.CanWrite(M.TimeEntry.ThroughDate));
        }

        [Fact]
        public void SalesInvoiceOwnOrganisation()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var salesInvoice = new SalesInvoiceBuilder(this.Session).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive();

            this.SetIdentity(localAdmin.UserName);

            Assert.True(salesInvoice.Strategy.IsNewInSession);

            var acl = new AccessControlList(salesInvoice, localAdmin);
            Assert.True(acl.CanRead(M.SalesInvoice.Description));
            Assert.True(acl.CanWrite(M.SalesInvoice.Description));

            this.Session.Commit();

            Assert.False(salesInvoice.Strategy.IsNewInSession);

            acl = new AccessControlList(salesInvoice, localAdmin);
            Assert.True(acl.CanRead(M.SalesInvoice.Description));
            Assert.True(acl.CanWrite(M.SalesInvoice.Description));
        }

        [Fact]
        public void SalesInvoiceOtherOrganisation()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build())
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var otherInternalOrganisation = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true).WithName("other internalOrganisation").Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(otherInternalOrganisation).Build();

            var salesInvoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithBilledFrom(otherInternalOrganisation)
                .WithInvoiceNumber("1")
                .WithStore(new StoreBuilder(this.Session)
                    .WithName("store")
                    .WithInternalOrganisation(otherInternalOrganisation)
                    .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                    .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                    .WithDefaultCollectionMethod(ownBankAccount)
                    .Build())
                .Build();

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive();

            this.SetIdentity(localAdmin.UserName);

            Assert.True(salesInvoice.Strategy.IsNewInSession);

            var acl = new AccessControlList(salesInvoice, localAdmin);
            Assert.False(acl.CanRead(M.SalesInvoice.Description));
            Assert.False(acl.CanWrite(M.SalesInvoice.Description));

            this.Session.Commit();

            Assert.False(salesInvoice.Strategy.IsNewInSession);

            acl = new AccessControlList(salesInvoice, localAdmin);
            Assert.False(acl.CanRead(M.SalesInvoice.Description));
            Assert.False(acl.CanWrite(M.SalesInvoice.Description));
        }

        [Fact]
        public void Organisation()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);
            this.Session.Commit();

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(internalOrganisation, localAdmin);
            Assert.True(acl.CanRead(M.Organisation.Name));
            Assert.False(acl.CanWrite(M.Organisation.Name));
        }

        [Fact]
        public void Usergroup()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            var userGroup = new UserGroups(this.Session).Administrators;

            var localAdmin = new PersonBuilder(this.Session)
                .WithUserName("localadmin")
                .WithLastName("admin")
                .Build();

            internalOrganisation.AddLocalAdministrator(localAdmin);
            new EmploymentBuilder(this.Session).WithEmployee(localAdmin).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(localAdmin);

            this.Session.Derive(true);
            this.Session.Commit();

            this.SetIdentity(localAdmin.UserName);

            var acl = new AccessControlList(userGroup, localAdmin);
            Assert.True(acl.CanRead(M.UserGroup.Members));
            Assert.False(acl.CanWrite(M.UserGroup.Members));
        }
    }
}
