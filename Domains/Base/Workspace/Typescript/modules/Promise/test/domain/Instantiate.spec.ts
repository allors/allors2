import { domain, Person, Media, Organisation } from "../../src/allors/domain";
import { MetaPopulation, PullRequest, Workspace, Pull, Filter, Fetch, TreeNode, Tree } from "../../src/allors/framework";
import { data, MetaDomain, TreeFactory, PathPerson, PathOrganisation } from "../../src/allors/meta";

import { Database, Scope } from "../../src/allors/promise";
import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";
import "mocha";

describe("Instantiate",
    () => {
        let metaPopulation: MetaPopulation;
        let m: MetaDomain;
        let scope: Scope;

        let people: Person[] = [];
        let organisation: Organisation[] = [];

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            m = metaPopulation.metaDomain;
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);

            const http = new AxiosHttp("http://localhost:5000/");
            await http.login("TestAuthentication/Token", "administrator");
            const database = new Database(http);
            scope = new Scope(database, workspace);
                
            const pulls = [
                new Pull({
                    extent: new Filter({
                        objectType: m.Person
                    })
                }),
                new Pull({
                    extent: new Filter({
                        objectType: m.Organisation
                    })
                }),
            ];

            scope.session.reset();

            const loaded = await scope
                .load("Pull", new PullRequest({ pulls }));

            people = loaded.collections["People"] as Person[];
            organisation = loaded.collections["Organisations"] as Organisation[];

            scope = new Scope(database, workspace);
        });

        describe("Person",
            () => {
                it("should return person", async () => {

                    const object = people[0].id;

                    const pulls = [
                        new Pull({
                            object: object
                        }),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load("Pull", new PullRequest({ pulls }));

                    const person = loaded.objects["Person"] as Person;

                    assert.isNotNull(person);
                    assert.equal(object, person.id);
                });
            });
            
/*
         describe("People with include tree",
            () => {
                it("should return all people", async () => {

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Person,
                            }),
                            fetches: [
                                new Fetch({
                                    include: new Tree({
                                        objectType: m.Person,
                                        nodes: [
                                            new TreeNode({
                                                roleType: m.Person.Photo,
                                            }),
                                        ]
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
                            fetches: [
                                new Fetch({
                                    include: tree.Person({
                                        nodes: {
                                            Photo: {}
                                        }
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
                            fetches: [
                                new Fetch({
                                    path: {
                                        Owner: {},
                                    }
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
                            fetches: [
                                new Fetch({
                                    path: {
                                        Employees: {},
                                    }
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
 */    
});
