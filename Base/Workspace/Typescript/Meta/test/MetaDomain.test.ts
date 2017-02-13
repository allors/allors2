import * as chai from "chai";
import { Population as DataPopulation } from "../src/base/data/Population";
import { Population } from "../src/base/Population";

const expect = chai.expect;

describe("MetaDomain",
    () => {
        describe("constructor with minimal population",
        () => {

            let minimalDataPopulation: DataPopulation = { domains: ["Base"] };

            it("should be newable",
                () => {
                    let metaPopulation = new Population(minimalDataPopulation);

                    expect(metaPopulation).not.null;
                });

            it("should contain 8 types",
                () => {
                    let metaPopulation = new Population(minimalDataPopulation);

                    expect(metaPopulation.objectTypeByName.size).equals(8);
                });

            it("should contain Binary, Boolean, DateTime, Decimal, Float, Integer, String, Unique",
                () => {
                    let metaPopulation = new Population(minimalDataPopulation);

                    ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"].forEach(name => {
                        let unit = metaPopulation.objectTypeByName[name];
                        expect(unit).not.null;
                    });
                });
        });
});
