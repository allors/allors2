import { domain, Organisation, Person } from "../../src/allors/domain";
import { Fetch, MetaPopulation, PullRequest, Session, Workspace } from "../../src/allors/framework";
import { data } from "../../src/allors/meta";
import { Database, Loaded, Scope } from "../../src/allors/promise";

import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";
import "mocha";
import { QueryFactory } from "../../src/allors/meta/generated/query.g";

describe("Query",
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

                    const query = new QueryFactory(metaPopulation);

                    const queries = [
                        query.People(),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ queries }));

                    const people = loaded.collections[query.People.name] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);
                });
            });

        describe("People with include",
            () => {
                it("should return all people", async () => {

                    const query = new QueryFactory(metaPopulation);

                    const queries = [
                        query.People({
                            include: {
                                Photo: {},
                            },
                        }),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ queries }));

                    const people = loaded.collections[query.People.name] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);

                    people.forEach((person) => {
                        const photo = person.Photo;
                    });
                });
            });
    });
