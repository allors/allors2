import { domain } from '../domain';
import { Organisation } from '../generated/Organisation.g';

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
                return this.PartyName || this.Name || 'N/A';
            },
        },
        displayClassification: {
            get(this: Organisation): string {
                return this.CustomClassifications.map(w => w.Name).join(', ');
            },
        },
        displayAddress: {
            get(this: Organisation): string {
                return `${this.GeneralCorrespondence && this.GeneralCorrespondence.Address1 ? this.GeneralCorrespondence.Address1 : ''} ${this.GeneralCorrespondence && this.GeneralCorrespondence.Address2 ? this.GeneralCorrespondence.Address2 : ''} ${this.GeneralCorrespondence && this.GeneralCorrespondence.Address3 ? this.GeneralCorrespondence.Address3 : ''}`;
            },
        },
        displayAddress2: {
            get(this: Organisation): string {
                return `${this.GeneralCorrespondence && this.GeneralCorrespondence.PostalBoundary ? this.GeneralCorrespondence.PostalBoundary.PostalCode : ''} ${this.GeneralCorrespondence && this.GeneralCorrespondence.PostalBoundary ? this.GeneralCorrespondence.PostalBoundary.Locality : ''}`;
                },
        },
        displayAddress3: {
            get(this: Organisation): string {
                return  `${this.GeneralCorrespondence && this.GeneralCorrespondence.PostalBoundary.Country ? this.GeneralCorrespondence.PostalBoundary.Country.Name : ''}`;
            },
        },
        displayPhone: {
            get(this: Organisation): string {
                return `${this.GeneralPhoneNumber ? this.GeneralPhoneNumber.CountryCode : ''} ${this.GeneralPhoneNumber && this.GeneralPhoneNumber.AreaCode ? this.GeneralPhoneNumber.AreaCode : ''} ${this.GeneralPhoneNumber ? this.GeneralPhoneNumber.ContactNumber : ''}`;
            },
        },
    });

});
