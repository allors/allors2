import { Session, Person, Organisation } from "../src/allors/domain";
import { Workspace } from "../src/allors/domain/base/Workspace";
import { ResponseType } from "../src/allors/domain/base/data/responses/ResponseType";
import { syncResponse } from "./fixture";

import { Population as MetaPopulation } from "../src/allors/meta";
import { constructorByName } from "../src/allors/domain/generated/domain.g";

import * as chai from "chai";

const expect = chai.expect;

describe("Workspace",
    () => {

        let metaPopulation: MetaPopulation;
        let workspace: Workspace;

        beforeEach(() => {
            metaPopulation = new MetaPopulation();
            metaPopulation.init();
            workspace = new Workspace(metaPopulation, constructorByName);
        });


        it("should have its relations set when synced", () => {
            workspace.sync(syncResponse);

            let martien = workspace.get("3");

            expect(martien.id).to.equal("3");
            expect(martien.version).to.equal("1003");
            expect(martien.objectType.name).to.equal("Person");
            expect(martien.roles.FirstName).to.equal("Martien");
            expect(martien.roles.MiddleName).to.equal("van");
            expect(martien.roles.LastName).to.equal("Knippenberg");
            expect(martien.roles.IsStudent).to.equal(undefined);
            expect(martien.roles.BirthDate).to.equal(undefined);
        });

         describe("synced with same securityHash",
            () => {

                beforeEach(() => {
                    workspace.sync(syncResponse);
                });

                it("should require load objects only for changed version", () => {
                    let pullResponse = {
                            responseType: ResponseType.Pull,
                            userSecurityHash: "#",
                            objects: [
                                ["1", "1001"],
                                ["2", "1002"],
                                ["3", "1004"]
                            ]
                        };

                    let requireLoad = workspace.diff(pullResponse);

                    expect(requireLoad.objects.length).to.equal(1);
                    expect(requireLoad.objects[0]).to.equal("3");
                });

            }
        );

        describe("synced with different securityHash",
            () => {
                beforeEach(() => {
                    workspace.sync(syncResponse);
                });

                it("should require load objects for all objects", () => {
                    let pullResponse = {
                            responseType: ResponseType.Pull,
                            userSecurityHash: "abc",
                            objects: [
                                ["1", "1001"],
                                ["2", "1002"],
                                ["3", "1004"]
                            ]
                        };

                    let requireLoad = workspace.diff(pullResponse);

                    expect(requireLoad.objects.length).to.equal(3);
                    expect(requireLoad.objects).to.contain("1");
                    expect(requireLoad.objects).to.contain("2");
                    expect(requireLoad.objects).to.contain("3");
                });
            });
        }
);
