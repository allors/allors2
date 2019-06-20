import { domain } from '../domain';
import { PostalAddress } from '../generated/PostalAddress.g';

declare module '../generated/PostalAddress.g' {
  interface PostalAddress {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: PostalAddress = workspace.prototypeByName['PostalAddress'];

  Object.defineProperty(obj, 'displayName', {
    get(this: PostalAddress) {
      let fullAddress: string;
      if (this.Address1 || this.Address2 || this.Address3) {
        if (this.Address1) {
          fullAddress = this.Address1;
        } else if (this.Address2) {
          fullAddress = this.Address2;
        } else {
          fullAddress = this.Address3;
        }
      }

      if (fullAddress === '') {
        fullAddress += this.PostalCode;
      } else {
        fullAddress += ' ' + this.PostalCode;
      }

      if (fullAddress === '') {
        fullAddress += this.Locality;
      } else {
        fullAddress += ' ' + this.Locality;
      }

      if (fullAddress === '') {
        return fullAddress += this.Country.Name;
      } else {
        return fullAddress += ' ' + this.Country.Name;
      }
    },
  });

});
