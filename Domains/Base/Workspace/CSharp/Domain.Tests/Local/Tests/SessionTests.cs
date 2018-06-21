namespace Tests.Local
{
    using System;
    using System.Linq;

    using Allors.Meta;
    using Allors.Server;
    using Allors.Workspace;
    using Allors.Workspace.Domain;

    using Xunit;

    public class SessionTests : LocalTest
    {
        [Fact]
        public void UnitGet()
        {
            this.Workspace.Sync(Fixture.loadData);
            var session = new Session(this.Workspace);

            var koen = session.Get(1) as Person;

            Assert.Equal("Koen", koen.FirstName);
            Assert.Null(koen.MiddleName);
            Assert.Equal("Van Exem", koen.LastName);
            Assert.Equal(DateTime.Parse("1973-03-27T18:00:00Z"), koen.BirthDate);
            Assert.True(koen.IsStudent);

            var patrick = session.Get(2) as Person;

            Assert.Equal("Patrick", patrick.FirstName);
            Assert.Equal("De Boeck", patrick.LastName);
            Assert.Null(patrick.MiddleName);
            Assert.Null(patrick.BirthDate);
            Assert.False(patrick.IsStudent);

            var martien = session.Get(3) as Person;

            Assert.Equal("Martien", martien.FirstName);
            Assert.Equal("Knippenberg", martien.LastName);
            Assert.Equal("van", martien.MiddleName);
            Assert.Null(martien.BirthDate);
            Assert.Null(martien.IsStudent);
        }

        [Fact]
        public void UnitSet()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session1 = new Session(this.Workspace);
            var martien1 = session1.Get(3) as Person;

            var session2 = new Session(this.Workspace);
            var martien2 = session2.Get(3) as Person;

            martien2.FirstName = "Martinus";
            martien2.MiddleName = "X";

            Assert.Equal("Martien", martien1.FirstName);
            Assert.Equal("Knippenberg", martien1.LastName);
            Assert.Equal("van", martien1.MiddleName);

            Assert.Equal("Martinus", martien2.FirstName);
            Assert.Equal("Knippenberg", martien2.LastName);
            Assert.Equal("X", martien2.MiddleName);
        }

        [Fact]
        public void HasChanges()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session = new Session(this.Workspace);
            var martien = session.Get(3) as Person;
            var acme = session.Get(101) as Organisation;

            Assert.False(session.HasChanges);

            var firstName = martien.FirstName;
            martien.FirstName = firstName;

            Assert.False(session.HasChanges);

            martien.UserName = null;

            Assert.False(session.HasChanges);

            var owner = acme.Owner;
            acme.Owner = owner;

            Assert.False(session.HasChanges);

            acme.CycleOne = null;

            Assert.False(session.HasChanges);

            var employees = acme.Employees;
            acme.Employees = employees;

            Assert.False(session.HasChanges);

            employees = employees.Reverse().ToArray();
            acme.Employees = employees;

            Assert.False(session.HasChanges);

            acme.CycleMany = null;

            Assert.False(session.HasChanges);
        }
        
        [Fact]
        public void UnitSave()
        {
            this.Workspace.Sync(Fixture.loadData);
            var session = new Session(this.Workspace);

            var koen = session.Get(1) as Person;
            var patrick = session.Get(2) as Person;
            var martien = session.Get(3) as Person;

            koen.FirstName = "K";
            koen.LastName = "VE";
            martien.FirstName = "Martinus";
            martien.MiddleName = "X";

            var save = session.PushRequest();

            Assert.Equal(2, save.Objects.Length);

            var savedKoen = save.Objects.First(v => (v.I == "1"));

            Assert.Equal("1001", savedKoen.V);
            Assert.Equal(2, savedKoen.Roles.Length);

            var savedKoenFirstName = savedKoen.Roles.First(v => v.T == "FirstName");
            var savedKoenLastName = savedKoen.Roles.First(v => v.T == "LastName");

            Assert.Equal("K", savedKoenFirstName.S);
            Assert.Null(savedKoenFirstName.A);
            Assert.Null(savedKoenFirstName.R);
            Assert.Equal("VE", savedKoenLastName.S);
            Assert.Null(savedKoenLastName.A);
            Assert.Null(savedKoenLastName.R);

            var savedMartien = save.Objects.First(v => v.I == "3");

            Assert.Equal("1003", savedMartien.V);
            Assert.Equal(2, savedMartien.Roles.Length);

            var savedMartienFirstName = savedMartien.Roles.First(v => v.T == "FirstName");
            var savedMartienMiddleName = savedMartien.Roles.First(v => v.T == "MiddleName");

            Assert.Equal("Martinus", savedMartienFirstName.S);
            Assert.Null(savedMartienFirstName.A);
            Assert.Null(savedMartienFirstName.R);
            Assert.Equal("X", savedMartienMiddleName.S);
            Assert.Null(savedMartienMiddleName.A);
            Assert.Null(savedMartienMiddleName.R);
        }

        [Fact]
        public void OneGet()
        {
            this.Workspace.Sync(Fixture.loadData);
            var session = new Session(this.Workspace);

            var koen = session.Get(1) as Person;
            var patrick = session.Get(2) as Person;
            var martien = session.Get(3) as Person;

            var acme = session.Get(101) as Organisation;
            var ocme = session.Get(102) as Organisation;
            var icme = session.Get(103) as Organisation;

            Assert.Equal(koen, acme.Owner);
            Assert.Equal(patrick, ocme.Owner);
            Assert.Equal(martien, icme.Owner);

            Assert.Null(acme.Manager);
            Assert.Null(ocme.Manager);
            Assert.Null(icme.Manager);
        }

        [Fact]
        public void OneSet()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session1 = new Session(this.Workspace);

            var session2 = new Session(this.Workspace);

            var koen1 = session1.Get(1) as Person;
            var patrick1 = session1.Get(2) as Person;
            var martien1 = session1.Get(3) as Person;

            var acme1 = session1.Get(101) as Organisation;
            var ocme1 = session1.Get(102) as Organisation;
            var icme1 = session1.Get(103) as Organisation;

            var koen2 = session2.Get(1) as Person;
            var patrick2 = session2.Get(2) as Person;
            var martien2 = session2.Get(3) as Person;

            var acme2 = session2.Get(101) as Organisation;
            var ocme2 = session2.Get(102) as Organisation;
            var icme2 = session2.Get(103) as Organisation;

            acme2.Owner = martien2;
            ocme2.Owner = null;
            acme2.Manager = patrick2;

            Assert.Equal(koen1, acme1.Owner);
            Assert.Equal(patrick1, ocme1.Owner);
            Assert.Equal(martien1, icme1.Owner);

            Assert.Null(acme1.Manager);
            Assert.Null(ocme1.Manager);
            Assert.Null(icme1.Manager);

            Assert.Equal(martien2, acme2.Owner); //x
            Assert.Null(ocme2.Owner);
            Assert.Equal(martien2, icme2.Owner);

            Assert.Equal(patrick2, acme2.Manager); //x
            Assert.Null(ocme2.Manager);
            Assert.Null(icme2.Manager);
        }

        [Fact]
        public void OneSave()
        {
            this.Workspace.Sync(Fixture.loadData);
            var session = new Session(this.Workspace);

            var koen = session.Get(1) as Person;
            var patrick = session.Get(2) as Person;
            var martien = session.Get(3) as Person;

            var acme = session.Get(101) as Organisation;
            var ocme = session.Get(102) as Organisation;
            var icme = session.Get(103) as Organisation;

            acme.Owner = martien;
            ocme.Owner = null;

            acme.Manager = patrick;

            var save = session.PushRequest();

            Assert.Equal(2, save.Objects.Length);

            var savedAcme = save.Objects.First(v => v.I == "101");

            Assert.Equal("1101", savedAcme.V);
            Assert.Equal(2, savedAcme.Roles.Length);

            var savedAcmeOwner = savedAcme.Roles.First(v => v.T == "Owner");
            var savedAcmeManager = savedAcme.Roles.First(v => v.T == "Manager");

            Assert.Equal("3", savedAcmeOwner.S);
            Assert.Null(savedAcmeOwner.A);
            Assert.Null(savedAcmeOwner.R);
            Assert.Equal("2", savedAcmeManager.S);
            Assert.Null(savedAcmeManager.A);
            Assert.Null(savedAcmeManager.R);

            var savedOcme = save.Objects.First(v => v.I == "102");

            Assert.Equal("1102", savedOcme.V);
            Assert.Single(savedOcme.Roles);

            var savedOcmeOwner = savedOcme.Roles.First(v => v.T == "Owner");

            Assert.Null(savedOcmeOwner.S);
            Assert.Null(savedOcmeOwner.A);
            Assert.Null(savedOcmeOwner.R);
        }

        [Fact]
        public void ManyGet()
        {
            this.Workspace.Sync(Fixture.loadData);
            var session = new Session(this.Workspace);

            var koen = (Person)session.Get(1);
            var patrick = (Person)session.Get(2);
            var martien = (Person)session.Get(3);

            var acme = (Organisation)session.Get(101);
            var ocme = (Organisation)session.Get(102);
            var icme = (Organisation)session.Get(103);

            Assert.Equal(3, acme.Employees.Length);
            Assert.Contains(koen, acme.Employees);
            Assert.Contains(martien, acme.Employees);
            Assert.Contains(patrick, acme.Employees);

            Assert.Single(ocme.Employees);
            Assert.Contains(koen, ocme.Employees);

            Assert.Empty(icme.Employees);

            Assert.Empty(acme.Shareholders);
            Assert.Empty(ocme.Shareholders);
            Assert.Empty(icme.Shareholders);
        }

        [Fact]
        public void ManySet()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session1 = new Session(this.Workspace);

            var session2 = new Session(this.Workspace);

            var koen1 = session1.Get(1) as Person;
            var patrick1 = session1.Get(2) as Person;
            var martien1 = session1.Get(3) as Person;

            var acme1 = session1.Get(101) as Organisation;
            var ocme1 = session1.Get(102) as Organisation;
            var icme1 = session1.Get(103) as Organisation;

            var koen2 = session2.Get(1) as Person;
            var patrick2 = session2.Get(2) as Person;
            var martien2 = session2.Get(3) as Person;

            var acme2 = session2.Get(101) as Organisation;
            var ocme2 = session2.Get(102) as Organisation;
            var icme2 = session2.Get(103) as Organisation;

            acme2.Employees = null;
            icme2.Employees = new[] { koen2, patrick2, martien2 };

            Assert.Equal(3, acme1.Employees.Length);
            Assert.Contains(koen1, acme1.Employees);
            Assert.Contains(martien1, acme1.Employees);
            Assert.Contains(patrick1, acme1.Employees);

            Assert.Single(ocme1.Employees);
            Assert.Contains(koen1, ocme1.Employees);

            Assert.Empty(icme1.Employees);

            Assert.Empty(acme2.Employees);

            Assert.Single(ocme2.Employees);
            Assert.Contains(koen2, ocme2.Employees);

            Assert.Equal(3, icme2.Employees.Length);
            Assert.Contains(koen2, icme2.Employees);
            Assert.Contains(martien2, icme2.Employees);
            Assert.Contains(patrick2, icme2.Employees);
        }

        [Fact]
        public void ManySaveWithExistingObjects()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session = new Session(this.Workspace);

            var koen = session.Get(1) as Person;
            var patrick = session.Get(2) as Person;
            var martien = session.Get(3) as Person;

            var acme = session.Get(101) as Organisation;
            var ocme = session.Get(102) as Organisation;
            var icme = session.Get(103) as Organisation;

            acme.Employees = null;
            ocme.Employees = new[] { martien, patrick };
            icme.Employees = new[] { koen, patrick, martien };

            var save = session.PushRequest();

            Assert.Empty(save.NewObjects);
            Assert.Equal(3, save.Objects.Length);

            var savedAcme = save.Objects.First(v => v.I == "101");

            Assert.Equal("1101", savedAcme.V);
            Assert.Single(savedAcme.Roles);

            var savedAcmeEmployees = savedAcme.Roles.First(v => v.T == "Employees");

            Assert.Null(savedAcmeEmployees.S);
            Assert.Empty(savedAcmeEmployees.A);
            Assert.Contains("1", savedAcmeEmployees.R);
            Assert.Contains("2", savedAcmeEmployees.R);
            Assert.Contains("3", savedAcmeEmployees.R);
            var savedOcme = save.Objects.First(v => v.I == "102");

            Assert.Equal("1102", savedOcme.V);
            Assert.Single(savedOcme.Roles);

            var savedOcmeEmployees = savedOcme.Roles.First(v => v.T == "Employees");

            Assert.Null(savedOcmeEmployees.S);
            Assert.Equal(2, savedOcmeEmployees.A.Length);
            Assert.Contains("2", savedOcmeEmployees.A);
            Assert.Contains("3", savedOcmeEmployees.A);

            Assert.Single(savedOcmeEmployees.R);
            Assert.Contains("1", savedOcmeEmployees.R);

            var savedIcme = save.Objects.First(v => v.I == "103");

            Assert.Equal("1103", savedIcme.V);
            Assert.Single(savedIcme.Roles);

            var savedIcmeEmployees = savedIcme.Roles.First(v => v.T == "Employees");

            Assert.Null(savedIcmeEmployees.S);
            Assert.Equal(3, savedIcmeEmployees.A.Length);
            Assert.Contains("1", savedIcmeEmployees.A);
            Assert.Contains("2", savedIcmeEmployees.A);
            Assert.Contains("3", savedIcmeEmployees.A);
            Assert.Null(savedIcmeEmployees.R);
        }

        [Fact]
        public void ManySaveWithNewObjects()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session = new Session(this.Workspace);

            var martien = session.Get(3) as Person;

            var mathijs = session.Create(M.Person.Class) as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            var acme2 = session.Create(M.Organisation.Class) as Organisation;
            acme2.Name = "Acme 2";
            acme2.Manager = mathijs;
            acme2.AddEmployee(mathijs);

            var acme3 = session.Create(M.Organisation.Class) as Organisation;
            acme3.Name = "Acme 3";
            acme3.Manager = martien;
            acme3.AddEmployee(martien);

            var save = session.PushRequest();

            Assert.Equal(3, save.NewObjects.Length);
            Assert.Empty(save.Objects);
            {
                var savedMathijs = save.NewObjects.First(v => v.NI == mathijs.NewId?.ToString());

                Assert.Equal("Person", savedMathijs.T);
                Assert.Equal(2, savedMathijs.Roles.Length);

                var savedMathijsFirstName = savedMathijs.Roles.First(v => v.T == "FirstName");
                Assert.Equal("Mathijs", savedMathijsFirstName.S);

                var savedMathijsLastName = savedMathijs.Roles.First(v => v.T == "LastName");
                Assert.Equal("Verwer", savedMathijsLastName.S);
            }

            {
                var savedAcme2 = save.NewObjects.First(v => v.NI == acme2.NewId?.ToString());

                Assert.Equal("Organisation", savedAcme2.T);
                Assert.Equal(3, savedAcme2.Roles.Length);

                var savedAcme2Manager = savedAcme2.Roles.First(v => v.T == "Manager");

                Assert.Equal(mathijs.NewId.ToString(), savedAcme2Manager.S);

                var savedAcme2Employees = savedAcme2.Roles.First(v => v.T == "Employees");

                Assert.Null(savedAcme2Employees.S);
                Assert.Contains(mathijs.NewId?.ToString(), savedAcme2Employees.A);
                Assert.Null(savedAcme2Employees.R);
            }

            {
                var savedAcme3 = save.NewObjects.First(v => v.NI == acme3.NewId?.ToString());

                Assert.Equal("Organisation", savedAcme3.T);
                Assert.Equal(3, savedAcme3.Roles.Length);

                var savedAcme3Manager = savedAcme3.Roles.First(v => v.T == "Manager");

                Assert.Equal("3", savedAcme3Manager.S);

                var savedAcme3Employees = savedAcme3.Roles.First(v => v.T == "Employees");

                Assert.Null(savedAcme3Employees.S);
                Assert.Contains("3", savedAcme3Employees.A);
                Assert.Null(savedAcme3Employees.R);
            }

        }

        [Fact]
        public void SyncWithNewObjects()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session = new Session(this.Workspace);

            var martien = session.Get(3) as Person;

            var mathijs = session.Create(M.Person.Class) as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            var acme2 = session.Create(M.Organisation.Class) as Organisation;
            acme2.Name = "Acme 2";
            acme2.Owner = martien;
            acme2.Manager = mathijs;
            acme2.AddEmployee(martien);
            acme2.AddEmployee(mathijs);

            session.Reset();

            //Assert.Null(mathijs.Id);
            Assert.True(mathijs.NewId < 0);
            Assert.Null(mathijs.FirstName);
            Assert.Null(mathijs.LastName);

            //Assert.Null(acme2.Id);
            Assert.True(acme2.NewId < 0);
            Assert.Null(acme2.Owner);
            Assert.Null(acme2.Manager);

            Assert.Empty(acme2.Employees);
        }

        [Fact]
        public void Onsaved()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session = new Session(this.Workspace);

            var saveResponse = new PushResponse();

            session.PushResponse(saveResponse);

            var mathijs = session.Create(M.Person.Class) as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            var newId = mathijs.NewId.Value;

            saveResponse = new PushResponse
                               {
                                   NewObjects = new[] { new PushResponseNewObject { I = "10000", NI = newId.ToString() } }
                               };

            session.PushResponse(saveResponse);

            Assert.Null(mathijs.NewId);
            Assert.Equal(10000, mathijs.Id);
            Assert.Equal("Person", mathijs.ObjectType.Name);

            mathijs = session.Get(10000) as Person;

            Assert.NotNull(mathijs);

            var exceptionThrown = false;
            try
            {
                session.Get(newId);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }

        //[Fact]
        //public void methodCanExecute()
        //{
        //    var workspace = new Workspace();
        //    workspace.Sync(Fixture.loadData);

        //    var session = new Session(workspace);

        //    var acme = session.Get("101") as Organisation;
        //    var ocme = session.Get("102") as Organisation;
        //    var icme = session.Get("102") as Organisation;

        //    Assert.True(acme.CanExecuteJustDoIt);
        //    this.isFalse(ocme.CanExecuteJustDoIt);
        //    this.isFalse(icme.CanExecuteJustDoIt);
        //}

        [Fact]
        public void Get()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session = new Session(this.Workspace);

            var acme = (Organisation)session.Create(M.Organisation);

            var acmeAgain = session.Get(acme.Id);

            Assert.Equal(acme, acmeAgain);

            acmeAgain = session.Get(acme.NewId.Value);

            Assert.Equal(acme, acmeAgain);
        }
    }
}