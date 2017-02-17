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

                it("should contain 14 types",
                    () => {
                        expect(Object.keys(metaPopulation.objectTypeByName).length).equal(14);
                    });

                it("should contain Binary, Boolean, DateTime, Decimal, Float, Integer, String, Unique (from Core)",
                    () => {
                        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"].forEach(name => {
                            let unit = metaPopulation.objectTypeByName[name];
                            expect(unit).not.null;
                        });
                    });

                it("should contain Media, ObjectState, Counter, Person, Role, UserGroup (from Base)",
                    () => {
                        ["Media", "ObjectState", "Counter", "Person", "Role", "UserGroup"].forEach(name => {
                            let unit = metaPopulation.objectTypeByName[name];
                            expect(unit).not.null;
                        });
                    });
            });
        });
});
