"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const chai_1 = require("chai");
require("mocha");
const framework_1 = require("@allors/framework");
const workspace_1 = require("@allors/workspace");
describe("MetaDomain", () => {
    describe("default constructor", () => {
        const metaPopulation = new framework_1.MetaPopulation(workspace_1.data);
        it("should be newable", () => {
            chai_1.assert.isNotNull(metaPopulation);
        });
        describe("init with empty data population", () => {
            it("should contain Binary, Boolean, DateTime, Decimal, Float, Integer, String, Unique (from Core)", () => {
                ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"].forEach((name) => {
                    const unit = metaPopulation.objectTypeByName[name];
                    chai_1.assert.isNotNull(unit);
                });
            });
            it("should contain Media, ObjectState, Counter, Person, Role, UserGroup (from Base)", () => {
                ["Media", "ObjectState", "Counter", "Person", "Role", "UserGroup"].forEach((name) => {
                    const unit = metaPopulation.objectTypeByName[name];
                    chai_1.assert.isNotNull(unit);
                });
            });
        });
    });
});
//# sourceMappingURL=MetaDomain.spec.js.map