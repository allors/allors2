import { async } from '@angular/core/testing';

import { PullRequest, Pull, Filter, Result, Fetch, Tree, TreeNode } from '../../allors/framework';
import { Loaded } from '../../allors/angular';
import { Person, Organisation } from '../../allors/domain';

import { Fixture } from '../Fixture';

import 'jasmine-expect';
import { assertDataInRangeInternal } from '@angular/core/src/render3/util';

let fixture: Fixture;

let people: Person[] = [];

describe('Instantiate', () => {
  beforeEach(async(() => {
    fixture = new Fixture();
  }));

  beforeEach(async () => {
    await fixture.init();

    const { scope, pull } = fixture.allors;

    const pulls = [
      pull.Person()
    ];

    const loaded = await scope
      .load('Pull', new PullRequest({ pulls }))
      .toPromise();

    people = loaded.collections.People as Person[];
  });

  describe('Person',
    () => {
      it('should return person', async () => {

        const { m, scope } = fixture.allors;

        const object = people[0].id;

        const pulls = [
          new Pull({
            object: object
          }),
        ];

        scope.session.reset();

        const loaded = await scope
          .load('Pull', new PullRequest({ pulls }))
          .toPromise();

        const person = loaded.objects.Person as Person;

        expect(person.id).toBe(object);
      });
    });


  describe('People with include tree',
    () => {
      it('should return all people', async () => {

        const { m, scope } = fixture.allors;

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
          .load('Pull', new PullRequest({ pulls }))
          .toPromise();

        people = loaded.collections['People'] as Person[];

        expect(people).not.toBeEmptyArray();

        people.forEach(() => {
        });
      });
    });
});
