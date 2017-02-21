import { Session, Person, Organisation } from "../src/domain";
import { Workspace } from "../src/domain/base/Workspace";
import { ResponseType } from "../src/domain/base/data/responses/ResponseType";
import { syncResponse } from "./fixture";

import { Population as MetaPopulation } from "../src/meta";
import { constructorByName } from "../src/domain/generated/domain.g";

import * as chai from "chai";

const expect = chai.expect;

describe("Workspace",
    () => {
/*
        let workspace: Workspace;

        beforeEach(() => {
            let metaPopulation = new MetaPopulation();
            metaPopulation.init();

            workspace = new Workspace(metaPopulation, constructorByName);
        });


        describe("load",
            () => {
                workspace.sync(syncResponse);

                let martien = workspace.get("3");

                this.areIdentical("3", martien.id);
                this.areIdentical("1003", martien.version);
                this.areIdentical("Person", martien.objectType.name);
                this.areIdentical("Martien", martien.roles.FirstName);
                this.areIdentical("van", martien.roles.MiddleName);
                this.areIdentical("Knippenberg", martien.roles.LastName);
                this.areIdentical(undefined, martien.roles.IsStudent);
                this.areIdentical(undefined, martien.roles.BirthDate);
            }
        );

         describe("checkVersions",
            () => {
                workspace.userSecurityHash = "#";
                workspace.sync(syncResponse);

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

                this.areIdentical(1, requireLoad.objects.length);
            }
        );

         describe("checkVersionsUserSecurityHash",
            () => {
                workspace.userSecurityHash = "abc";
                workspace.sync(syncResponse);

                let pullResponse = {
                        responseType: ResponseType.Pull,
                        userSecurityHash: "def",
                        objects: [
                            ["1", "1001"],
                            ["2", "1002"],
                            ["3", "1004"]
                        ]
                    };

                let requireLoad = workspace.diff(pullResponse);

                this.areIdentical(3, requireLoad.objects.length);
            }
        );
*/});
