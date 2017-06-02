import * as chai from "chai";
import { Population } from "../src/allors/meta";

const expect = chai.expect;

describe("MetaDomain",
    () => {
        describe("default constructor",
        () => {

            let metaPopulation = new Population();

            it("should be newable",
                () => {
                    expect(metaPopulation).not.null;
                });

            describe("init with empty data population", () => {

                metaPopulation.init();
              
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
