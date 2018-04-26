import { domain, Organisation, Person } from "../../src/allors/domain";
import { Fetch, MetaPopulation, PullRequest, Session, Workspace } from "../../src/allors/framework";
import { data, FetchFactory, FetchPerson, IncludeLanguage, IncludePerson } from "../../src/allors/meta";

import { assert } from "chai";
import "mocha";
import { QueryFactory } from "../../src/allors/meta/generated/query.g";

describe("Query",
    () => {
        let metaPopulation: MetaPopulation;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);
        });

        describe("with empty include",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new QueryFactory(metaPopulation);

                    const query = factory.Organisations({
                        include: {},
                    });

                    const json = JSON.stringify(query);
                    const include = JSON.parse(json).i;

                    assert.isArray(include);
                    assert.isEmpty(include);
                });
            });

        describe("with one role include",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new QueryFactory(metaPopulation);

                    const query = factory.Organisations({
                        include: {
                            Employees: {},
                        },
                    });

                    const json = JSON.stringify(query);
                    const include = JSON.parse(json).i;

                    assert.deepEqual(include, [
                        {
                            rt: "b95c7b34a295460082c8826cc2186a00",
                        },
                    ]);
                });
            });

        // See Fetch.Include for other variants
    });
