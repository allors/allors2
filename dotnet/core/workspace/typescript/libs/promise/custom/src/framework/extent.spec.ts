import { Pull, Result, Fetch, Tree, And, Equals, Extent, Node } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { Organisation, User, Media, C1, Person } from '@allors/domain/generated';

import { Fixture } from '../Fixture';

import 'jest-extended';

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

          const { m, ctx } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
                objectType: m.Person
              })
            }),
          ];

          ctx.session.reset();

          const pullRequest = new PullRequest({ pulls });
          const loaded = await ctx
            .load(pullRequest);

          const people = loaded.collections['People'] as Person[];

          expect(people).toBeArray();
          expect(people).not.toBeEmpty();
          expect(6).toBe( people.length);
        });
      });

    describe('People with include tree',
      () => {
        it('should return all people', async () => {

          const { m, ctx } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
                objectType: m.Person,
              }),
              results: [
                new Result({
                  fetch: new Fetch({
                    include: new Tree({
                      objectType: m.Person,
                      nodes: [
                        new Node({
                          propertyType: m.Person.Photo,
                        }),
                      ]
                    })
                  })
                })
              ]
            }),
          ];

          ctx.session.reset();

          const pullRequest = new PullRequest({ pulls });

          const json = JSON.stringify(pullRequest);

          const loaded = await ctx
            .load(pullRequest);

          const people = loaded.collections['People'] as Person[];

          expect(people).toBeArray();
          expect(people).not.toBeEmpty();

          people.forEach(() => {
          });
        });
      });

    describe('Organisation with tree builder',
      () => {
        it('should return all organisations', async () => {

          const { m, ctx, tree } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
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

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const organisations = loaded.collections['Organisations'] as Organisation[];

          expect(organisations).toBeArray();
          expect(organisations).not.toBeEmpty();

          organisations.forEach((organisation) => {
            const owner = organisation.Owner;
            if (owner) {
              // TODO:
            }
          });
        });
      });


      describe('User with tree (and Person)',
      () => {
        it('should return all users', async () => {

          const { m, ctx, tree } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
                objectType: m.User,
              }),
              results: [
                new Result({
                  fetch: new Fetch({
                    include: tree.User({
                      Person_Address: {},
                    })
                  })
                })
              ]
            }),
          ];

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const users = loaded.collections['Users'] as User[];

          expect(users).toBeArray();
          expect(users).not.toBeEmpty();

          const personWithAddress = users.find(v => (v as Person).Address) as Person;

          expect(personWithAddress).toBeDefined();
          expect('Jane').toBe( personWithAddress.FirstName)
        });
      });

    describe('Organisation with path',
      () => {
        it('should return all owners', async () => {

          const { m, ctx, fetch } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
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

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const owners = loaded.collections['Owners'] as Person[];

          expect(owners).toBeArray();
          expect(owners).not.toBeEmpty();
          expect(2).toBe( owners.length);
        });

        it('should return all employees', async () => {

          const { m, ctx, fetch } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
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

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const employees = loaded.collections['Employees'] as Media[];

          expect(employees).toBeArray();
          expect(employees).not.toBeEmpty();
          expect(3).toBe( employees.length);
        });
      });

    describe('Organisation with typesafe path',
      () => {
        it('should return all employees', async () => {

          const { m, ctx, fetch } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent(m.Organisation),
              results: [
                new Result({
                  fetch: fetch.Organisation({
                    Employees: {},
                  })
                })
              ]
            })
          ];

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const employees = loaded.collections['Employees'] as Person[];

          expect(employees).toBeArray();
          expect(employees).not.toBeEmpty();
          expect(3).toBe( employees.length);
        });
      });

    describe('Organisation with typesafe path and tree',
      () => {
        it('should return all people', async () => {

          const { m, ctx, fetch } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent(m.Organisation),
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

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const owners = loaded.collections['Owners'] as Person[];

          owners.forEach(v => v.Photo);

          expect(owners).toBeArray();
          expect(owners).not.toBeEmpty();
          expect(2).toBe( owners.length);
        });
      });

    describe('with predicate values',
      () => {
        it('should return results', async () => {

          const { m, ctx } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
                objectType: m.C1,
                predicate: new And({
                  operands: [
                    new Equals({
                      propertyType: m.C1.C1AllorsBoolean,
                      value: true,
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsDateTime,
                      value: new Date(),
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsDateTime,
                      value: new Date().toISOString(),
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsDecimal,
                      value: '10.0',
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsDecimal,
                      value: 10,
                    }),
                    new Equals({
                      propertyType: m.C1.I12AllorsDecimal,
                      value: 10.1,
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsInteger,
                      value: 1001,
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsInteger,
                      value: '1001',
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsUnique,
                      value: '{8E896822-C6DC-4D6D-BE30-77D24FDEA2CC}',
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsUnique,
                      value: '8E896822-C6DC-4D6D-BE30-77D24FDEA2CC',
                    }),
                  ]
                })
              })
            }),
          ];

          ctx.session.reset();

          const pullRequest = new PullRequest({ pulls });
          const loaded = await ctx
            .load(pullRequest);

          const c1s = loaded.collections['C1s'] as C1[];

          expect(c1s).toBeArray();
          // expect(c1s).not.toBeEmpty();
          // expect(7).toBe( c1s.length);
        });
      });

    describe('with predicate parameters',
      () => {
        it('should return results', async () => {

          const { m, ctx } = fixture;

          const pulls = [
            new Pull({
              extent: new Extent({
                objectType: m.C1,
                predicate: new And({
                  operands: [
                    new Equals({
                      propertyType: m.C1.C1AllorsBoolean,
                      parameter: 'p1',
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsDateTime,
                      parameter: 'p2',
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsDateTime,
                      parameter: 'p3',
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsDecimal,
                      parameter: 'p4',
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsDecimal,
                      parameter: 'p5',
                    }),
                    new Equals({
                      propertyType: m.C1.I12AllorsDecimal,
                      parameter: 'p6',
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsInteger,
                      parameter: 'p7',
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsInteger,
                      parameter: 'p8',
                    }),
                    new Equals({
                      propertyType: m.C1.C1AllorsUnique,
                      parameter: 'p9',
                    }),
                    new Equals({
                      propertyType: m.C1.I1AllorsUnique,
                      parameter: 'p10',
                    }),
                  ]
                })
              }),
              parameters: {
                p1: true,
                p2: new Date(),
                p3: new Date().toISOString(),
                p4: '10.0',
                p5: 10,
                p6: 10.1,
                p7: 1001,
                p8: '1001',
                p9: '{8E896822-C6DC-4D6D-BE30-77D24FDEA2CC}',
                p10: '8E896822-C6DC-4D6D-BE30-77D24FDEA2CC',
              }
            }),
          ];

          ctx.session.reset();

          const pullRequest = new PullRequest({ pulls });
          const loaded = await ctx
            .load(pullRequest);

          const c1s = loaded.collections['C1s'] as C1[];

          expect(c1s).toBeArray();
          // expect(c1s).not.toBeEmpty();
          // expect(7).toBe( c1s.length);
        });
      });
  });
