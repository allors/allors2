import { async } from '@angular/core/testing';

import { PullRequest, Pull, Filter, Result, Fetch, Tree, TreeNode } from '../../allors/framework';
import { Person, Organisation } from '../../allors/domain';
import { Fixture } from '../Fixture.spec';

import 'jasmine-expect';

let fixture: Fixture;

let organisations: Organisation[] = [];

describe('Method', () => {
  beforeEach(async(() => {
    fixture = new Fixture();
  }));

  beforeEach(async () => {
    await fixture.init();

    const { pull } = fixture.meta;

    const pulls = [
      pull.Organisation()
    ];

    const loaded = await fixture.allors.context
      .load(new PullRequest({ pulls }))
      .toPromise();

    organisations = loaded.collections.Organisations as Organisation[];
  });

  describe('JustDoIt on Organisation',
    () => {
      it('should update JustDidIt', async () => {

        const { pull } = fixture.meta;
        const { context } = fixture.allors;

        const organisation = organisations[0];

        expect(organisation.JustDidIt).toBeFalse();

        const result = await context.invoke(organisation.JustDoIt).toPromise();
        expect(result.response.hasErrors).toBeFalse();

        const organisationPull = pull.Organisation({ object: organisation });
        await context.load(organisationPull).toPromise();

        context.session.reset();

        expect(organisation.JustDidIt).toBeTrue();
      });

    });
});
