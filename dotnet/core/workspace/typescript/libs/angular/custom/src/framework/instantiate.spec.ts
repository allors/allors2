import { waitForAsync } from '@angular/core/testing';

import { PullRequest } from '@allors/protocol/system';
import { Pull, Extent, Result, Fetch, Tree, Node } from '@allors/data/system';
import { Person } from '@allors/domain/generated';

import { Fixture } from '../Fixture';

import 'jest-extended';

let fixture: Fixture;

let people: Person[] = [];

describe('Instantiate', () => {
  beforeEach(waitForAsync(() => {
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
            object
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

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context
          .load(new PullRequest({ pulls }))
          .toPromise();

        people = loaded.collections.People as Person[];

        expect(people.length).not.toBe(0);

        people.forEach(() => {
        });
      });
    });
});
