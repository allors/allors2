"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const chai_1 = require("chai");
require("mocha");
const framework_1 = require("@allors/framework");
const workspace_1 = require("@allors/workspace");
const fixture_1 = require("./fixture");
describe("Workspace", () => {
    let metaPopulation;
    let workspace;
    beforeEach(() => {
        metaPopulation = new framework_1.MetaPopulation(workspace_1.data);
        workspace = new framework_1.Workspace(metaPopulation, workspace_1.constructorByName);
    });
    it("should have its relations set when synced", () => {
        workspace.sync(fixture_1.syncResponse);
        const martien = workspace.get("3");
        chai_1.assert.equal(martien.id, "3");
        chai_1.assert.equal(martien.version, "1003");
        chai_1.assert.equal(martien.objectType.name, "Person");
        chai_1.assert.equal(martien.roles.FirstName, "Martien");
        chai_1.assert.equal(martien.roles.MiddleName, "van");
        chai_1.assert.equal(martien.roles.LastName, "Knippenberg");
        chai_1.assert.isUndefined(martien.roles.IsStudent);
        chai_1.assert.isUndefined(martien.roles.BirthDate);
    });
    describe("synced with same securityHash", () => {
        beforeEach(() => {
            workspace.sync(fixture_1.syncResponse);
        });
        it("should require load objects only for changed version", () => {
            const pullResponse = {
                hasErrors: false,
                objects: [
                    ["1", "1001"],
                    ["2", "1002"],
                    ["3", "1004"],
                ],
                responseType: framework_1.ResponseType.Pull,
                userSecurityHash: "#",
            };
            const requireLoad = workspace.diff(pullResponse);
            chai_1.assert.equal(requireLoad.objects.length, 1);
            chai_1.assert.equal(requireLoad.objects[0], "3");
        });
    });
    describe("synced with different securityHash", () => {
        beforeEach(() => {
            workspace.sync(fixture_1.syncResponse);
        });
        it("should require load objects for all objects", () => {
            const pullResponse = {
                hasErrors: false,
                objects: [
                    ["1", "1001"],
                    ["2", "1002"],
                    ["3", "1004"],
                ],
                responseType: framework_1.ResponseType.Pull,
                userSecurityHash: "abc",
            };
            const requireLoad = workspace.diff(pullResponse);
            chai_1.assert.equal(requireLoad.objects.length, 3);
            chai_1.assert.sameMembers(requireLoad.objects, ["1", "2", "3"]);
        });
    });
});
//# sourceMappingURL=Workspace.test.js.map