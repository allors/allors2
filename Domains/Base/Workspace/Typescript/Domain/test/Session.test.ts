import { Session, Person, Organisation } from "../src/allors/domain";
import { Workspace } from "../src/allors/domain/base/Workspace";
import { PushResponse } from "../src/allors/domain/base/data/responses/PushResponse";
import { ResponseType } from "../src/allors/domain/base/data/responses/ResponseType";
import { syncResponse } from "./fixture";

import { Population as MetaPopulation } from "../src/allors/meta";
import { constructorByName } from "../src/allors/domain/generated/domain.g";

import * as chai from "chai";

import * as _ from "lodash";

const expect = chai.expect;

describe("Session",
    () => {
        let metaPopulation: MetaPopulation;
        let workspace: Workspace;

        beforeEach(() => {
            metaPopulation = new MetaPopulation();
            metaPopulation.init();
            workspace = new Workspace(metaPopulation, constructorByName);
        });

        describe("sync",
        () => {
            let session: Session;

            beforeEach(() => {
                workspace.sync(syncResponse);
                session = new Session(workspace);
            });

            it("should get unit roles", () => {
                let koen = session.get("1") as Person;

                expect(koen.FirstName).to.equal("Koen");
                expect(koen.MiddleName).to.equal(null);
                expect(koen.LastName).to.equal("Van Exem");
                expect(koen.BirthDate.toUTCString()).to.equal(new Date("1973-03-27T18:00:00Z").toUTCString());
                expect(koen.IsStudent).to.equal(true);

                let patrick = session.get("2") as Person;

                expect(patrick.FirstName).to.equal("Patrick");
                expect(patrick.MiddleName).to.equal(null);
                expect(patrick.LastName).to.equal("De Boeck");
                expect(patrick.BirthDate).to.equal(null);
                expect(patrick.IsStudent).to.equal(false);

                let martien = session.get("3") as Person;

                expect(martien.FirstName).to.equal("Martien");
                expect(martien.MiddleName).to.equal("van");
                expect(martien.LastName).to.equal("Knippenberg");
                expect(martien.BirthDate).to.equal(null);
                expect(martien.IsStudent).to.equal(null);
            });

            it("should get composite roles", () => {
                let koen = session.get("1") as Person;
                let patrick = session.get("2") as Person;
                let martien = session.get("3") as Person;

                let acme = session.get("101") as Organisation;

                expect(acme.Owner).to.equal(koen);
                expect(acme.Manager).to.equal(null);

                let ocme = session.get("102") as Organisation;

                expect(ocme.Owner).to.equal(patrick);
                expect(ocme.Manager).to.equal(null);

                let icme = session.get("103") as Organisation;

                expect(icme.Owner).to.equal(martien);
                expect(icme.Manager).to.equal(null);
            });

            it("should get composites roles", () => {
                let koen = session.get("1") as Person;
                let patrick = session.get("2") as Person;
                let martien = session.get("3") as Person;

                let acme = session.get("101") as Organisation;
                let ocme = session.get("102") as Organisation;
                let icme = session.get("103") as Organisation;

                expect(acme.Employees).to.have.members([koen, patrick, martien]);
                expect(ocme.Employees).to.have.members([koen]);
                expect(icme.Employees).to.have.members([]);

                expect(acme.Shareholders).to.have.members([]);
                expect(ocme.Shareholders).to.have.members([]);
                expect(icme.Shareholders).to.have.members([]);
            });

            describe("two different sessions with same objects",
            () => {
                let session1: Session;
                let session2: Session;

                let koen1: Person;
                let patrick1: Person;
                let martien1: Person;

                let acme1: Organisation;
                let ocme1:  Organisation;
                let icme1:  Organisation;

                let koen2: Person;
                let patrick2:  Person;
                let martien2:  Person;

                let acme2: Organisation;
                let ocme2: Organisation;
                let icme2: Organisation;

                beforeEach(() => {
                    session1 = new Session(workspace);
                    session2 = new Session(workspace);

                    koen1 = session1.get("1") as Person;
                    patrick1 = session1.get("2") as Person;
                    martien1 = session1.get("3") as Person;

                    acme1 = session1.get("101") as Organisation;
                    ocme1 = session1.get("102") as Organisation;
                    icme1 = session1.get("103") as Organisation;

                    koen2 = session2.get("1") as Person;
                    patrick2 = session2.get("2") as Person;
                    martien2 = session2.get("3") as Person;

                    acme2 = session2.get("101") as Organisation;
                    ocme2 = session2.get("102") as Organisation;
                    icme2 = session2.get("103") as Organisation;
                });

                describe("update unit roles", () => {

                    beforeEach(() => {
                        martien2.FirstName = "Martinus";
                        martien2.MiddleName = "X";
                    });

                    it("should see changes in this session", () => {
                        expect(martien1.FirstName).to.equal("Martien");
                        expect(martien1.LastName).to.equal("Knippenberg");
                        expect(martien1.MiddleName).to.equal("van");
                    });

                    it("should not see changes in the other session", () => {
                        expect(martien2.FirstName).to.equal("Martinus");
                        expect(martien2.LastName).to.equal("Knippenberg");
                        expect(martien2.MiddleName).to.equal("X");
                    });
                });

                describe("update composite roles", () => {

                    beforeEach(() => {
                        acme2.Owner = martien2;
                        ocme2.Owner = null;
                        acme2.Manager = patrick2;
                    });

                    it("should see changes in this session", () => {
                        expect(acme1.Owner).to.equal(koen1);
                        expect(ocme1.Owner).to.equal(patrick1);
                        expect(icme1.Owner).to.equal(martien1);

                        expect(acme1.Manager).to.equal(null);
                        expect(ocme1.Manager).to.equal(null);
                        expect(icme1.Manager).to.equal(null);
                    });

                    it("should not see changes in the other session", () => {
                        expect(acme2.Owner).to.equal(martien2);
                        expect(ocme2.Owner).to.equal(null);
                        expect(icme2.Owner).to.equal(martien2);

                        expect(acme2.Manager).to.equal(patrick2);
                        expect(ocme2.Manager).to.equal(null);
                        expect(icme2.Manager).to.equal(null);
                    });
                });

                describe("update composites roles", () => {

                    beforeEach(() => {
                        acme2.Employees = null;
                        icme2.Employees = [koen2, patrick2, martien2];
                    });

                    it("should see changes in this session", () => {
                        expect(acme1.Employees).to.have.members([koen1, patrick1, martien1]);
                        expect(ocme1.Employees).to.have.members([koen1]);
                        expect(icme1.Employees).to.have.members([]);
                    });

                    it("should not see changes in the other session", () => {
                        expect(acme2.Employees).to.have.members([]);
                        expect(ocme2.Employees).to.have.members([koen2]);
                        expect(icme2.Employees).to.have.members([koen2, patrick2, martien2]);
                    });
                });
            });

            it("pushRequest should have all changes from session",
            () => {
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

                expect(save.objects.length).to.equal(2);

                let savedAcme = _.find(save.objects, v => (v.i === "101"));

                expect(savedAcme.v).to.equal("1101");
                expect(savedAcme.roles.length).to.equal(2);

                let savedAcmeOwner = _.find(savedAcme.roles, v => (v.t === "Owner"));
                let savedAcmeManager = _.find(savedAcme.roles, v => (v.t === "Manager"));

                expect(savedAcmeOwner.s).to.equal("3");
                expect(savedAcmeOwner.a).to.equal(undefined);
                expect(savedAcmeOwner.r).to.equal(undefined);
                expect(savedAcmeManager.s).to.equal("2");
                expect(savedAcmeManager.a).to.equal(undefined);
                expect(savedAcmeManager.r).to.equal(undefined);

                let savedOcme = _.find(save.objects, v => (v.i === "102"));

                expect(savedOcme.v).to.equal("1102");
                expect(savedOcme.roles.length).to.equal(1);

                let savedOcmeOwner = _.find(savedOcme.roles, v => (v.t === "Owner"));

                expect(savedOcmeOwner.s).to.equal(null);
                expect(savedOcmeOwner.a).to.equal(undefined);
                expect(savedOcmeOwner.r).to.equal(undefined);
            });

            it("pushRequest should have all changes from session",
            () => {
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

                expect(save.objects.length).to.equal(2);

                let savedAcme = _.find(save.objects, v => (v.i === "101"));

                expect(savedAcme.v).to.equal("1101");
                expect(savedAcme.roles.length).to.equal(2);

                let savedAcmeOwner = _.find(savedAcme.roles, v => (v.t === "Owner"));
                let savedAcmeManager = _.find(savedAcme.roles, v => (v.t === "Manager"));

                expect(savedAcmeOwner.s).to.equal("3");
                expect(savedAcmeOwner.a).to.equal(undefined);
                expect(savedAcmeOwner.r).to.equal(undefined);
                expect(savedAcmeManager.s).to.equal("2");
                expect(savedAcmeManager.a).to.equal(undefined);
                expect(savedAcmeManager.r).to.equal(undefined);

                let savedOcme = _.find(save.objects, v => (v.i === "102"));

                expect(savedOcme.v).to.equal("1102");
                expect(savedOcme.roles.length).to.equal(1);

                let savedOcmeOwner = _.find(savedOcme.roles, v => (v.t === "Owner"));

                expect(savedOcmeOwner.s).to.equal(null);
                expect(savedOcmeOwner.a).to.equal(undefined);
                expect(savedOcmeOwner.r).to.equal(undefined);
            });

            it("should save with many objects", () => {
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
                expect(save.newObjects.length).to.equal(3);
                expect(save.objects.length).to.equal(0);

                {
                    let savedMathijs = _.find(save.newObjects, v => (v.ni === mathijs.newId));
                    expect(savedMathijs.t).to.equal("Person");
                    expect(savedMathijs.roles.length).to.equal(2);

                    let savedMathijsFirstName = _.find(savedMathijs.roles, v => (v.t === "FirstName"));
                    expect(savedMathijsFirstName.s).to.equal("Mathijs");

                    let savedMathijsLastName = _.find(savedMathijs.roles, v => (v.t === "LastName"));
                    expect(savedMathijsLastName.s).to.equal("Verwer");
                }

                {
                    let savedAcme2 = _.find(save.newObjects, v => (v.ni === acme2.newId));
                    expect(savedAcme2.t).to.equal("Organisation");
                    expect(savedAcme2.roles.length).to.equal(3);

                    let savedAcme2Manager = _.find(savedAcme2.roles, v => (v.t === "Manager"));
                    expect(savedAcme2Manager.s).to.equal(mathijs.newId);

                    let savedAcme2Employees = _.find(savedAcme2.roles, v => (v.t === "Employees"));
                    expect(savedAcme2Employees.s).to.equal(undefined);
                    expect(savedAcme2Employees.a).to.have.members([mathijs.newId]);
                    expect(savedAcme2Employees.r).to.equal(undefined);
                }

                {
                    let savedAcme3 = _.find(save.newObjects, v => (v.ni === acme3.newId));
                    expect(savedAcme3.t).to.equal("Organisation");
                    expect(savedAcme3.roles.length).to.equal(3);

                    let savedAcme3Manager = _.find(savedAcme3.roles, v => (v.t === "Manager"));
                    expect(savedAcme3Manager.s).to.equal("3");

                    let savedAcme3Employees = _.find(savedAcme3.roles, v => (v.t === "Employees"));
                    expect(savedAcme3Employees.s).to.equal(undefined);
                    expect(savedAcme3Employees.a).to.have.members(["3"]);
                    expect(savedAcme3Employees.r).to.equal(undefined);
                }
            });

            it("should save with existing objects", () => {
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

                expect(save.newObjects.length).to.equal(0);
                expect(save.objects.length).to.equal(3);

                let savedAcme = _.find(save.objects, v => (v.i === "101"));

                expect(savedAcme.v).to.equal("1101");
                expect(savedAcme.roles.length).to.equal(1);

                let savedAcmeEmployees = _.find(savedAcme.roles, v => (v.t === "Employees"));

                expect(savedAcmeEmployees.s).to.equal(undefined);
                expect(savedAcmeEmployees.a).to.have.members([]);
                expect(savedAcmeEmployees.r).to.have.members(["1", "2", "3"]);

                let savedOcme = _.find(save.objects, v => (v.i === "102"));

                expect(savedOcme.v).to.equal("1102");
                expect(savedOcme.roles.length).to.equal(1);

                let savedOcmeEmployees = _.find(savedOcme.roles, v => (v.t === "Employees"));

                expect(savedOcmeEmployees.s).to.equal(undefined);
                expect(savedOcmeEmployees.a).to.have.members(["2", "3"]);
                expect(savedOcmeEmployees.r).to.have.members(["1"]);

                let savedIcme = _.find(save.objects, v => (v.i === "103"));

                expect(savedIcme.v).to.equal("1103");
                expect(savedIcme.roles.length).to.equal(1);

                let savedIcmeEmployees = _.find(savedIcme.roles, v => (v.t === "Employees"));

                expect(savedIcmeEmployees.s).to.equal(undefined);
                expect(savedIcmeEmployees.a).to.have.members(["1", "2", "3"]);
                expect(savedIcmeEmployees.r).to.equal(undefined);
            });

            it("should save with new objects", () => {

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

                expect(save.newObjects.length).to.equal(3);
                expect(save.objects.length).to.equal(0);

                {
                    let savedMathijs = _.find(save.newObjects, v => (v.ni === mathijs.newId));
                    expect(savedMathijs.t).to.equal("Person");
                    expect(savedMathijs.roles.length).to.equal(2);

                    let savedMathijsFirstName = _.find(savedMathijs.roles, v => (v.t === "FirstName"));
                    expect(savedMathijsFirstName.s).to.equal("Mathijs");

                    let savedMathijsLastName = _.find(savedMathijs.roles, v => (v.t === "LastName"));
                    expect(savedMathijsLastName.s).to.equal("Verwer");
                }

                {
                    let savedAcme2 = _.find(save.newObjects, v => (v.ni === acme2.newId));
                    expect(savedAcme2.t).to.equal("Organisation");
                    expect(savedAcme2.roles.length).to.equal(3);

                    let savedAcme2Manager = _.find(savedAcme2.roles, v => (v.t === "Manager"));
                    expect(savedAcme2Manager.s).to.equal(mathijs.newId);

                    let savedAcme2Employees = _.find(savedAcme2.roles, v => (v.t === "Employees"));
                    expect(savedAcme2Employees.s).to.equal(undefined);
                    expect(savedAcme2Employees.a).to.have.members([mathijs.newId]);
                    expect(savedAcme2Employees.r).to.equal(undefined);
                }

                {
                    let savedAcme3 = _.find(save.newObjects, v => (v.ni === acme3.newId));
                    expect(savedAcme3.t).to.equal("Organisation");
                    expect(savedAcme3.roles.length).to.equal(3);

                    let savedAcme3Manager = _.find(savedAcme3.roles, v => (v.t === "Manager"));
                    expect(savedAcme3Manager.s).to.equal("3");

                    let savedAcme3Employees = _.find(savedAcme3.roles, v => (v.t === "Employees"));
                    expect(savedAcme3Employees.s).to.equal(undefined);
                    expect(savedAcme3Employees.a).to.have.members(["3"]);
                    expect(savedAcme3Employees.r).to.equal(undefined);
                }
            });

            it("should exist when created", () => {

                const john = session.create("Person") as Person;

                expect(john).to.exist;

                expect(john.FirstName).to.be.null;
                expect(john.CanReadFirstName).to.be.true;
                expect(john.CanWriteFirstName).to.be.true;
            });

            it("reset", () => {

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

                expect(mathijs.isNew).to.be.false;
                expect(mathijs.id).to.be.undefined;
                expect(mathijs.newId).to.be.undefined;
                expect(mathijs.session).to.be.undefined;
                expect(mathijs.objectType).to.be.undefined;
                expect(mathijs.FirstName).to.be.undefined;
                expect(mathijs.LastName).to.be.undefined;
                expect(mathijs.CycleOne).to.be.undefined;
                expect(mathijs.CycleMany).to.be.undefined;

                expect(acme2.isNew).to.be.false;
                expect(acme2.id).to.be.undefined;
                expect(acme2.newId).to.be.undefined;
                expect(acme2.session).to.be.undefined;
                expect(acme2.objectType).to.be.undefined;
                expect(acme2.Name).to.be.undefined;
                expect(acme2.Owner).to.be.undefined;
                expect(acme2.Manager).to.be.undefined;
            });

            it("onsaved", () => {
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

                expect(mathijs.newId).to.be.undefined;
                expect(mathijs.id).to.equal("10000");
                expect(mathijs.objectType.name).to.equal("Person");

                mathijs = session.get("10000") as Person;

                expect(mathijs).to.exist;

                let exceptionThrown = false;
                try {
                    session.get(newId);
                }
                catch (e) {
                    exceptionThrown = true;
                }

                expect(exceptionThrown).to.be.true;
            });

            it("methodCanExecute", () => {
                let acme = session.get("101") as Organisation;
                let ocme = session.get("102") as Organisation;
                let icme = session.get("102") as Organisation;

                expect(acme.CanExecuteJustDoIt).to.be.true;
                expect(ocme.CanExecuteJustDoIt).to.be.false;
                expect(icme.CanExecuteJustDoIt).to.be.false;
            });

            it("get", () => {
                let acme = session.create("Organisation") as Organisation;

                let acmeAgain = session.get(acme.id);
                expect(acmeAgain).to.equal(acme);

                acmeAgain = session.get(acme.newId);
                expect(acmeAgain).to.equal(acme);
            });

            it("hasChangesWithNewObject", () => {
                expect(session.hasChanges).to.be.false;

                const walter = session.create("Person") as Person;

                expect(session.hasChanges).to.be.true;
            });

            it("hasChangesWithChangedRelations", () => {
                const martien = session.get("3") as Person;

                expect(session.hasChanges).to.be.false;

                // Unit
                martien.FirstName = "New Name";

                expect(session.hasChanges).to.be.true;

                session.reset();

                const acme = session.get("101") as Organisation;

                expect(acme.Employees).to.contain(martien);

                // Composites
                acme.RemoveEmployee(martien);

                expect(session.hasChanges).to.be.true;

                expect(acme.Employees).not.to.contain(martien);
            });
        });
});
