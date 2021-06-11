// <copyright file="SessionTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Mock
{
    using System;
    using System.Linq;
    using Allors.Protocol.Data;
    using Allors.Protocol.Remote.Push;
    using Allors.Workspace;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Xunit;

    public class SessionTests : MockTest
    {
        [Fact]
        public void UnitGet()
        {
            this.Workspace.Sync(Fixture.LoadData);
            var session = new Session(this.Workspace);

            var koen = session.Get(1) as Person;

            Assert.Equal("Koen", koen.FirstName);
            Assert.Null(koen.MiddleName);
            Assert.Equal("Van Exem", koen.LastName);
            Assert.Equal(UnitConvert.Parse(M.Person.BirthDate.ObjectType.Id, "1973-03-27T18:00:00Z"), koen.BirthDate);
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
            this.Workspace.Sync(Fixture.LoadData);

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
            this.Workspace.Sync(Fixture.LoadData);

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
            this.Workspace.Sync(Fixture.LoadData);
            var session = new Session(this.Workspace);

            var koen = session.Get(1) as Person;
            var patrick = session.Get(2) as Person;
            var martien = session.Get(3) as Person;

            koen.FirstName = "K";
            koen.LastName = "VE";
            martien.FirstName = "Martinus";
            martien.MiddleName = "X";

            var save = session.PushRequest();

            Assert.Equal(2, save.objects.Length);

            var savedKoen = save.objects.First(v => v.i == "1");

            Assert.Equal("1001", savedKoen.v);
            Assert.Equal(2, savedKoen.roles.Length);

            var savedKoenFirstName = savedKoen.roles.First(v => v.t == M.Person.FirstName.IdAsString);
            var savedKoenLastName = savedKoen.roles.First(v => v.t == M.Person.LastName.IdAsString);

            Assert.Equal("K", savedKoenFirstName.s);
            Assert.Null(savedKoenFirstName.a);
            Assert.Null(savedKoenFirstName.r);
            Assert.Equal("VE", savedKoenLastName.s);
            Assert.Null(savedKoenLastName.a);
            Assert.Null(savedKoenLastName.r);

            var savedMartien = save.objects.First(v => v.i == "3");

            Assert.Equal("1003", savedMartien.v);
            Assert.Equal(2, savedMartien.roles.Length);

            var savedMartienFirstName = savedMartien.roles.First(v => v.t == M.Person.FirstName.IdAsString);
            var savedMartienMiddleName = savedMartien.roles.First(v => v.t == M.Person.MiddleName.IdAsString);

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
            this.Workspace.Sync(Fixture.LoadData);
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
            this.Workspace.Sync(Fixture.LoadData);

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

            Assert.Equal(martien2, acme2.Owner); // x
            Assert.Null(ocme2.Owner);
            Assert.Equal(martien2, icme2.Owner);

            Assert.Equal(patrick2, acme2.Manager); // x
            Assert.Null(ocme2.Manager);
            Assert.Null(icme2.Manager);
        }

        [Fact]
        public void OneSave()
        {
            this.Workspace.Sync(Fixture.LoadData);
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

            Assert.Equal(2, save.objects.Length);

            var savedAcme = save.objects.First(v => v.i == "101");

            Assert.Equal("1101", savedAcme.v);
            Assert.Equal(2, savedAcme.roles.Length);

            var savedAcmeOwner = savedAcme.roles.First(v => v.t == M.Organisation.Owner.IdAsString);
            var savedAcmeManager = savedAcme.roles.First(v => v.t == M.Organisation.Manager.IdAsString);

            Assert.Equal("3", savedAcmeOwner.s);
            Assert.Null(savedAcmeOwner.a);
            Assert.Null(savedAcmeOwner.r);
            Assert.Equal("2", savedAcmeManager.s);
            Assert.Null(savedAcmeManager.a);
            Assert.Null(savedAcmeManager.r);

            var savedOcme = save.objects.First(v => v.i == "102");

            Assert.Equal("1102", savedOcme.v);
            Assert.Single(savedOcme.roles);

            var savedOcmeOwner = savedOcme.roles.First(v => v.t == M.Organisation.Owner.IdAsString);

            Assert.Null(savedOcmeOwner.s);
            Assert.Null(savedOcmeOwner.a);
            Assert.Null(savedOcmeOwner.r);
        }

        [Fact]
        public void ManyGet()
        {
            this.Workspace.Sync(Fixture.LoadData);
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
            this.Workspace.Sync(Fixture.LoadData);

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
            this.Workspace.Sync(Fixture.LoadData);

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

            Assert.Empty(save.newObjects);
            Assert.Equal(3, save.objects.Length);

            var savedAcme = save.objects.First(v => v.i == "101");

            Assert.Equal("1101", savedAcme.v);
            Assert.Single(savedAcme.roles);

            var savedAcmeEmployees = savedAcme.roles.First(v => v.t == M.Organisation.Employees.IdAsString);

            Assert.Null(savedAcmeEmployees.s);
            Assert.Empty(savedAcmeEmployees.a);
            Assert.Contains("1", savedAcmeEmployees.r);
            Assert.Contains("2", savedAcmeEmployees.r);
            Assert.Contains("3", savedAcmeEmployees.r);

            var savedOcme = save.objects.First(v => v.i == "102");

            Assert.Equal("1102", savedOcme.v);
            Assert.Single(savedOcme.roles);

            var savedOcmeEmployees = savedOcme.roles.First(v => v.t == M.Organisation.Employees.IdAsString);

            Assert.Null(savedOcmeEmployees.s);
            Assert.Equal(2, savedOcmeEmployees.a.Length);
            Assert.Contains("2", savedOcmeEmployees.a);
            Assert.Contains("3", savedOcmeEmployees.a);

            Assert.Single(savedOcmeEmployees.r);
            Assert.Contains("1", savedOcmeEmployees.r);

            var savedIcme = save.objects.First(v => v.i == "103");

            Assert.Equal("1103", savedIcme.v);
            Assert.Single(savedIcme.roles);

            var savedIcmeEmployees = savedIcme.roles.First(v => v.t == M.Organisation.Employees.IdAsString);

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
            this.Workspace.Sync(Fixture.LoadData);

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

            Assert.Equal(3, save.newObjects.Length);
            Assert.Empty(save.objects);
            {
                var savedMathijs = save.newObjects.First(v => v.ni == mathijs.NewId?.ToString());

                Assert.Equal(M.Person.Class.IdAsString, savedMathijs.t);
                Assert.Equal(2, savedMathijs.roles.Length);

                var savedMathijsFirstName = savedMathijs.roles.First(v => v.t == M.Person.FirstName.IdAsString);
                Assert.Equal("Mathijs", savedMathijsFirstName.s);

                var savedMathijsLastName = savedMathijs.roles.First(v => v.t == M.Person.LastName.IdAsString);
                Assert.Equal("Verwer", savedMathijsLastName.s);
            }

            {
                var savedAcme2 = save.newObjects.First(v => v.ni == acme2.NewId?.ToString());

                Assert.Equal(M.Organisation.Class.IdAsString, savedAcme2.t);
                Assert.Equal(3, savedAcme2.roles.Length);

                var savedAcme2Manager = savedAcme2.roles.First(v => v.t == M.Organisation.Manager.IdAsString);

                Assert.Equal(mathijs.NewId.ToString(), savedAcme2Manager.s);

                var savedAcme2Employees = savedAcme2.roles.First(v => v.t == M.Organisation.Employees.IdAsString);

                Assert.Null(savedAcme2Employees.s);
                Assert.Contains(mathijs.NewId?.ToString(), savedAcme2Employees.a);
                Assert.Null(savedAcme2Employees.r);
            }

            {
                var savedAcme3 = save.newObjects.First(v => v.ni == acme3.NewId?.ToString());

                Assert.Equal(M.Organisation.Class.IdAsString, savedAcme3.t);
                Assert.Equal(3, savedAcme3.roles.Length);

                var savedAcme3Manager = savedAcme3.roles.First(v => v.t == M.Organisation.Manager.IdAsString);

                Assert.Equal("3", savedAcme3Manager.s);

                var savedAcme3Employees = savedAcme3.roles.First(v => v.t == M.Organisation.Employees.IdAsString);

                Assert.Null(savedAcme3Employees.s);
                Assert.Contains("3", savedAcme3Employees.a);
                Assert.Null(savedAcme3Employees.r);
            }
        }

        [Fact]
        public void SyncWithNewObjects()
        {
            this.Workspace.Sync(Fixture.LoadData);

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

            // Assert.Null(mathijs.Id);
            Assert.True(mathijs.NewId < 0);
            Assert.Null(mathijs.FirstName);
            Assert.Null(mathijs.LastName);

            // Assert.Null(acme2.Id);
            Assert.True(acme2.NewId < 0);
            Assert.Null(acme2.Owner);
            Assert.Null(acme2.Manager);

            Assert.Empty(acme2.Employees);
        }

        [Fact]
        public void Onsaved()
        {
            this.Workspace.Sync(Fixture.LoadData);

            var session = new Session(this.Workspace);

            var pushResponse = new PushResponse();

            session.PushResponse(pushResponse);

            var mathijs = session.Create(M.Person.Class) as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            var newId = mathijs.NewId.Value;

            pushResponse = new PushResponse
            {
                newObjects = new[] { new PushResponseNewObject { i = "10000", ni = newId.ToString() } },
            };

            session.PushResponse(pushResponse);

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

        /*
        [Fact]
        public void methodCanExecute()
        {
            var workspace = new Workspace();
            workspace.Sync(Fixture.loadData);

            var session = new Session(workspace);

            var acme = session.Get("101") as Organisation;
            var ocme = session.Get("102") as Organisation;
            var icme = session.Get("102") as Organisation;

            Assert.True(acme.CanExecuteJustDoIt);
            this.isFalse(ocme.CanExecuteJustDoIt);
            this.isFalse(icme.CanExecuteJustDoIt);
        }
        */

        [Fact]
        public void Get()
        {
            this.Workspace.Sync(Fixture.LoadData);

            var session = new Session(this.Workspace);

            var acme = (Organisation)session.Create(M.Organisation.Class);

            var acmeAgain = session.Get(acme.Id);

            Assert.Equal(acme, acmeAgain);

            acmeAgain = session.Get(acme.NewId.Value);

            Assert.Equal(acme, acmeAgain);
        }
    }
}
