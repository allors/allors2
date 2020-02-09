import { domain } from '../domain';
import { WorkEffortPartyAssignment } from '../generated/WorkEffortPartyAssignment.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/WorkEffortPartyAssignment.g' {
  interface WorkEffortPartyAssignment {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.WorkEffortPartyAssignment);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: WorkEffortPartyAssignment) {
      return this.Party?.displayName ?? 'N/A';
    },
  });

});
