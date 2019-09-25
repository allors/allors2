import { Person } from '../../src/allors/domain';
import { PullRequest, Pull, Filter, TreeNode, Tree, Result, Fetch } from '../../src/allors/framework';

import { assert } from 'chai';
import 'mocha';

import { Fixture } from '../Fixture';

describe('Instantiate',
    () => {
        let fixture: Fixture;

        let people: Person[] = [];

        beforeEach(async () => {
            fixture = new Fixture();
            await fixture.init();

            const { m, scope } = fixture;

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
                .load(new PullRequest({ pulls }));

            people = loaded.collections['People'] as Person[];
        });

        describe('Person',
            () => {
                it('should return person', async () => {

                    const { m, scope } = fixture;

                    const object = people[0].id;

                    const pulls = [
                        new Pull({
                            object: object
                        }),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load(new PullRequest({ pulls }));

                    const person = loaded.objects['Person'] as Person;

                    assert.isNotNull(person);
                    assert.equal(object, person.id);
                });
            });


        describe('People with include tree',
            () => {
                it('should return all people', async () => {

                    const { m, scope } = fixture;

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
                                                    propertyType: m.Person.Photo,
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
                        .load(new PullRequest({ pulls }));

                    people = loaded.collections['People'] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);

                    people.forEach(() => {
                    });
                });
            });
/*
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
                        .load(new PullRequest({ pulls }));

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
                        .load(new PullRequest({ pulls }));

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
                        .load(new PullRequest({ pulls }));

                    const employees = loaded.collections["Employees"] as Media[];

                    assert.isArray(employees);
                    assert.isNotEmpty(employees);
                    assert.equal(3, employees.length);
                });
            });
 */
    });
