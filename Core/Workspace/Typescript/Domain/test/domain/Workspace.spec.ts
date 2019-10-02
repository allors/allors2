import { assert } from 'chai';
import 'mocha';

import { domain } from '../../src/allors/domain';
import { MetaPopulation, PullResponse, PushResponse, ResponseType, Session, Workspace } from '../../src/allors/framework';
import { data, Meta } from '../../src/allors/meta';

import { syncResponse } from './fixture';
import { Compressor } from '../../src/allors/framework/protocol/Compressor';

describe('Workspace',
  () => {

    let m: Meta;
    let workspace: Workspace;

    beforeEach(() => {
      m = new MetaPopulation(data) as Meta;
      workspace = new Workspace(m);
      domain.apply(workspace);
    });

    it('should have its relations set when synced', () => {
      workspace.sync(syncResponse(m));

      const martien = workspace.get('3');

      assert.equal(martien.id, '3');
      assert.equal(martien.version, '1003');
      assert.equal(martien.objectType.name, 'Person');
      assert.equal(martien.roleByRoleTypeId.get(m.Person.FirstName.id), 'Martien');
      assert.equal(martien.roleByRoleTypeId.get(m.Person.MiddleName.id), 'van');
      assert.equal(martien.roleByRoleTypeId.get(m.Person.LastName.id), 'Knippenberg');
      assert.isUndefined(martien.roleByRoleTypeId.get(m.Person.IsStudent.id));
      assert.isUndefined(martien.roleByRoleTypeId.get(m.Person.BirthDate.id));
    });

    describe('synced with same access control',
      () => {

        beforeEach(() => {
          workspace.sync(syncResponse(m));
        });

        it('should require load objects only for changed version', () => {
          const c = new Compressor();

          const pullResponse: PullResponse = {
            hasErrors: false,
            objects: [
              ['101', '1101', c.write('801')],
              ['102', '1102'],
              ['103', '1104'],
            ],
            responseType: ResponseType.Pull,
          };

          const requireLoad = workspace.diff(pullResponse);

          assert.equal(requireLoad.objects.length, 1);
          assert.equal(requireLoad.objects[0], '103');
        });

      },
    );

    describe('synced with different security',
      () => {
        beforeEach(() => {
          workspace.sync(syncResponse(m));
        });

        it('should require load objects for all objects', () => {
          const c = new Compressor();

          const pullResponse: PullResponse = {
            hasErrors: false,
            objects: [
              ['101', '1101', c.write('801'), c.write('904')],
              ['102', '1102', c.write('801')],
              ['103', '1103'],
            ],
            responseType: ResponseType.Pull,
          };

          const requireLoad = workspace.diff(pullResponse);

          assert.equal(requireLoad.objects.length, 2);
          assert.sameMembers(requireLoad.objects, ['101', '102']);
        });
      });
  },
);
