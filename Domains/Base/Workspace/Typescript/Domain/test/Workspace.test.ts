import { MetaPopulation, Person, PullResponse, ResponseType, Session, Workspace } from "@allors";
import { constructorByName } from "@generatedDomain/domain.g";
import { data } from "@generatedMeta/meta.g";

import { syncResponse } from "./fixture";

import { assert } from "chai";
import "mocha";

describe("Workspace",
    () => {

        let metaPopulation: MetaPopulation;
        let workspace: Workspace;

        beforeEach(() => {
            metaPopulation = new MetaPopulation(data);
            workspace = new Workspace(metaPopulation, constructorByName);
        });

        it("should have its relations set when synced", () => {
            workspace.sync(syncResponse);

            const martien = workspace.get("3");

            assert.equal(martien.id, "3");
            assert.equal(martien.version, "1003");
            assert.equal(martien.objectType.name, "Person");
            assert.equal(martien.roles.FirstName, "Martien");
            assert.equal(martien.roles.MiddleName, "van");
            assert.equal(martien.roles.LastName, "Knippenberg");
            assert.isUndefined(martien.roles.IsStudent);
            assert.isUndefined(martien.roles.BirthDate);
        });

        describe("synced with same securityHash",
            () => {

                beforeEach(() => {
                    workspace.sync(syncResponse);
                });

                it("should require load objects only for changed version", () => {
                    const pullResponse: PullResponse = {
                        hasErrors: false,
                        objects: [
                            ["1", "1001"],
                            ["2", "1002"],
                            ["3", "1004"],
                        ],
                        responseType: ResponseType.Pull,
                        userSecurityHash: "#",
                    };

                    const requireLoad = workspace.diff(pullResponse);

                    assert.equal(requireLoad.objects.length, 1);
                    assert.equal(requireLoad.objects[0], "3");
                });

            },
        );

        describe("synced with different securityHash",
            () => {
                beforeEach(() => {
                    workspace.sync(syncResponse);
                });

                it("should require load objects for all objects", () => {
                    const pullResponse: PullResponse = {
                        hasErrors: false,
                        objects: [
                            ["1", "1001"],
                            ["2", "1002"],
                            ["3", "1004"],
                        ],
                        responseType: ResponseType.Pull,
                        userSecurityHash: "abc",
                    };

                    const requireLoad = workspace.diff(pullResponse);

                    assert.equal(requireLoad.objects.length, 3);
                    assert.sameMembers(requireLoad.objects, ["1", "2", "3"]);
                });
            });
    },
);
