import { domain } from '../domain';
import { Person } from '../generated/Person.g';
import { Meta } from '../../meta/generated/domain.g';
import { EmailAddress, TelecommunicationsNumber } from '../generated';

declare module '../generated/Person.g' {
    interface Person {
        displayName;
        displayEmail;
        displayPhone;
    }
}

domain.extend((workspace) => {

    const m = workspace.metaPopulation as Meta;
    const obj = workspace.constructorByObjectType.get(m.Person).prototype as any;

    Object.defineProperty(obj, 'displayName', {
        configurable: true,
        get(this: Person): string {
            if (this.FirstName || this.LastName) {
                let name = null;
                if (this.FirstName) {
                    name = this.FirstName;
                }

                if (this.MiddleName) {
                    if (name != null) {
                        name += ' ' + this.MiddleName;
                    } else {
                        name = this.MiddleName;
                    }
                }

                if (this.LastName) {
                    if (name != null) {
                        name += ' ' + this.LastName;
                    } else {
                        name = this.LastName;
                    }
                }

                return name;
            }

            if (this.UserName) {
                return this.UserName;
            }

            return 'N/A';
        },
    });

    Object.defineProperty(obj, 'displayEmail', {
        configurable: true,
        get(this: Person): string {
            const emailAddresses = this.PartyContactMechanisms.filter((v) => v.ContactMechanism.objectType === m.EmailAddress);

            if (emailAddresses.length > 0) {
                return emailAddresses
                .map((v) => {
                    const emailAddress = v.ContactMechanism as EmailAddress;
                    return emailAddress.ElectronicAddressString;
                })
                .reduce((acc: string, cur: string) => acc + ', ' + cur);
            }
        },
    });

    Object.defineProperty(obj, 'displayPhone', {
        configurable: true,
        get(this: Person): string {
            const telecommunicationsNumbers = this.PartyContactMechanisms.filter((v) => v.ContactMechanism.objectType === m.TelecommunicationsNumber);

            if (telecommunicationsNumbers.length > 0) {
                return  telecommunicationsNumbers
                .map((v) => {
                    const telecommunicationsNumber = v.ContactMechanism as TelecommunicationsNumber;
                    return telecommunicationsNumber.displayName;
                })
                .reduce((acc: string, cur: string) => acc + ', ' + cur);
            }
        },
    });
});
