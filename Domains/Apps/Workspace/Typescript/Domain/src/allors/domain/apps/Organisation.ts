import { domain } from '../domain';
import { Organisation } from '../generated/Organisation.g';
import { Meta } from '../../meta';
import { PostalAddress } from '../generated';

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

    const obj: Organisation = workspace.prototypeByName['Organisation'];

    Object.defineProperties(obj, {
        displayName: {
            get(this: Organisation): string {
                return this.Name || 'N/A';
            },
        },
        displayClassification: {
            get(this: Organisation): string {
                return this.CustomClassifications.map(w => w.Name).join(', ');
            },
        },
        displayAddress: {
            get(this: Organisation): string {
                const postalAddress = this.GeneralCorrespondence as PostalAddress;
                if (postalAddress) {
                    return `${postalAddress.Address1 ? postalAddress.Address1 : ''} ${postalAddress.Address2 ? postalAddress.Address2 : ''} ${postalAddress.Address3 ? postalAddress.Address3 : ''}`;
                }
            },
        },
        displayAddress2: {
            get(this: Organisation): string {
                const postalAddress = this.GeneralCorrespondence as PostalAddress;
                if (postalAddress) {
                    return `${postalAddress.PostalBoundary ? postalAddress.PostalBoundary.PostalCode : ''} ${postalAddress.PostalBoundary ? postalAddress.PostalBoundary.Locality : ''}`;
                }
            },
        },
        displayAddress3: {
            get(this: Organisation): string {
                const postalAddress = this.GeneralCorrespondence as PostalAddress;
                if (postalAddress) {
                    return `${postalAddress.PostalBoundary.Country ? postalAddress.PostalBoundary.Country.Name : ''}`;
                }
            },
        },
        displayPhone: {
            get(this: Organisation): string {
                return `${this.GeneralPhoneNumber ? this.GeneralPhoneNumber.displayName : ''}`;
            },
        },
    });

});
