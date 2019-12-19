import { domain } from '../domain';
import { Organisation } from '../generated/Organisation.g';
import { PostalAddress, TelecommunicationsNumber } from '../generated';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/Organisation.g' {
    interface Organisation {
        displayName: string;
        displayClassification: string;
        displayPhone: string;
        displayAddress: string;
        displayAddress2: string;
        displayAddress3: string;
    }
}

domain.extend((workspace) => {

    const m = workspace.metaPopulation as Meta;
    const obj = workspace.constructorByObjectType.get(m.Organisation).prototype as any;

    Object.defineProperties(obj, {
        displayName: {
            configurable: true,
            get(this: Organisation): string {
                return this.Name || 'N/A';
            },
        },
        displayClassification: {
            configurable: true,
            get(this: Organisation): string {
                return this.CustomClassifications.map(w => w.Name).join(', ');
            },
        },
        displayAddress: {
            configurable: true,
            get(this: Organisation): string {
                if (this.GeneralCorrespondence && this.GeneralCorrespondence.objectType.name === 'PostalAddress') {
                    const postalAddress = this.GeneralCorrespondence as PostalAddress;
                    return `${postalAddress.Address1 ? postalAddress.Address1 : ''} ${postalAddress.Address2 ? postalAddress.Address2 : ''} ${postalAddress.Address3 ? postalAddress.Address3 : ''}`;
                }

                return '';
            },
        },
        displayAddress2: {
            configurable: true,
            get(this: Organisation): string {
                if (this.GeneralCorrespondence && this.GeneralCorrespondence.objectType.name === 'PostalAddress') {
                    const postalAddress = this.GeneralCorrespondence as PostalAddress;
                    return `${postalAddress.PostalCode} ${postalAddress.Locality}`;
                }
            },
        },
        displayAddress3: {
            configurable: true,
            get(this: Organisation): string {
                if (this.GeneralCorrespondence && this.GeneralCorrespondence.objectType.name === 'PostalAddress') {
                    const postalAddress = this.GeneralCorrespondence as PostalAddress;
                    return `${postalAddress.Country ? postalAddress.Country.Name : ''}`;
                }
                return '';
            },
        },
        displayPhone: {
            configurable: true,
            get(this: Organisation): string {
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
        },
    });

});
