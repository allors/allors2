namespace Tests.Local
{
    using System;
    using System.Linq;

    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Meta;

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
            Assert.Equal(null, koen.MiddleName);
            Assert.Equal("Van Exem", koen.LastName);
            Assert.Equal(DateTime.Parse("1973-03-27T18:00:00Z"), koen.BirthDate);
            Assert.Equal(true, koen.IsStudent);

            var patrick = session.Get(2) as Person;

            Assert.Equal("Patrick", patrick.FirstName);
            Assert.Equal("De Boeck", patrick.LastName);
            Assert.Equal(null, patrick.MiddleName);
            Assert.Equal(null, patrick.BirthDate);
            Assert.Equal(false, patrick.IsStudent);

            var martien = session.Get(3) as Person;

            Assert.Equal("Martien", martien.FirstName);
            Assert.Equal("Knippenberg", martien.LastName);
            Assert.Equal("van", martien.MiddleName);
            Assert.Equal(null, martien.BirthDate);
            Assert.Equal(null, martien.IsStudent);
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

            Assert.Equal(2, save.objects.Count);

            var savedKoen = save.objects.First(v => (v.i == "1"));

            Assert.Equal("1001", savedKoen.v);
            Assert.Equal(2, savedKoen.roles.Length);

            var savedKoenFirstName = savedKoen.roles.First(v => v.t == "FirstName");
            var savedKoenLastName = savedKoen.roles.First(v => v.t == "LastName");

            Assert.Equal("K", savedKoenFirstName.s);
            Assert.Null(savedKoenFirstName.a);
            Assert.Null(savedKoenFirstName.r);
            Assert.Equal("VE", savedKoenLastName.s);
            Assert.Null(savedKoenLastName.a);
            Assert.Null(savedKoenLastName.r);

            var savedMartien = save.objects.First(v => v.i == "3");

            Assert.Equal("1003", savedMartien.v);
            Assert.Equal(2, savedMartien.roles.Length);

            var savedMartienFirstName = savedMartien.roles.First(v => v.t == "FirstName");
            var savedMartienMiddleName = savedMartien.roles.First(v => v.t == "MiddleName");

            Assert.Equal("Martinus", savedMartienFirstName.s);
            Assert.Null(savedMartienFirstName.a);
            Assert.Null(savedMartienFirstName.r);
            Assert.Equal("X", savedMartienMiddleName.s);
            Assert.Null(savedMartienMiddleName.a);
            Assert.Null(savedMartienMiddleName.r);
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

            Assert.Equal(null, acme.Manager);
            Assert.Equal(null, ocme.Manager);
            Assert.Equal(null, icme.Manager);
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

            Assert.Equal(null, acme1.Manager);
            Assert.Equal(null, ocme1.Manager);
            Assert.Equal(null, icme1.Manager);

            Assert.Equal(martien2, acme2.Owner); //x
            Assert.Equal(null, ocme2.Owner);
            Assert.Equal(martien2, icme2.Owner);

            Assert.Equal(patrick2, acme2.Manager); //x
            Assert.Equal(null, ocme2.Manager);
            Assert.Equal(null, icme2.Manager);
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

            Assert.Equal(2, save.objects.Count);

            var savedAcme = save.objects.First(v => v.i == "101");

            Assert.Equal("1101", savedAcme.v);
            Assert.Equal(2, savedAcme.roles.Length);

            var savedAcmeOwner = savedAcme.roles.First(v => v.t == "Owner");
            var savedAcmeManager = savedAcme.roles.First(v => v.t == "Manager");

            Assert.Equal("3", savedAcmeOwner.s);
            Assert.Null(savedAcmeOwner.a);
            Assert.Null(savedAcmeOwner.r);
            Assert.Equal("2", savedAcmeManager.s);
            Assert.Null(savedAcmeManager.a);
            Assert.Null(savedAcmeManager.r);

            var savedOcme = save.objects.First(v => v.i == "102");

            Assert.Equal("1102", savedOcme.v);
            Assert.Equal(1, savedOcme.roles.Length);

            var savedOcmeOwner = savedOcme.roles.First(v => v.t == "Owner");

            Assert.Equal(null, savedOcmeOwner.s);
            Assert.Null(savedOcmeOwner.a);
            Assert.Null(savedOcmeOwner.r);
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

            Assert.Equal(1, ocme.Employees.Length);
            Assert.Contains(koen, ocme.Employees);

            Assert.Equal(0, icme.Employees.Length);

            Assert.Equal(0, acme.Shareholders.Length);
            Assert.Equal(0, ocme.Shareholders.Length);
            Assert.Equal(0, icme.Shareholders.Length);
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
            icme2.Employees = new[] {koen2, patrick2, martien2};

            Assert.Equal(3, acme1.Employees.Length);
            Assert.Contains(koen1, acme1.Employees);
            Assert.Contains(martien1, acme1.Employees);
            Assert.Contains(patrick1, acme1.Employees);

            Assert.Equal(1, ocme1.Employees.Length);
            Assert.Contains(koen1, ocme1.Employees);

            Assert.Equal(0, icme1.Employees.Count());

            Assert.Equal(0, acme2.Employees.Count());

            Assert.Equal(1, ocme2.Employees.Length);
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
            ocme.Employees = new[] {martien, patrick};
            icme.Employees = new[] {koen, patrick, martien};

            var save = session.PushRequest();

            Assert.Equal(0, save.newObjects.Count);
            Assert.Equal(3, save.objects.Count);

            var savedAcme = save.objects.First(v => v.i == "101");

            Assert.Equal(savedAcme.v, "1101");
            Assert.Equal(savedAcme.roles.Length, 1);

            var savedAcmeEmployees = savedAcme.roles.First(v => v.t == "Employees");

            Assert.Null(savedAcmeEmployees.s);
            Assert.Equal(0, savedAcmeEmployees.a.Length);
            Assert.Contains("1", savedAcmeEmployees.r);
            Assert.Contains("2", savedAcmeEmployees.r);
            Assert.Contains("3", savedAcmeEmployees.r);
            var savedOcme = save.objects.First(v => v.i == "102");

            Assert.Equal("1102", savedOcme.v);
            Assert.Equal(1, savedOcme.roles.Length);

            var savedOcmeEmployees = savedOcme.roles.First(v => v.t == "Employees");

            Assert.Null(savedOcmeEmployees.s);
            Assert.Equal(2, savedOcmeEmployees.a.Length);
            Assert.Contains("2", savedOcmeEmployees.a);
            Assert.Contains("3", savedOcmeEmployees.a);

            Assert.Equal(1, savedOcmeEmployees.r.Length);
            Assert.Contains("1", savedOcmeEmployees.r);

            var savedIcme = save.objects.First(v => v.i == "103");

            Assert.Equal("1103", savedIcme.v);
            Assert.Equal(1, savedIcme.roles.Length);

            var savedIcmeEmployees = savedIcme.roles.First(v => v.t == "Employees");

            Assert.Null(savedIcmeEmployees.s);
            Assert.Equal(3, savedIcmeEmployees.a.Length);
            Assert.Contains("1", savedIcmeEmployees.a);
            Assert.Contains("2", savedIcmeEmployees.a);
            Assert.Contains("3", savedIcmeEmployees.a);
            Assert.Null(savedIcmeEmployees.r);
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

            Assert.Equal(3, save.newObjects.Count);
            Assert.Equal(0, save.objects.Count);
            {
                var savedMathijs = save.newObjects.First(v => v.ni == mathijs.NewId?.ToString());

                Assert.Equal("Person", savedMathijs.t);
                Assert.Equal(2, savedMathijs.roles.Length);

                var savedMathijsFirstName = savedMathijs.roles.First(v => v.t == "FirstName");
                Assert.Equal("Mathijs", savedMathijsFirstName.s);

                var savedMathijsLastName = savedMathijs.roles.First(v => v.t == "LastName");
                Assert.Equal("Verwer", savedMathijsLastName.s);
            }

            {
                var savedAcme2 = save.newObjects.First(v => v.ni == acme2.NewId?.ToString());

                Assert.Equal("Organisation", savedAcme2.t);
                Assert.Equal(3, savedAcme2.roles.Length);

                var savedAcme2Manager = savedAcme2.roles.First(v => v.t == "Manager");

                Assert.Equal(mathijs.NewId.ToString(), savedAcme2Manager.s);

                var savedAcme2Employees = savedAcme2.roles.First(v => v.t == "Employees");

                Assert.Null(savedAcme2Employees.s);
                Assert.Contains(mathijs.NewId?.ToString(), savedAcme2Employees.a);
                Assert.Null(savedAcme2Employees.r);
            }

            {
                var savedAcme3 = save.newObjects.First(v => v.ni == acme3.NewId?.ToString());

                Assert.Equal("Organisation", savedAcme3.t);
                Assert.Equal(3, savedAcme3.roles.Length);

                var savedAcme3Manager = savedAcme3.roles.First(v => v.t == "Manager");

                Assert.Equal("3", savedAcme3Manager.s);

                var savedAcme3Employees = savedAcme3.roles.First(v => v.t == "Employees");

                Assert.Null(savedAcme3Employees.s);
                Assert.Contains("3", savedAcme3Employees.a);
                Assert.Null(savedAcme3Employees.r);
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
            Assert.Equal(null, mathijs.FirstName);
            Assert.Equal(null, mathijs.LastName);

            //Assert.Null(acme2.Id);
            Assert.True(acme2.NewId < 0);
            Assert.Equal(null, acme2.Owner);
            Assert.Equal(null, acme2.Manager);

            Assert.Equal(0, acme2.Employees.Count());
        }

        [Fact]
        public void Onsaved()
        {
            this.Workspace.Sync(Fixture.loadData);

            var session = new Session(this.Workspace);

            var saveResponse = new PushResponse { hasErrors = false };

            session.PushResponse(saveResponse);

            var mathijs = session.Create(M.Person.Class) as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            var newId = mathijs.NewId.Value;

            saveResponse = new PushResponse
                               {
                                   hasErrors = false,
                                   newObjects = new[] { new PushResponseNewObject { i = "10000", ni = newId.ToString() } }
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