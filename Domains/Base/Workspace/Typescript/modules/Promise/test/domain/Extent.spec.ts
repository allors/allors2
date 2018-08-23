import { domain, Person } from "../../src/allors/domain";
import { MetaPopulation, PullRequest, Workspace } from "../../src/allors/framework";
import { data } from "../../src/allors/meta";
import { Database, Scope } from "../../src/allors/promise";
import { PullExtent } from "../../src/allors/framework/database/pull/PullExtent";

import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";
import "mocha";
import { Extent } from "../../src/allors/framework/database/pull/Extent";

describe("Extent",
    () => {
        let metaPopulation: MetaPopulation;
        let scope: Scope;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);

            const http = new AxiosHttp("http://localhost:5000/");
            await http.login("TestAuthentication/Token", "administrator");
            const database = new Database(http);
            scope = new Scope(database, workspace);

        });

        describe("People",
            () => {
                it("should return all people", async () => {

                    var extent: PullExtent = new PullExtent({ extent: new Extent({ objectType: metaPopulation.objectTypeByName['Person'] }) });
                    var extents = [extent];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ extents }));

                    const people = loaded.collections["People"] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);
                });
            });

    });
