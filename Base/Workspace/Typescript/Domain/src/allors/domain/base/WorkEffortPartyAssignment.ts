import { domain } from '../domain';
import { WorkEffortPartyAssignment } from '../generated/WorkEffortPartyAssignment.g';

declare module '../generated/WorkEffortPartyAssignment.g' {
  interface WorkEffortPartyAssignment {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: WorkEffortPartyAssignment = workspace.prototypeByName['WorkEffortPartyAssignment'];

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
