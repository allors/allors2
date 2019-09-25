import { async } from '@angular/core/testing';

import { PullRequest, Pull, Filter, Result, Fetch, Tree, TreeNode } from '../../allors/framework';
import { Person, Organisation } from '../../allors/domain';
import { Fixture } from '../Fixture.spec';

import 'jasmine-expect';

let fixture: Fixture;

let people: Person[] = [];

describe('Instantiate', () => {
  beforeEach(async(() => {
    fixture = new Fixture();
  }));

  beforeEach(async () => {
    await fixture.init();

    const { pull } = fixture.meta;

    const pulls = [
      pull.Person()
    ];

    const loaded = await fixture.allors.context
      .load(new PullRequest({ pulls }))
      .toPromise();

    people = loaded.collections.People as Person[];
  });

  describe('Person',
    () => {
      it('should return person', async () => {

        const object = people[0].id;

        const pulls = [
          new Pull({
            object: object
          }),
        ];

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context
          .load(new PullRequest({ pulls }))
          .toPromise();

        const person = loaded.objects.Person as Person;

        expect(person.id).toBe(object);
      });
    });


  describe('People with include tree',
    () => {
      it('should return all people', async () => {

        const { m } = fixture.meta;

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

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context
          .load(new PullRequest({ pulls }))
          .toPromise();

        people = loaded.collections['People'] as Person[];

        expect(people).not.toBeEmptyArray();

        people.forEach(() => {
        });
      });
    });
});
