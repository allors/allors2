import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';
import { ResponseType, PullResponse } from '@allors/protocol/system';

import { data, Meta } from '@allors/meta/generated';

import { syncResponse, securityResponse, securityResponse2 } from './fixture';
import { extend } from '../index';

import "jest-extended";

describe('Workspace',
  () => {

    let m: Meta;
    let workspace: Workspace;

    beforeEach(() => {
      m = new MetaPopulation(data) as Meta;
      workspace = new Workspace(m);
      extend(workspace);
    });

    it('should have its relations set when synced', () => {
      workspace.sync(syncResponse(m));

      const martien = workspace.get('3');

      expect(martien?.id).toBe( '3');
      expect(martien?.version).toBe( '1003');
      expect(martien?.objectType.name).toBe( 'Person');
      expect(martien?.roleByRoleTypeId.get(m.Person.FirstName.id)).toBe( 'Martien');
      expect(martien?.roleByRoleTypeId.get(m.Person.MiddleName.id)).toBe( 'van');
      expect(martien?.roleByRoleTypeId.get(m.Person.LastName.id)).toBe( 'Knippenberg');
      expect(martien?.roleByRoleTypeId.get(m.Person.IsStudent.id)).toBeUndefined();
      expect(martien?.roleByRoleTypeId.get(m.Person.BirthDate.id)).toBeUndefined();
    });

    describe('synced with same access control',
      () => {

        beforeEach(() => {
          workspace.sync(syncResponse(m));
        });

        it('should require load objects only for changed version', () => {
          const pullResponse: PullResponse = {
            hasErrors: false,
            objects: [
              ['101', '1101', '801'],
              ['102', '1102'],
              ['103', '1104'],
            ],
            responseType: ResponseType.Pull,
          };

          const requireLoad = workspace.diff(pullResponse);

          expect(requireLoad.objects.length).toBe( 1);
          expect(requireLoad.objects[0]).toBe( '103');
        });
      },
    );

    describe('synced with different security',
      () => {
        beforeEach(() => {
          workspace.sync(syncResponse(m));
        });

        it('should require load objects for all objects', () => {
          const pullResponse: PullResponse = {
            hasErrors: false,
            objects: [
              ['101', '1101', '801', '904'],
              ['102', '1102', '801'],
              ['103', '1103'],
            ],
            responseType: ResponseType.Pull,
          };

          const syncRequest = workspace.diff(pullResponse);

          expect(syncRequest.objects.length).toBe( 2);
          expect(syncRequest.objects).toIncludeSameMembers( ['101', '102']);
        });
      });


    describe('call security',
      () => {
        beforeEach(() => {
          workspace.sync(syncResponse(m));
          workspace.security(securityResponse(m));
        });

        it('with different accesscontrol but already existing permissions', () => {

          const accessControl801 = workspace.accessControlById.get('801');

          workspace.security(securityResponse2(m));

          const accessControl802 = workspace.accessControlById.get('802');

          if (accessControl802?.permissionIds) {
            for (const permission of accessControl802.permissionIds) {
              expect(accessControl801?.permissionIds ?? []).toContain( permission);
            }
          }

        });
      });
  },
);
