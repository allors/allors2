import { Pull, Result, Fetch, Tree, Extent, Node } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { Person } from '@allors/domain/generated';

import { Fixture } from '../Fixture';

import 'jest-extended';

describe('Instantiate',
  () => {
    let fixture: Fixture;

    let people: Person[] = [];

    beforeEach(async () => {
      fixture = new Fixture();
      await fixture.init();

      const { m, ctx } = fixture;

      const pulls = [
        new Pull({
          extent: new Extent({
            objectType: m.Person
          })
        }),
        new Pull({
          extent: new Extent({
            objectType: m.Organisation
          })
        }),
      ];

      ctx.session.reset();

      const loaded = await ctx
        .load(new PullRequest({ pulls }));

      people = loaded.collections['People'] as Person[];
    });

    describe('Person',
      () => {
        it('should return person', async () => {

          const { ctx } = fixture;

          const object = people[0].id;

          const pulls = [
            new Pull({
              object: object
            }),
          ];

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const person = loaded.objects['Person'] as Person;

          expect(person).not.toBeNull();
          expect(object).toBe( person.id);
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

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          people = loaded.collections['People'] as Person[];

          expect(people).toBeArray();
          expect(people).not.toBeEmpty();

          people.forEach(() => {
          });
        });
      });
  });
