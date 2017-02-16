import * as chai from "chai";
import * as meta from "../src/meta";

const expect = chai.expect;

describe("MetaDomain",
    () => {
        describe("using default constructor",
        () => {

            let metaPopulation = new meta.Population();

            it("should exist",
                () => {
                    expect(metaPopulation).not.null;
                });

            describe("and init", () => {

                metaPopulation.init();

                it("should contain 8 types",
                    () => {
                        expect(Object.keys(metaPopulation.objectTypeByName).length).equal(8);
                    });

                it("should contain Binary, Boolean, DateTime, Decimal, Float, Integer, String, Unique",
                    () => {
                        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"].forEach(name => {
                            let unit = metaPopulation.objectTypeByName[name];
                            expect(unit).not.null;
                        });
                    });
            });
        });
});
