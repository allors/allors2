import { waitForAsync } from '@angular/core/testing';

import { PullRequest } from '@allors/protocol/system';
import { Pull, Extent, Result, Fetch, Tree, Node } from '@allors/data/system';
import { Organisation, Person } from '@allors/domain/generated';

import { Fixture } from '../Fixture';

import 'jest-extended';
import { Loaded } from '@allors/angular/services/core';

let fixture: Fixture;

describe('Extent', () => {
  beforeEach(waitForAsync(() => {
    fixture = new Fixture();
  }));

  beforeEach(async () => {
    await fixture.init();
  });

  describe('People', () => {
    it('people should return all people', async () => {
      const { pull } = fixture.meta;

      const pulls = [pull.Person()];

      const loaded: Loaded = await fixture.allors.context.load(new PullRequest({ pulls })).toPromise();
      const people = loaded.collections.People as Person[];

      expect(people.length).toBe(6);
    });
  });

  describe('People with include tree', () => {
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
                  ],
                }),
              }),
            }),
          ],
        }),
      ];

      fixture.allors.context.reset();

      const loaded = await fixture.allors.context.load(new PullRequest({ pulls })).toPromise();

      const people = loaded.collections.People as Person[];

      expect(people.length).toBe(6);

      people.forEach((person) => {
        const photo = person.Photo;
      });
    });
  });

  describe('Organisation with tree builder', () => {
    it('should return all organisations', async () => {
      const { m, tree } = fixture.meta;

      const pulls = [
        new Pull({
          extent: new Extent({
            objectType: m.Organisation,
          }),
          results: [
            new Result({
              fetch: new Fetch({
                include: tree.Organisation({
                  Owner: {},
                }),
              }),
            }),
          ],
        }),
      ];

      fixture.allors.context.reset();

      const loaded = await fixture.allors.context.load(new PullRequest({ pulls })).toPromise();

      const organisations = loaded.collections.Organisations as Organisation[];

      expect(organisations.length).not.toBe(0);

      organisations.forEach((organisation) => {
        const owner = organisation.Owner;
      });
    });
  });

  describe('Organisation with path', () => {
    it('should return all owners', async () => {
      const { m, fetch } = fixture.meta;

      const pulls = [
        new Pull({
          extent: new Extent({
            objectType: m.Organisation,
          }),
          results: [
            new Result({
              fetch: fetch.Organisation({
                Owner: {},
              }),
            }),
          ],
        }),
      ];

      fixture.allors.context.reset();

      const loaded = await fixture.allors.context.load(new PullRequest({ pulls })).toPromise();

      const owners = loaded.collections.Owners as Person[];

      expect(owners.length).toBe(2);
    });

    it('should return all employees', async () => {
      const { m, fetch } = fixture.meta;

      const pulls = [
        new Pull({
          extent: new Extent({
            objectType: m.Organisation,
          }),
          results: [
            new Result({
              fetch: fetch.Organisation({
                Employees: {},
              }),
            }),
          ],
        }),
      ];

      fixture.allors.context.reset();

      const loaded = await fixture.allors.context.load(new PullRequest({ pulls })).toPromise();

      const employees = loaded.collections.Employees as Person[];

      expect(employees.length).toBe(3);
    });
  });

  describe('Organisation with typesafe path', () => {
    it('should return all employees', async () => {
      const { m, fetch } = fixture.meta;

      const pulls = [
        new Pull({
          extent: new Extent(m.Organisation),
          results: [
            new Result({
              fetch: fetch.Organisation({
                Employees: {},
              }),
            }),
          ],
        }),
      ];

      fixture.allors.context.reset();

      const loaded = await fixture.allors.context.load(new PullRequest({ pulls })).toPromise();

      const employees = loaded.collections.Employees as Person[];

      expect(employees.length).toBe(3);
    });
  });

  describe('Organisation with typesafe path and tree', () => {
    it('should return all people', async () => {
      const { m, fetch } = fixture.meta;

      const pulls = [
        new Pull({
          extent: new Extent(m.Organisation),
          results: [
            new Result({
              fetch: fetch.Organisation({
                Owner: {
                  include: {
                    Photo: {},
                  },
                },
              }),
            }),
          ],
        }),
      ];

      fixture.allors.context.reset();

      const loaded = await fixture.allors.context.load(new PullRequest({ pulls })).toPromise();

      const owners = loaded.collections.Owners as Person[];

      owners.forEach((v) => v.Photo);

      expect(owners.length).toBe(2);
    });
  });
});
