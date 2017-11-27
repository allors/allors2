"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const chai_1 = require("chai");
require("mocha");
const framework_1 = require("@allors/framework");
const workspace_1 = require("@allors/workspace");
describe("Person", () => {
    let session;
    beforeEach(() => {
        const metaPopulation = new framework_1.MetaPopulation(workspace_1.data);
        const workspace = new framework_1.Workspace(metaPopulation, workspace_1.constructorByName);
        session = new framework_1.Session(workspace);
    });
    describe("displayName", () => {
        let person;
        beforeEach(() => {
            person = session.create("Person");
        });
        it("should be N/A when nothing set", () => {
            chai_1.assert.equal(person.displayName, "N/A");
        });
        it("should be john@doe.com when username is john@doe.com", () => {
            person.UserName = "john@doe.com";
            chai_1.assert.equal(person.displayName, "john@doe.com");
        });
        it("should be Doe when lastName is Doe", () => {
            person.LastName = "Doe";
            chai_1.assert.equal(person.displayName, "Doe");
        });
        it("should be John with firstName John", () => {
            person.FirstName = "John";
            chai_1.assert.equal(person.displayName, "John");
        });
        it("should be John Doe (even twice) with firstName John and lastName Doe", () => {
            person.FirstName = "John";
            person.LastName = "Doe";
            chai_1.assert.equal(person.displayName, "John Doe");
            chai_1.assert.equal(person.displayName, "John Doe");
        });
    });
});
//# sourceMappingURL=Person.spec.js.map