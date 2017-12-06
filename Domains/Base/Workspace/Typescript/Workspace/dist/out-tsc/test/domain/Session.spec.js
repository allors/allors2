"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const chai_1 = require("chai");
const domain_1 = require("../../src/allors/domain");
const framework_1 = require("../../src/allors/framework");
const meta_1 = require("../../src/allors/meta");
const fixture_1 = require("./fixture");
describe("Session", () => {
    let metaPopulation;
    let workspace;
    beforeEach(() => {
        metaPopulation = new framework_1.MetaPopulation(meta_1.data);
        workspace = new framework_1.Workspace(metaPopulation, domain_1.constructorByName);
    });
    describe("sync", () => {
        let session;
        beforeEach(() => {
            workspace.sync(fixture_1.syncResponse);
            session = new framework_1.Session(workspace);
        });
        it("should get unit roles", () => {
            const koen = session.get("1");
            chai_1.assert.equal(koen.FirstName, "Koen");
            chai_1.assert.isNull(koen.MiddleName);
            chai_1.assert.equal(koen.LastName, "Van Exem");
            chai_1.assert.equal(koen.BirthDate.toUTCString(), new Date("1973-03-27T18:00:00Z").toUTCString());
            chai_1.assert.isTrue(koen.IsStudent);
            const patrick = session.get("2");
            chai_1.assert.equal(patrick.FirstName, "Patrick");
            chai_1.assert.isNull(patrick.MiddleName, null);
            chai_1.assert.equal(patrick.LastName, "De Boeck");
            chai_1.assert.isNull(patrick.BirthDate);
            chai_1.assert.isFalse(patrick.IsStudent);
            const martien = session.get("3");
            chai_1.assert.equal(martien.FirstName, "Martien");
            chai_1.assert.equal(martien.MiddleName, "van");
            chai_1.assert.equal(martien.LastName, "Knippenberg");
            chai_1.assert.isNull(martien.BirthDate);
            chai_1.assert.isNull(martien.IsStudent);
        });
        it("should get composite roles", () => {
            const koen = session.get("1");
            const patrick = session.get("2");
            const martien = session.get("3");
            const acme = session.get("101");
            chai_1.assert.equal(acme.Owner, koen);
            chai_1.assert.isNull(acme.Manager);
            const ocme = session.get("102");
            chai_1.assert.equal(ocme.Owner, patrick);
            chai_1.assert.isNull(ocme.Manager);
            const icme = session.get("103");
            chai_1.assert.equal(icme.Owner, martien);
            chai_1.assert.isNull(icme.Manager);
        });
        it("should get composites roles", () => {
            const koen = session.get("1");
            const patrick = session.get("2");
            const martien = session.get("3");
            const acme = session.get("101");
            const ocme = session.get("102");
            const icme = session.get("103");
            chai_1.assert.sameMembers(acme.Employees, [koen, patrick, martien]);
            chai_1.assert.sameMembers(ocme.Employees, [koen]);
            chai_1.assert.sameMembers(icme.Employees, []);
            chai_1.assert.sameMembers(acme.Shareholders, []);
            chai_1.assert.sameMembers(ocme.Shareholders, []);
            chai_1.assert.sameMembers(icme.Shareholders, []);
        });
        describe("two different sessions with same objects", () => {
            let session1;
            let session2;
            let koen1;
            let patrick1;
            let martien1;
            let acme1;
            let ocme1;
            let icme1;
            let koen2;
            let patrick2;
            let martien2;
            let acme2;
            let ocme2;
            let icme2;
            beforeEach(() => {
                session1 = new framework_1.Session(workspace);
                session2 = new framework_1.Session(workspace);
                koen1 = session1.get("1");
                patrick1 = session1.get("2");
                martien1 = session1.get("3");
                acme1 = session1.get("101");
                ocme1 = session1.get("102");
                icme1 = session1.get("103");
                koen2 = session2.get("1");
                patrick2 = session2.get("2");
                martien2 = session2.get("3");
                acme2 = session2.get("101");
                ocme2 = session2.get("102");
                icme2 = session2.get("103");
            });
            describe("update unit roles", () => {
                beforeEach(() => {
                    martien2.FirstName = "Martinus";
                    martien2.MiddleName = "X";
                });
                it("should see changes in this session", () => {
                    chai_1.assert.equal(martien1.FirstName, "Martien");
                    chai_1.assert.equal(martien1.LastName, "Knippenberg");
                    chai_1.assert.equal(martien1.MiddleName, "van");
                });
                it("should not see changes in the other session", () => {
                    chai_1.assert.equal(martien2.FirstName, "Martinus");
                    chai_1.assert.equal(martien2.LastName, "Knippenberg");
                    chai_1.assert.equal(martien2.MiddleName, "X");
                });
            });
            describe("update composite roles", () => {
                beforeEach(() => {
                    acme2.Owner = martien2;
                    ocme2.Owner = null;
                    acme2.Manager = patrick2;
                });
                it("should see changes in this session", () => {
                    chai_1.assert.equal(acme1.Owner, koen1);
                    chai_1.assert.equal(ocme1.Owner, patrick1);
                    chai_1.assert.equal(icme1.Owner, martien1);
                    chai_1.assert.isNull(acme1.Manager);
                    chai_1.assert.isNull(ocme1.Manager);
                    chai_1.assert.isNull(icme1.Manager);
                });
                it("should not see changes in the other session", () => {
                    chai_1.assert.equal(acme2.Owner, martien2);
                    chai_1.assert.isNull(ocme2.Owner);
                    chai_1.assert.equal(icme2.Owner, martien2);
                    chai_1.assert.equal(acme2.Manager, patrick2);
                    chai_1.assert.isNull(ocme2.Manager);
                    chai_1.assert.isNull(icme2.Manager);
                });
            });
            describe("update composites roles", () => {
                beforeEach(() => {
                    acme2.Employees = null;
                    icme2.Employees = [koen2, patrick2, martien2];
                });
                it("should see changes in this session", () => {
                    chai_1.assert.sameMembers(acme1.Employees, [koen1, patrick1, martien1]);
                    chai_1.assert.sameMembers(ocme1.Employees, [koen1]);
                    chai_1.assert.sameMembers(icme1.Employees, []);
                });
                it("should not see changes in the other session", () => {
                    chai_1.assert.sameMembers(acme2.Employees, []);
                    chai_1.assert.sameMembers(ocme2.Employees, [koen2]);
                    chai_1.assert.sameMembers(icme2.Employees, [koen2, patrick2, martien2]);
                });
            });
        });
        it("pushRequest should have all changes from session", () => {
            const koen = session.get("1");
            const patrick = session.get("2");
            const martien = session.get("3");
            const acme = session.get("101");
            const ocme = session.get("102");
            const icme = session.get("103");
            acme.Owner = martien;
            ocme.Owner = null;
            acme.Manager = patrick;
            const save = session.pushRequest();
            chai_1.assert.equal(save.objects.length, 2);
            const savedAcme = save.objects.find((v) => (v.i === "101"));
            chai_1.assert.equal(savedAcme.v, "1101");
            chai_1.assert.equal(savedAcme.roles.length, 2);
            const savedAcmeOwner = savedAcme.roles.find((v) => (v.t === "Owner"));
            const savedAcmeManager = savedAcme.roles.find((v) => (v.t === "Manager"));
            chai_1.assert.equal(savedAcmeOwner.s, "3");
            chai_1.assert.isUndefined(savedAcmeOwner.a);
            chai_1.assert.isUndefined(savedAcmeOwner.r);
            chai_1.assert.equal(savedAcmeManager.s, "2");
            chai_1.assert.isUndefined(savedAcmeManager.a);
            chai_1.assert.isUndefined(savedAcmeManager.r);
            const savedOcme = save.objects.find((v) => (v.i === "102"));
            chai_1.assert.equal(savedOcme.v, "1102");
            chai_1.assert.equal(savedOcme.roles.length, 1);
            const savedOcmeOwner = savedOcme.roles.find((v) => (v.t === "Owner"));
            chai_1.assert.isNull(savedOcmeOwner.s);
            chai_1.assert.isUndefined(savedOcmeOwner.a);
            chai_1.assert.isUndefined(savedOcmeOwner.r);
        });
        it("pushRequest should have all changes from session", () => {
            const koen = session.get("1");
            const patrick = session.get("2");
            const martien = session.get("3");
            const acme = session.get("101");
            const ocme = session.get("102");
            const icme = session.get("103");
            acme.Owner = martien;
            ocme.Owner = null;
            acme.Manager = patrick;
            const save = session.pushRequest();
            chai_1.assert.equal(save.objects.length, 2);
            const savedAcme = save.objects.find((v) => (v.i === "101"));
            chai_1.assert.equal(savedAcme.v, "1101");
            chai_1.assert.equal(savedAcme.roles.length, 2);
            const savedAcmeOwner = savedAcme.roles.find((v) => (v.t === "Owner"));
            const savedAcmeManager = savedAcme.roles.find((v) => (v.t === "Manager"));
            chai_1.assert.equal(savedAcmeOwner.s, "3");
            chai_1.assert.isUndefined(savedAcmeOwner.a);
            chai_1.assert.isUndefined(savedAcmeOwner.r);
            chai_1.assert.equal(savedAcmeManager.s, "2");
            chai_1.assert.isUndefined(savedAcmeManager.a);
            chai_1.assert.isUndefined(savedAcmeManager.r);
            const savedOcme = save.objects.find((v) => (v.i === "102"));
            chai_1.assert.equal(savedOcme.v, "1102");
            chai_1.assert.equal(savedOcme.roles.length, 1);
            const savedOcmeOwner = savedOcme.roles.find((v) => (v.t === "Owner"));
            chai_1.assert.isNull(savedOcmeOwner.s);
            chai_1.assert.isUndefined(savedOcmeOwner.a);
            chai_1.assert.isUndefined(savedOcmeOwner.r);
        });
        it("should save with many objects", () => {
            const martien = session.get("3");
            const mathijs = session.create("Person");
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";
            const acme2 = session.create("Organisation");
            acme2.Name = "Acme 2";
            acme2.Manager = mathijs;
            acme2.AddEmployee(mathijs);
            const acme3 = session.create("Organisation");
            acme3.Name = "Acme 3";
            acme3.Manager = martien;
            acme3.AddEmployee(martien);
            const save = session.pushRequest();
            chai_1.assert.equal(save.newObjects.length, 3);
            chai_1.assert.equal(save.objects.length, 0);
            {
                const savedMathijs = save.newObjects.find((v) => (v.ni === mathijs.newId));
                chai_1.assert.equal(savedMathijs.t, "Person");
                chai_1.assert.equal(savedMathijs.roles.length, 2);
                const savedMathijsFirstName = savedMathijs.roles.find((v) => (v.t === "FirstName"));
                chai_1.assert.equal(savedMathijsFirstName.s, "Mathijs");
                const savedMathijsLastName = savedMathijs.roles.find((v) => (v.t === "LastName"));
                chai_1.assert.equal(savedMathijsLastName.s, "Verwer");
            }
            {
                const savedAcme2 = save.newObjects.find((v) => (v.ni === acme2.newId));
                chai_1.assert.equal(savedAcme2.t, "Organisation");
                chai_1.assert.equal(savedAcme2.roles.length, 3);
                const savedAcme2Manager = savedAcme2.roles.find((v) => (v.t === "Manager"));
                chai_1.assert.equal(savedAcme2Manager.s, mathijs.newId);
                const savedAcme2Employees = savedAcme2.roles.find((v) => (v.t === "Employees"));
                chai_1.assert.isUndefined(savedAcme2Employees.s);
                chai_1.assert.sameMembers(savedAcme2Employees.a, [mathijs.newId]);
                chai_1.assert.isUndefined(savedAcme2Employees.r);
            }
            {
                const savedAcme3 = save.newObjects.find((v) => (v.ni === acme3.newId));
                chai_1.assert.equal(savedAcme3.t, "Organisation");
                chai_1.assert.equal(savedAcme3.roles.length, 3);
                const savedAcme3Manager = savedAcme3.roles.find((v) => (v.t === "Manager"));
                chai_1.assert.equal(savedAcme3Manager.s, "3");
                const savedAcme3Employees = savedAcme3.roles.find((v) => (v.t === "Employees"));
                chai_1.assert.isUndefined(savedAcme3Employees.s);
                chai_1.assert.sameMembers(savedAcme3Employees.a, ["3"]);
                chai_1.assert.isUndefined(savedAcme3Employees.r);
            }
        });
        it("should save with existing objects", () => {
            const koen = session.get("1");
            const patrick = session.get("2");
            const martien = session.get("3");
            const acme = session.get("101");
            const ocme = session.get("102");
            const icme = session.get("103");
            acme.Employees = null;
            ocme.Employees = [martien, patrick];
            icme.Employees = [koen, patrick, martien];
            const save = session.pushRequest();
            chai_1.assert.equal(save.newObjects.length, 0);
            chai_1.assert.equal(save.objects.length, 3);
            const savedAcme = save.objects.find((v) => (v.i === "101"));
            chai_1.assert.equal(savedAcme.v, "1101");
            chai_1.assert.equal(savedAcme.roles.length, 1);
            const savedAcmeEmployees = savedAcme.roles.find((v) => (v.t === "Employees"));
            chai_1.assert.isUndefined(savedAcmeEmployees.s);
            chai_1.assert.sameMembers(savedAcmeEmployees.a, []);
            chai_1.assert.sameMembers(savedAcmeEmployees.r, ["1", "2", "3"]);
            const savedOcme = save.objects.find((v) => (v.i === "102"));
            chai_1.assert.equal(savedOcme.v, "1102");
            chai_1.assert.equal(savedOcme.roles.length, 1);
            const savedOcmeEmployees = savedOcme.roles.find((v) => (v.t === "Employees"));
            chai_1.assert.isUndefined(savedOcmeEmployees.s);
            chai_1.assert.sameMembers(savedOcmeEmployees.a, ["2", "3"]);
            chai_1.assert.sameMembers(savedOcmeEmployees.r, ["1"]);
            const savedIcme = save.objects.find((v) => (v.i === "103"));
            chai_1.assert.equal(savedIcme.v, "1103");
            chai_1.assert.equal(savedIcme.roles.length, 1);
            const savedIcmeEmployees = savedIcme.roles.find((v) => (v.t === "Employees"));
            chai_1.assert.isUndefined(savedIcmeEmployees.s);
            chai_1.assert.sameMembers(savedIcmeEmployees.a, ["1", "2", "3"]);
            chai_1.assert.isUndefined(savedIcmeEmployees.r);
        });
        it("should save with new objects", () => {
            const martien = session.get("3");
            const mathijs = session.create("Person");
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";
            const acme2 = session.create("Organisation");
            acme2.Name = "Acme 2";
            acme2.Manager = mathijs;
            acme2.AddEmployee(mathijs);
            const acme3 = session.create("Organisation");
            acme3.Name = "Acme 3";
            acme3.Manager = martien;
            acme3.AddEmployee(martien);
            const save = session.pushRequest();
            chai_1.assert.equal(save.newObjects.length, 3);
            chai_1.assert.equal(save.objects.length, 0);
            {
                const savedMathijs = save.newObjects.find((v) => (v.ni === mathijs.newId));
                chai_1.assert.equal(savedMathijs.t, "Person");
                chai_1.assert.equal(savedMathijs.roles.length, 2);
                const savedMathijsFirstName = savedMathijs.roles.find((v) => (v.t === "FirstName"));
                chai_1.assert.equal(savedMathijsFirstName.s, "Mathijs");
                const savedMathijsLastName = savedMathijs.roles.find((v) => (v.t === "LastName"));
                chai_1.assert.equal(savedMathijsLastName.s, "Verwer");
            }
            {
                const savedAcme2 = save.newObjects.find((v) => (v.ni === acme2.newId));
                chai_1.assert.equal(savedAcme2.t, "Organisation");
                chai_1.assert.equal(savedAcme2.roles.length, 3);
                const savedAcme2Manager = savedAcme2.roles.find((v) => (v.t === "Manager"));
                chai_1.assert.equal(savedAcme2Manager.s, mathijs.newId);
                const savedAcme2Employees = savedAcme2.roles.find((v) => (v.t === "Employees"));
                chai_1.assert.isUndefined(savedAcme2Employees.s);
                chai_1.assert.sameMembers(savedAcme2Employees.a, [mathijs.newId]);
                chai_1.assert.isUndefined(savedAcme2Employees.r);
            }
            {
                const savedAcme3 = save.newObjects.find((v) => (v.ni === acme3.newId));
                chai_1.assert.equal(savedAcme3.t, "Organisation");
                chai_1.assert.equal(savedAcme3.roles.length, 3);
                const savedAcme3Manager = savedAcme3.roles.find((v) => (v.t === "Manager"));
                chai_1.assert.equal(savedAcme3Manager.s, "3");
                const savedAcme3Employees = savedAcme3.roles.find((v) => (v.t === "Employees"));
                chai_1.assert.isUndefined(savedAcme3Employees.s);
                chai_1.assert.sameMembers(savedAcme3Employees.a, ["3"]);
                chai_1.assert.isUndefined(savedAcme3Employees.r);
            }
        });
        it("should exist when created", () => {
            const john = session.create("Person");
            chai_1.assert.exists(john);
            chai_1.assert.isNull(john.FirstName);
            chai_1.assert.isTrue(john.CanReadFirstName);
            chai_1.assert.isTrue(john.CanWriteFirstName);
        });
        it("reset", () => {
            const martien = session.get("3");
            const mathijs = session.create("Person");
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";
            const acme2 = session.create("Organisation");
            acme2.Name = "Acme 2";
            acme2.Owner = martien;
            acme2.Manager = mathijs;
            acme2.AddEmployee(martien);
            acme2.AddEmployee(mathijs);
            session.reset();
            chai_1.assert.isFalse(mathijs.isNew);
            chai_1.assert.isUndefined(mathijs.id);
            chai_1.assert.isUndefined(mathijs.newId);
            chai_1.assert.isUndefined(mathijs.session);
            chai_1.assert.isUndefined(mathijs.objectType);
            chai_1.assert.isUndefined(mathijs.FirstName);
            chai_1.assert.isUndefined(mathijs.LastName);
            chai_1.assert.isUndefined(mathijs.CycleOne);
            chai_1.assert.isUndefined(mathijs.CycleMany);
            chai_1.assert.isFalse(acme2.isNew);
            chai_1.assert.isUndefined(acme2.id);
            chai_1.assert.isUndefined(acme2.newId);
            chai_1.assert.isUndefined(acme2.session);
            chai_1.assert.isUndefined(acme2.objectType);
            chai_1.assert.isUndefined(acme2.Name);
            chai_1.assert.isUndefined(acme2.Owner);
            chai_1.assert.isUndefined(acme2.Manager);
        });
        it("onsaved", () => {
            let saveResponse = {
                hasErrors: false,
                responseType: framework_1.ResponseType.Push,
            };
            session.pushResponse(saveResponse);
            let mathijs = session.create("Person");
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";
            const newId = mathijs.newId;
            saveResponse = {
                hasErrors: false,
                newObjects: [
                    {
                        i: "10000",
                        ni: newId,
                    },
                ],
                responseType: framework_1.ResponseType.Push,
            };
            session.pushResponse(saveResponse);
            chai_1.assert.isUndefined(mathijs.newId);
            chai_1.assert.equal(mathijs.id, "10000");
            chai_1.assert.equal(mathijs.objectType.name, "Person");
            mathijs = session.get("10000");
            chai_1.assert.exists(mathijs);
            let exceptionThrown = false;
            try {
                session.get(newId);
            }
            catch (e) {
                exceptionThrown = true;
            }
            chai_1.assert.isTrue(exceptionThrown);
        });
        it("methodCanExecute", () => {
            const acme = session.get("101");
            const ocme = session.get("102");
            const icme = session.get("103");
            chai_1.assert.isTrue(acme.CanExecuteJustDoIt);
            chai_1.assert.isFalse(ocme.CanExecuteJustDoIt);
            chai_1.assert.isFalse(icme.CanExecuteJustDoIt);
        });
        it("get", () => {
            const acme = session.create("Organisation");
            let acmeAgain = session.get(acme.id);
            chai_1.assert.equal(acmeAgain, acme);
            acmeAgain = session.get(acme.newId);
            chai_1.assert.equal(acmeAgain, acme);
        });
        it("hasChangesWithNewObject", () => {
            chai_1.assert.isFalse(session.hasChanges);
            const walter = session.create("Person");
            chai_1.assert.isTrue(session.hasChanges);
        });
        it("hasChangesWithChangedRelations", () => {
            const martien = session.get("3");
            chai_1.assert.isFalse(session.hasChanges);
            // Unit
            martien.FirstName = "New Name";
            chai_1.assert.isTrue(session.hasChanges);
            session.reset();
            const acme = session.get("101");
            chai_1.assert.include(acme.Employees, martien);
            // Composites
            acme.RemoveEmployee(martien);
            chai_1.assert.isTrue(session.hasChanges);
            chai_1.assert.notInclude(acme.Employees, martien);
        });
    });
});
//# sourceMappingURL=Session.spec.js.map