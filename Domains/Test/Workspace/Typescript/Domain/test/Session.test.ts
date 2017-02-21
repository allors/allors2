import { Session, Person, Organisation } from "../src/domain";
import { Workspace } from "../src/domain/base/Workspace";
import { PushResponse } from "../src/domain/base/data/responses/PushResponse";
import { ResponseType } from "../src/domain/base/data/responses/ResponseType";
import { syncResponse } from "./fixture";

import { Population as MetaPopulation } from "../src/meta";
import { constructorByName } from "../src/domain/generated/domain.g";

import * as chai from "chai";

import * as _ from "lodash";

const expect = chai.expect;

describe("Session",
    () => {
        let metaPopulation: MetaPopulation;
        let workspace: Workspace;

        metaPopulation = new MetaPopulation();
        metaPopulation.init();

        describe("sync",
        () => {
            workspace = new Workspace(metaPopulation, constructorByName);
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            describe("get", () =>{
                describe("koen", () =>{
                    let koen = session.get("1") as Person;

                    it("should have its properties synced", () => {
                        expect(koen.FirstName).to.equal("Koen");
                        expect(koen.MiddleName).to.equal(null);
                        expect(koen.LastName).to.equal("Van Exem");
                        expect(koen.BirthDate.toUTCString()).to.equal(new Date("1973-03-27T18:00:00Z").toUTCString());
                        expect(koen.IsStudent).to.equal(true);
                    });
                });

                 describe("patrick", () =>{
                    let patrick = session.get("2") as Person;

                    it("should have its properties synced", () => {
                        expect(patrick.FirstName).to.equal("Patrick");
                        expect(patrick.MiddleName).to.equal(null);
                        expect(patrick.LastName).to.equal("De Boeck");
                        expect(patrick.BirthDate).to.equal(null);
                        expect(patrick.IsStudent).to.equal(false);
                    });
                });

                 describe("martien", () => {
                    let patrick = session.get("3") as Person;

                    it("should have its properties synced", () => {
                        expect(patrick.FirstName).to.equal("Martien");
                        expect(patrick.MiddleName).to.equal("van");
                        expect(patrick.LastName).to.equal("Knippenberg");
                        expect(patrick.BirthDate).to.equal(null);
                        expect(patrick.IsStudent).to.equal(null);
                    });
                });
            });
        });

/*        describe("unitSet",
        () => {
            workspace.sync(syncResponse);

            let session1 = new Session(workspace);
            let martien1 = session1.get("3") as Person;

            let session2 = new Session(workspace);
            let martien2 = session2.get("3") as Person;

            martien2.FirstName = "Martinus";
            martien2.MiddleName = "X";

            this.areIdentical("Martien", martien1.FirstName);
            this.areIdentical("Knippenberg", martien1.LastName);
            this.areIdentical("van", martien1.MiddleName);

            this.areIdentical("Martinus", martien2.FirstName);
            this.areIdentical("Knippenberg", martien2.LastName);
            this.areIdentical("X", martien2.MiddleName);
        });

         describe("unitSave",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);
            let koen = session.get("1") as Person;
            let patrick = session.get("2") as Person;
            let martien = session.get("3") as Person;

            koen.FirstName = "K";
            koen.LastName = "VE";
            martien.FirstName = "Martinus";
            martien.MiddleName = "X";

            let save = session.pushRequest();

            this.areIdentical(2, save.objects.length);

            let savedKoen = _.find(save.objects, v => (v.i === "1"));

            this.areIdentical("1001", savedKoen.v);
            this.areIdentical(2, savedKoen.roles.length);

            let savedKoenFirstName = _.find(savedKoen.roles, v => (v.t === "FirstName"));
            let savedKoenLastName = _.find(savedKoen.roles, v => (v.t === "LastName"));

            this.areIdentical("K", savedKoenFirstName.s);
            this.areIdentical(undefined, savedKoenFirstName.a);
            this.areIdentical(undefined, savedKoenFirstName.r);
            this.areIdentical("VE", savedKoenLastName.s);
            this.areIdentical(undefined, savedKoenLastName.a);
            this.areIdentical(undefined, savedKoenLastName.r);

            let savedMartien = _.find(save.objects, v => (v.i === "3"));

            this.areIdentical("1003", savedMartien.v);
            this.areIdentical(2, savedMartien.roles.length);

            let savedMartienFirstName = _.find(savedMartien.roles, v => (v.t === "FirstName"));
            let savedMartienMiddleName = _.find(savedMartien.roles, v => (v.t === "MiddleName"));

            this.areIdentical("Martinus", savedMartienFirstName.s);
            this.areIdentical(undefined, savedMartienFirstName.a);
            this.areIdentical(undefined, savedMartienFirstName.r);
            this.areIdentical("X", savedMartienMiddleName.s);
            this.areIdentical(undefined, savedMartienMiddleName.a);
            this.areIdentical(undefined, savedMartienMiddleName.r);
        });

         describe("oneGet",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let koen = session.get("1") as Person;
            let patrick = session.get("2") as Person;
            let martien = session.get("3") as Person;

            let acme = session.get("101") as Organisation;
            let ocme = session.get("102") as Organisation;
            let icme = session.get("103") as Organisation;

            this.areIdentical(koen, acme.Owner);
            this.areIdentical(patrick, ocme.Owner);
            this.areIdentical(martien, icme.Owner);

            this.areIdentical(null, acme.Manager);
            this.areIdentical(null, ocme.Manager);
            this.areIdentical(null, icme.Manager);
        });
         describe("oneSet",
        () => {
            workspace.sync(syncResponse);

            let session1 = new Session(workspace);

            let session2 = new Session(workspace);

            let koen1 = session1.get("1") as Person;
            let patrick1 = session1.get("2") as Person;
            let martien1 = session1.get("3") as Person;

            let acme1 = session1.get("101") as Organisation;
            let ocme1 = session1.get("102") as Organisation;
            let icme1 = session1.get("103") as Organisation;

            let koen2 = session2.get("1") as Person;
            let patrick2 = session2.get("2") as Person;
            let martien2 = session2.get("3") as Person;

            let acme2 = session2.get("101") as Organisation;
            let ocme2 = session2.get("102") as Organisation;
            let icme2 = session2.get("103") as Organisation;

            acme2.Owner = martien2;
            ocme2.Owner = null;
            acme2.Manager = patrick2;

            this.areIdentical(koen1, acme1.Owner);
            this.areIdentical(patrick1, ocme1.Owner);
            this.areIdentical(martien1, icme1.Owner);

            this.areIdentical(null, acme1.Manager);
            this.areIdentical(null, ocme1.Manager);
            this.areIdentical(null, icme1.Manager);

            this.areIdentical(martien2, acme2.Owner); // x
            this.areIdentical(null, ocme2.Owner);
            this.areIdentical(martien2, icme2.Owner);

            this.areIdentical(patrick2, acme2.Manager); // x
            this.areIdentical(null, ocme2.Manager);
            this.areIdentical(null, icme2.Manager);
        });

         describe("oneSave",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let koen = session.get("1") as Person;
            let patrick = session.get("2") as Person;
            let martien = session.get("3") as Person;

            let acme = session.get("101") as Organisation;
            let ocme = session.get("102") as Organisation;
            let icme = session.get("103") as Organisation;

            acme.Owner = martien;
            ocme.Owner = null;

            acme.Manager = patrick;

            let save = session.pushRequest();

            this.areIdentical(2, save.objects.length);

            let savedAcme = _.find(save.objects, v => (v.i === "101"));

            this.areIdentical("1101", savedAcme.v);
            this.areIdentical(2, savedAcme.roles.length);

            let savedAcmeOwner = _.find(savedAcme.roles, v => (v.t === "Owner"));
            let savedAcmeManager = _.find(savedAcme.roles, v => (v.t === "Manager"));

            this.areIdentical("3", savedAcmeOwner.s);
            this.areIdentical(undefined, savedAcmeOwner.a);
            this.areIdentical(undefined, savedAcmeOwner.r);
            this.areIdentical("2", savedAcmeManager.s);
            this.areIdentical(undefined, savedAcmeManager.a);
            this.areIdentical(undefined, savedAcmeManager.r);

            let savedOcme = _.find(save.objects, v => (v.i === "102"));

            this.areIdentical("1102", savedOcme.v);
            this.areIdentical(1, savedOcme.roles.length);

            let savedOcmeOwner = _.find(savedOcme.roles, v => (v.t === "Owner"));

            this.areIdentical(null, savedOcmeOwner.s);
            this.areIdentical(undefined, savedOcmeOwner.a);
            this.areIdentical(undefined, savedOcmeOwner.r);
        });

         describe("manyGet",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let koen = session.get("1") as Person;
            let patrick = session.get("2") as Person;
            let martien = session.get("3") as Person;

            let acme = session.get("101") as Organisation;
            let ocme = session.get("102") as Organisation;
            let icme = session.get("103") as Organisation;

            this.isTrue(this.arrayEqual([koen, patrick, martien], acme.Employees));
            this.isTrue(this.arrayEqual([koen], ocme.Employees));
            this.isTrue(this.arrayEqual([], icme.Employees));

            this.isTrue(this.arrayEqual([], acme.Shareholders));
            this.isTrue(this.arrayEqual([], ocme.Shareholders));
            this.isTrue(this.arrayEqual([], icme.Shareholders));
        });

          describe("manySet",
        () => {
            workspace.sync(syncResponse);

            let session1 = new Session(workspace);

            let session2 = new Session(workspace);

            let koen1 = session1.get("1") as Person;
            let patrick1 = session1.get("2") as Person;
            let martien1 = session1.get("3") as Person;

            let acme1 = session1.get("101") as Organisation;
            let ocme1 = session1.get("102") as Organisation;
            let icme1 = session1.get("103") as Organisation;

            let koen2 = session2.get("1") as Person;
            let patrick2 = session2.get("2") as Person;
            let martien2 = session2.get("3") as Person;

            let acme2 = session2.get("101") as Organisation;
            let ocme2 = session2.get("102") as Organisation;
            let icme2 = session2.get("103") as Organisation;

            acme2.Employees = null;
            icme2.Employees = [koen2, patrick2, martien2];

            this.isTrue(this.arrayEqual([koen1, patrick1, martien1], acme1.Employees));
            this.isTrue(this.arrayEqual([koen1], ocme1.Employees));
            this.isTrue(this.arrayEqual([], icme1.Employees));

            this.isTrue(this.arrayEqual([], acme2.Employees));
            this.isTrue(this.arrayEqual([koen2], ocme2.Employees));
            this.isTrue(this.arrayEqual([koen2, patrick2, martien2], icme2.Employees));
        });

            describe("manySaveWithExistingObjects",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let koen = session.get("1") as Person;
            let patrick = session.get("2") as Person;
            let martien = session.get("3") as Person;

            let acme = session.get("101") as Organisation;
            let ocme = session.get("102") as Organisation;
            let icme = session.get("103") as Organisation;

            acme.Employees = null;
            ocme.Employees = [martien, patrick];
            icme.Employees = [koen, patrick, martien];

            let save = session.pushRequest();

            this.areIdentical(0, save.newObjects.length);
            this.areIdentical(3, save.objects.length);

            let savedAcme = _.find(save.objects, v => (v.i === "101"));

            this.areIdentical(savedAcme.v, "1101");
            this.areIdentical(savedAcme.roles.length, 1);

            let savedAcmeEmployees = _.find(savedAcme.roles, v => (v.t === "Employees"));

            this.areIdentical(undefined, savedAcmeEmployees.s);
            this.isTrue(this.arrayEqual([], savedAcmeEmployees.a));
            this.isTrue(this.arrayEqual(["1", "2", "3"], savedAcmeEmployees.r));

            let savedOcme = _.find(save.objects, v => (v.i === "102"));

            this.areIdentical("1102", savedOcme.v);
            this.areIdentical(1, savedOcme.roles.length);

            let savedOcmeEmployees = _.find(savedOcme.roles, v => (v.t === "Employees"));

            this.areIdentical(undefined, savedOcmeEmployees.s);
            this.isTrue(this.arrayEqual(["2", "3"], savedOcmeEmployees.a));
            this.isTrue(this.arrayEqual(["1"], savedOcmeEmployees.r));

            let savedIcme = _.find(save.objects, v => (v.i === "103"));

            this.areIdentical("1103", savedIcme.v);
            this.areIdentical(1, savedIcme.roles.length);

            let savedIcmeEmployees = _.find(savedIcme.roles, v => (v.t === "Employees"));

            this.areIdentical(undefined, savedIcmeEmployees.s);
            this.isTrue(this.arrayEqual(["1", "2", "3"], savedIcmeEmployees.a));
            this.areIdentical(undefined, savedIcmeEmployees.r);
        });


            describe("manySaveWithNewObjects",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let martien = session.get("3") as Person;

            let mathijs = session.create("Person") as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            let acme2 = session.create("Organisation") as Organisation;
            acme2.Name = "Acme 2";
            acme2.Manager = mathijs;
            acme2.AddEmployee(mathijs);

            let acme3 = session.create("Organisation") as Organisation;
            acme3.Name = "Acme 3";
            acme3.Manager = martien;
            acme3.AddEmployee(martien);

            let save = session.pushRequest();

            this.areIdentical(3, save.newObjects.length);
            this.areIdentical(0, save.objects.length);
            {
                let savedMathijs = _.find(save.newObjects, v => (v.ni === mathijs.newId));

                this.areIdentical("Person", savedMathijs.t);
                this.areIdentical(2, savedMathijs.roles.length);

                let savedMathijsFirstName = _.find(savedMathijs.roles, v => (v.t === "FirstName"));
                this.areIdentical("Mathijs", savedMathijsFirstName.s);

                let savedMathijsLastName = _.find(savedMathijs.roles, v => (v.t === "LastName"));
                this.areIdentical("Verwer", savedMathijsLastName.s);
            }

            {
                let savedAcme2 = _.find(save.newObjects, v => (v.ni === acme2.newId));

                this.areIdentical("Organisation", savedAcme2.t);
                this.areIdentical(3, savedAcme2.roles.length);

                let savedAcme2Manager = _.find(savedAcme2.roles, v => (v.t === "Manager"));

                this.areIdentical(mathijs.newId, savedAcme2Manager.s);

                let savedAcme2Employees = _.find(savedAcme2.roles, v => (v.t === "Employees"));

                this.areIdentical(undefined, savedAcme2Employees.s);
                this.isTrue(this.arrayEqual([mathijs.newId], savedAcme2Employees.a));
                this.areIdentical(undefined, savedAcme2Employees.r);
            }

            {
                let savedAcme3 = _.find(save.newObjects, v => (v.ni === acme3.newId));

                this.areIdentical("Organisation", savedAcme3.t);
                this.areIdentical(3, savedAcme3.roles.length);

                let savedAcme3Manager = _.find(savedAcme3.roles, v => (v.t === "Manager"));

                this.areIdentical("3", savedAcme3Manager.s);

                let savedAcme3Employees = _.find(savedAcme3.roles, v => (v.t === "Employees"));

                this.areIdentical(undefined, savedAcme3Employees.s);
                this.isTrue(this.arrayEqual(["3"], savedAcme3Employees.a));
                this.areIdentical(undefined, savedAcme3Employees.r);
            }        });

            describe("new",
        () => {
           const workspace = new Workspace(metaPopulation, constructorByName);
            const session = new Session(workspace);

            const john = session.create("Person") as Person;

            this.isFalsey(john.FirstName);

            this.isTrue(john.CanReadFirstName);
            this.isTrue(john.CanWriteFirstName);

            john.FirstName = "John"; });

            describe("reset",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let martien = session.get("3") as Person;

            let mathijs = session.create("Person") as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            let acme2 = session.create("Organisation") as Organisation;
            acme2.Name = "Acme 2";
            acme2.Owner = martien;
            acme2.Manager = mathijs;
            acme2.AddEmployee(martien);
            acme2.AddEmployee(mathijs);

            session.reset();

            this.isFalse(mathijs.isNew);
            this.isTrue(mathijs.id === undefined);
            this.isTrue(mathijs.newId === undefined);
            this.isTrue(mathijs.session === undefined);
            this.isTrue(mathijs.objectType === undefined);
            this.isTrue(mathijs.FirstName === undefined);
            this.isTrue(mathijs.LastName === undefined);
            this.isTrue(mathijs.CycleOne === undefined);
            this.isTrue(mathijs.CycleMany === undefined);

            this.isFalse(acme2.isNew);
            this.isTrue(acme2.id === undefined);
            this.isTrue(acme2.newId === undefined);
            this.isTrue(acme2.session === undefined);
            this.isTrue(acme2.objectType === undefined);
            this.isTrue(acme2.Name === undefined);
            this.isTrue(acme2.Owner === undefined);
            this.isTrue(acme2.Manager === undefined);
            this.isTrue(acme2.Manager === undefined);
            this.isTrue(acme2.Manager === undefined);        });

            describe("onsaved",
        () => {
            workspace.sync(syncResponse);

            const session = new Session(workspace);

            let saveResponse: PushResponse = {
                responseType: ResponseType.Push,
                hasErrors: false
            };

            session.pushResponse(saveResponse);

            let mathijs = session.create("Person") as Person;
            mathijs.FirstName = "Mathijs";
            mathijs.LastName = "Verwer";

            let newId = mathijs.newId;

            saveResponse = {
                responseType: ResponseType.Push,
                hasErrors: false,
                newObjects: [
                    {
                        i: "10000",
                        ni: newId
                    }
                ]
            };

            session.pushResponse(saveResponse);

            this.areIdentical(undefined, mathijs.newId);
            this.areIdentical("10000", mathijs.id);
            this.areIdentical("Person", mathijs.objectType.name);

            mathijs = session.get("10000") as Person;

            this.areNotIdentical(undefined, mathijs);

            let exceptionThrown = false;
            try {
                session.get(newId);
            }
            catch (e) {
                exceptionThrown = true;
            }

            this.isTrue(exceptionThrown);
        });


        describe("methodCanExecute",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let acme = session.get("101") as Organisation;
            let ocme = session.get("102") as Organisation;
            let icme = session.get("102") as Organisation;

            this.isTrue(acme.CanExecuteJustDoIt);
            this.isFalse(ocme.CanExecuteJustDoIt);
            this.isFalse(icme.CanExecuteJustDoIt);
        });

        describe("get",
        () => {
            workspace.sync(syncResponse);

            let session = new Session(workspace);

            let acme = session.create("Organisation") as Organisation;

            let acmeAgain = session.get(acme.id);

            this.areIdentical(acme, acmeAgain);

            acmeAgain = session.get(acme.newId);

            this.areIdentical(acme, acmeAgain); });

        describe("hasChangesWithNewObject",
        () => {
            workspace.sync(syncResponse);

            const session = new Session(workspace);

            this.isFalse(session.hasChanges);

            const walter = session.create("Person") as Person;

            this.isTrue(session.hasChanges);
        });

        describe("hasChangesWithChangedRelations",
        () => {
            workspace.sync(syncResponse);

            const session = new Session(workspace);

            const martien = session.get("3") as Person;

            this.isFalse(session.hasChanges);

            // Unit
            martien.FirstName = "New Name";

            this.isTrue(session.hasChanges);

            session.reset();

            const acme = session.get("101") as Organisation;

            this.isTrue(acme.Employees.indexOf(martien) > -1);

            // Composites
            acme.RemoveEmployee(martien);

            this.isTrue(session.hasChanges);

            this.isTrue(acme.Employees.indexOf(martien) === -1);
        });
*/
        function arrayEqual(array1, array2) {
            return _.isEmpty(_.difference(array1, array2)) && _.isEmpty(_.difference(array2, array1));
        }
});
