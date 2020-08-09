import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';

import { Person, EmailAddress, TelecommunicationsNumber } from '@allors/domain/generated';

import { inlineLists } from 'common-tags';


export function extendPerson(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.Person);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: Person): string {
      if (this.FirstName || this.LastName) {
        return inlineLists`${[
          this.FirstName,
          this.MiddleName,
          this.LastName,
        ].filter((v) => v)}`;
      }

      if (this.UserName) {
        return this.UserName;
      }

      return 'N/A';
    },
  });

  Object.defineProperty(cls.prototype, 'displayEmail', {
    configurable: true,
    get(this: Person): string {
      const emailAddresses = this.PartyContactMechanisms.filter(
        (v) => v.ContactMechanism?.objectType === m.EmailAddress
      )
        .map((v) => {
          const emailAddress = v.ContactMechanism as EmailAddress;
          return emailAddress.ElectronicAddressString;
        })
        .filter((v) => v) as string[];

      return emailAddresses.join(', ');
    },
  });

  Object.defineProperty(cls.prototype, 'displayPhone', {
    configurable: true,
    get(this: Person): string {
      const telecommunicationsNumbers = this.PartyContactMechanisms.filter(
        (v) => v.ContactMechanism?.objectType === m.TelecommunicationsNumber
      );

      if (telecommunicationsNumbers.length > 0) {
        return telecommunicationsNumbers
          .map((v) => {
            const telecommunicationsNumber = v.ContactMechanism as TelecommunicationsNumber;
            return telecommunicationsNumber.displayName;
          })
          .reduce((acc: string, cur: string) => acc + ', ' + cur);
      }

      return '';
    },
  });
}
