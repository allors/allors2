declare module '@allors/domain/generated' {
  interface Organisation {
    displayName: string;
  }

  interface Person {
    displayName: string;

    hello(): string;
  }
}

import { Workspace } from '@allors/domain/system';
import { extendOrganisation } from './Organisation';
import { extendPerson } from './Person';

export function extend(workspace: Workspace) {
  extendOrganisation(workspace);
  extendPerson(workspace);
}
