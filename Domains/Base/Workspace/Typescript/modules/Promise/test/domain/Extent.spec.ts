import { domain, Person, Media } from "../../src/allors/domain";
import { MetaPopulation, PullRequest, Workspace, Pull, Filter, Fetch, TreeNode, Tree, Result } from "../../src/allors/framework";
import { data, MetaDomain, TreeFactory, PathPerson, PathOrganisation } from "../../src/allors/meta";

import { Database, Scope } from "../../src/allors/promise";
import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";
import "mocha";

describe("Extent",
    () => {
        let metaPopulation: MetaPopulation;
        let m: MetaDomain;
        let scope: Scope;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            m = metaPopulation.metaDomain;
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

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Person
                            })
                        }),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ pulls }));

                    const people = loaded.collections["People"] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);
                    assert.equal(7, people.length);
                });
            });

        describe("People with include tree",
            () => {
                it("should return all people", async () => {

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Person,
                            }),
                            results: [
                                new Result({
                                    fetch: new Fetch({
                                        include: new Tree({
                                            objectType: m.Person,
                                            nodes: [
                                                new TreeNode({
                                                    roleType: m.Person.Photo,
                                                }),
                                            ]
                                        })
                                    })
                                })
                            ]
                        }),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ pulls }));

                    const people = loaded.collections["People"] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);

                    people.forEach((person) => {
                    });
                });
            });

        describe("People with include tree (factory)",
            () => {
                it("should return all people", async () => {

                    const tree = new TreeFactory(metaPopulation);

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Person,
                            }),
                            results: [
                                new Result({
                                    fetch: new Fetch({
                                        include: tree.Person({
                                            nodes: {
                                                Photo: {}
                                            }
                                        })
                                    })
                                })
                            ]
                        }),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ pulls }));

                    const people = loaded.collections["People"] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);

                    people.forEach((person) => {
                    });
                });
            });

        describe("Organisation with path",
            () => {
                it("should return all owners", async () => {

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Organisation,
                            }),
                            results: [
                                new Result({
                                    fetch: new Fetch({
                                        path: {
                                            Owner: {},
                                        }
                                    })
                                })
                            ]
                        })
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ pulls }));

                    const owners = loaded.collections["Owners"] as Media[];

                    assert.isArray(owners);
                    assert.isNotEmpty(owners);
                    assert.equal(2, owners.length);
                });
            });

        describe("Organisation with path",
            () => {
                it("should return all employees", async () => {

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Organisation,
                            }),
                            results: [
                                new Result({
                                    fetch: new Fetch({
                                        path: {
                                            Employees: {},
                                        }
                                    })
                                })
                            ]
                        })
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ pulls }));

                    const employees = loaded.collections["Employees"] as Media[];

                    assert.isArray(employees);
                    assert.isNotEmpty(employees);
                    assert.equal(3, employees.length);
                });
            });
    });
