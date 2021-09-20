import { Workspace } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';

import { Person } from '@allors/domain/generated';

export function extendPerson(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const person = workspace.constructorByObjectType.get(m.Person)
    ?.prototype as any;

  person.hello = function (this: Person) {
    return `Hello ${this.displayName}`;
  };

  Object.defineProperty(person, 'displayName', {
    get(this: Person): string {
      if (this.FirstName || this.LastName) {
        if (this.FirstName && this.LastName) {
          return this.FirstName + ' ' + this.LastName;
        } else if (this.LastName) {
          return this.LastName;
        } else if (this.FirstName) {
          return this.FirstName;
        }
      }

      if (this.UserName) {
        return this.UserName;
      }

      return 'N/A';
    },
  });
}
