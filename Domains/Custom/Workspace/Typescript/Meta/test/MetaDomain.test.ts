import * as chai from "chai";
import * as meta from "../src/meta";
import * as data from "../src/meta/base/Data";

const expect = chai.expect;

describe("MetaDomain",
    () => {
        describe("default constructor",
        () => {

            let metaPopulation = new meta.Population();

            it("should be newable",
                () => {
                    expect(metaPopulation).not.null;
                });

            describe("init with empty data population", () => {

                metaPopulation.init();

                it("should contain 31 types",
                    () => {
                        expect(Object.keys(metaPopulation.objectTypeByName).length).equal(31);
                    });

                it("should contain object types from Core",
                    () => {
                        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"].forEach(name => {
                            let unit = metaPopulation.objectTypeByName[name];
                            expect(unit).not.null;
                        });
                    });

                it("should contain object types from Base",
                    () => {
                        ["Media", "ObjectState", "Counter", "Person", "Role", "UserGroup"].forEach(name => {
                            let unit = metaPopulation.objectTypeByName[name];
                            expect(unit).not.null;
                        });
                    });

                it("should contain object types from Test",
                    () => {
                        [
                            "Deletable",
                            "Enumeration",
                            "UniquelyIdentifiable",
                            "User",
                            "I1",
                            "AccessControl",
                            "AsyncDerivation",
                            "Login",
                            "MediaContent",
                            "Permission",
                            "SecurityToken",
                            "C1",
                            "Dependent",
                            "Gender",
                            "OrderObjectState",
                            "Organisation",
                            "UnitSample"
                        ].forEach(name => {
                            let unit = metaPopulation.objectTypeByName[name];
                            expect(unit).not.null;
                        });
                    });
            });
        });
});
