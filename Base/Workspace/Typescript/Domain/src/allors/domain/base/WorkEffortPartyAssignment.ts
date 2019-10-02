import { domain } from '../domain';
import { WorkEffortPartyAssignment } from '../generated/WorkEffortPartyAssignment.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/WorkEffortPartyAssignment.g' {
  interface WorkEffortPartyAssignment {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.WorkEffortPartyAssignment).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: WorkEffortPartyAssignment) {
      if (this.Party) {
        return this.Party.displayName;
      }

      return 'N/A';
    },
  });

});
