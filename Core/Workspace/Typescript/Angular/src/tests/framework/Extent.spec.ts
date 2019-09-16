import { async } from '@angular/core/testing';

import { PullRequest, Pull, Filter, Result, Fetch, Tree, TreeNode } from '../../allors/framework';
import { Loaded } from '../../allors/angular';
import { Person, Organisation } from '../../allors/domain';
import { Fixture } from '../Fixture.spec';

import 'jasmine-expect';

let fixture: Fixture;

describe('Extent', () => {
  beforeEach(async(() => {
    fixture = new Fixture();
  }));

  beforeEach(async () => {
    await fixture.init();
  });

  describe('People',
    () => {
      it('people should return all people', async () => {
        const { pull } = fixture.meta;

        const pulls = [
          pull.Person()
        ];

        const loaded: Loaded = await fixture.allors.context.load('Pull', new PullRequest({ pulls })).toPromise();
        const people = loaded.collections.People as Person[];

        expect(people).toBeArrayOfSize(7);
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
          .load('Pull', new PullRequest({ pulls })).toPromise();

        const people = loaded.collections['People'] as Person[];

        expect(people).not.toBeEmptyArray();

        people.forEach(() => {
        });
      });
    });

  describe('Organisation with tree builder',
    () => {
      it('should return all organisations', async () => {

        const { m, tree } = fixture.meta;

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

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context.load('Pull', new PullRequest({ pulls })).toPromise();

        const organisations = loaded.collections['Organisations'] as Organisation[];

        expect(organisations).not.toBeEmptyArray();

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

        const { m, fetch } = fixture.meta;

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

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context
          .load('Pull', new PullRequest({ pulls }))
          .toPromise();

        const owners = loaded.collections['Owners'] as Person[];

        expect(owners).toBeArrayOfSize(2);
      });

      it('should return all employees', async () => {

        const { m, fetch } = fixture.meta;

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

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context
          .load('Pull', new PullRequest({ pulls }))
          .toPromise();

        const employees = loaded.collections['Employees'] as Person[];

        expect(employees).toBeArrayOfSize(3);
      });
    });

  describe('Organisation with typesafe path',
    () => {
      it('should return all employees', async () => {

        const { m, fetch } = fixture.meta;

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

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context
          .load('Pull', new PullRequest({ pulls }))
          .toPromise();

        const employees = loaded.collections['Employees'] as Person[];

        expect(employees).toBeArrayOfSize(3);
      });
    });

  describe('Organisation with typesafe path and tree',
    () => {
      it('should return all people', async () => {

        const { m, fetch } = fixture.meta;

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

        fixture.allors.context.reset();

        const loaded = await fixture.allors.context
          .load('Pull', new PullRequest({ pulls }))
          .toPromise();

        const owners = loaded.collections['Owners'] as Person[];

        owners.forEach(v => v.Photo);

        expect(owners).toBeArrayOfSize(2);
      });
    });

});
