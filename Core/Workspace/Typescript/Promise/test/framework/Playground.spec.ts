import { Person, Media, Organisation, C1, User } from '../../src/allors/domain';
import { PullRequest, Pull, Filter, TreeNode, Tree, Result, Fetch, Equals, And } from '../../src/allors/framework';

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



  });
