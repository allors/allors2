

import { domain } from "../../src/allors/domain";
import { MetaPopulation, Workspace } from "../../src/allors/framework";
import { data, PullFactory } from "../../src/allors/meta";

import { assert } from "chai";
import "mocha";

describe("Pull",
    () => {
        let metaPopulation: MetaPopulation;
        let pull: PullFactory;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);

            pull = new PullFactory(metaPopulation);
        });

        describe("with empty flatPull",
            () => {
                it("should serialize to correct json", () => {

                    const original = pull.Organisation({
                        
                    });

                    const json = JSON.stringify(original);
                    const path = JSON.parse(json);

                    assert.isDefined(path);
                });
            });
    });
