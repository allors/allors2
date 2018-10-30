import { domain } from '../domain';
import { Person } from '../generated/Person.g';

declare module '../generated/Person.g' {
    interface Person {
        displayName;
        displayEmail;
        displayPhone;
    }
}

domain.extend((workspace) => {

    const obj: Person = workspace.prototypeByName['Person'];

    Object.defineProperty(obj, 'displayName', {
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

            return "N/A";
        },
    });

    Object.defineProperty(obj, 'displayEmail', {
        get(this: Person): string {
            const email = this.GeneralEmail;
            if (email) {
                return email.ElectronicAddressString;
            }
        },
    });

    Object.defineProperty(obj, 'displayPhone', {
        get(this: Person): string {

            const phone = this.GeneralPhoneNumber;
            if (phone) {
                return `${phone.CountryCode} ${phone.AreaCode} ${phone.ContactNumber}`;
            }
        },
    });
});
