import { Person, Media, Organisation } from '../../src/allors/domain';
import { PullRequest, Pull, Filter, TreeNode, Tree, Result, Fetch } from '../../src/allors/framework';

import { assert } from 'chai';
import 'mocha';

import { Fixture } from '../Fixture';

describe('Extent',
    () => {
        let fixture: Fixture;

        beforeEach(async () => {
            fixture = new Fixture();
            await fixture.init();
        });

        describe('People',
            () => {
                it('should return all people', async () => {

                    const { m, scope } = fixture;

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Person
                            })
                        }),
                    ];

                    scope.session.reset();

                    const pullRequest = new PullRequest({ pulls });
                    const loaded = await scope
                        .load('Pull', pullRequest);

                    const people = loaded.collections['People'] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);
                    assert.equal(7, people.length);
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
                        .load('Pull', new PullRequest({ pulls }));

                    const people = loaded.collections['People'] as Person[];

                    assert.isArray(people);
                    assert.isNotEmpty(people);

                    people.forEach(() => {
                    });
                });
            });

        describe('Organisation with tree builder',
            () => {
                it('should return all organisations', async () => {

                    const { m, scope, tree } = fixture;

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Organisation,
                            }),
                            results: [
                                new Result({
                                    fetch: new Fetch({
                                        include: tree.Organisation({
                                            Owner: {}
                                        })
                                    })
                                })
                            ]
                        }),
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load('Pull', new PullRequest({ pulls }));

                    const organisations = loaded.collections['Organisations'] as Organisation[];

                    assert.isArray(organisations);
                    assert.isNotEmpty(organisations);

                    organisations.forEach((organisation) => {
                        const owner = organisation.Owner;
                        if (owner) {
                        }
                    });
                });
            });

        describe('Organisation with path',
            () => {
                it('should return all owners', async () => {

                    const { m, scope, fetch } = fixture;

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Organisation,
                            }),
                            results: [
                                new Result({
                                    fetch: fetch.Organisation({
                                        Owner: {},
                                    })
                                })
                            ]
                        })
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load('Pull', new PullRequest({ pulls }));

                    const owners = loaded.collections['Owners'] as Person[];

                    assert.isArray(owners);
                    assert.isNotEmpty(owners);
                    assert.equal(2, owners.length);
                });

                it('should return all employees', async () => {

                    const { m, scope, fetch } = fixture;

                    const pulls = [
                        new Pull({
                            extent: new Filter({
                                objectType: m.Organisation,
                            }),
                            results: [
                                new Result({
                                    fetch: fetch.Organisation({
                                        Employees: {},
                                    })
                                })
                            ]
                        })
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load('Pull', new PullRequest({ pulls }));

                    const employees = loaded.collections['Employees'] as Media[];

                    assert.isArray(employees);
                    assert.isNotEmpty(employees);
                    assert.equal(3, employees.length);
                });
            });

        describe('Organisation with typesafe path',
            () => {
                it('should return all employees', async () => {

                    const { m, scope, fetch } = fixture;

                    const pulls = [
                        new Pull({
                            extent: new Filter(m.Organisation),
                            results: [
                                new Result({
                                    fetch: fetch.Organisation({
                                        Employees: {},
                                    })
                                })
                            ]
                        })
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load('Pull', new PullRequest({ pulls }));

                    const employees = loaded.collections['Employees'] as Person[];

                    assert.isArray(employees);
                    assert.isNotEmpty(employees);
                    assert.equal(3, employees.length);
                });
            });

        describe('Organisation with typesafe path and tree',
            () => {
                it('should return all people', async () => {

                    const { m, scope, fetch } = fixture;

                    const pulls = [
                        new Pull({
                            extent: new Filter(m.Organisation),
                            results: [
                                new Result({
                                    fetch: fetch.Organisation({
                                        Owner: {
                                            include: {
                                                Photo: {}
                                            }
                                        },
                                    })
                                })
                            ]
                        })
                    ];

                    scope.session.reset();

                    const loaded = await scope
                        .load('Pull', new PullRequest({ pulls }));

                    const owners = loaded.collections['Owners'] as Person[];

                    owners.forEach(v => v.Photo);

                    assert.isArray(owners);
                    assert.isNotEmpty(owners);
                    assert.equal(2, owners.length);
                });
            });
    });
