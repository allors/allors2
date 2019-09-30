import { Person, Media } from '../../src/allors/domain';
import { PullRequest, Pull, Filter, TreeNode, Tree, Result, Fetch, MetaPopulation } from '../../src/allors/framework';

import { assert } from 'chai';
import 'mocha';

import { Fixture } from '../Fixture';
import { TreeFactory } from '../../src/allors/meta';

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
  });
