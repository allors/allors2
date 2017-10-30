import { data } from "@generatedMeta/meta.g";
import * as chai from "chai";
import { Population } from "../src/allors/meta/base/index";

import { assert } from "chai";
import "mocha";

describe("MetaDomain",
    () => {
        describe("default constructor",
        () => {

            const metaPopulation = new Population();

            it("should be newable",
                () => {
                    assert.isNotNull(metaPopulation);
                });

            describe("init with empty data population", () => {

                metaPopulation.baseInit(data);

                it("should contain Binary, Boolean, DateTime, Decimal, Float, Integer, String, Unique (from Core)",
                    () => {
                        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"].forEach((name) => {
                            const unit = metaPopulation.objectTypeByName[name];
                            assert.isNotNull(unit);
                        });
                    });

                it("should contain Media, ObjectState, Counter, Person, Role, UserGroup (from Base)",
                    () => {
                        ["Media", "ObjectState", "Counter", "Person", "Role", "UserGroup"].forEach((name) => {
                            const unit = metaPopulation.objectTypeByName[name];
                            assert.isNotNull(unit);
                        });
                    });
            });
        });
});
