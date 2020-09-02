import { Workspace } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';

import { Organisation } from '@allors/domain/generated';

export function extendOrganisation(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const person = workspace.constructorByObjectType.get(m.Organisation)?.prototype as any;

  Object.defineProperty(person, 'displayName', {
    get(this: Organisation): string {
      return this.Name;
    },
  });
}
