import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { WorkEffortPartyAssignment } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';

export function extendWorkEffortPartyAssignment(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.WorkEffortPartyAssignment);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: WorkEffortPartyAssignment) {
      return this.Party?.displayName ?? 'N/A';
    },
  });
}
