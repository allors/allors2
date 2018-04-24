import { domain, Organisation, Person } from "../../src/allors/domain";
import { Fetch, MetaPopulation, PullRequest, Session, Workspace } from "../../src/allors/framework";
import { data, FetchFactory, FetchPerson, IncludeLanguage, IncludePerson } from "../../src/allors/meta";
import { Database, Loaded, Scope } from "../../src/allors/promise";

import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";
import "mocha";

describe("Fetch",
    () => {
        let metaPopulation: MetaPopulation;
        let scope: Scope;
        let id: string;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);

            const http = new AxiosHttp("http://localhost:5000/");
            await http.login("TestAuthentication/Token", "administrator");
            const database = new Database(http);
            scope = new Scope(database, workspace);

        });

        describe("Person with include",
            () => {
                it("should return person with its employees", async () => {

                    let loaded: Loaded = await scope.load("People");
                    const people = loaded.collections.people as Person[];
                    id = people[0].id;

                    const fetch = new FetchFactory(metaPopulation);

                    const fetches = [
                        fetch.Person({
                            id,
                            include: {
                                CycleOne: {},
                                Photo: {
                                    MediaContent: {},
                                },
                            },
                        }),
                    ];

                    scope.session.reset();

                    loaded = await scope
                        .load("Pull", new PullRequest({ fetches }));

                    const person = loaded.objects["Person"] as Person;

                    assert.equal(id, person.id);

                    const cycleOne = person.CycleOne;
                });
            });

        describe("Organisation with path",
            () => {

                it("should return employees", async () => {

                    let loaded: Loaded = await scope.load("Organisations");
                    const organisations = loaded.collections.organisations as Organisation[];
                    const organisation = organisations.filter((v) => v.Employees)[0];
                    id = organisation.id;

                    const fetch = new FetchFactory(metaPopulation);

                    const fetches = [
                        fetch.Organisation({
                            id,
                            path: {
                                Employees: {},
                            },
                        }),
                    ];

                    loaded = await scope
                        .load("Pull", new PullRequest({ fetches }));

                    const employees = loaded.collections.Employees as Person[];

                    assert.isDefined(employees);
                });
            });
    });
