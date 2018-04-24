import { domain } from "../domain";
import { PostalAddress } from "../generated/PostalAddress.g";

declare module "../generated/PostalAddress.g" {
  interface PostalAddress {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: PostalAddress = workspace.prototypeByName["PostalAddress"];

  Object.defineProperty(obj, "displayName", {
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

      if (fullAddress === "" && this.PostalBoundary.PostalCode) {
        fullAddress += this.PostalBoundary.PostalCode;
      } else {
        fullAddress += " " + this.PostalBoundary.PostalCode;
      }

      if (fullAddress === "" && this.PostalBoundary.Locality) {
        fullAddress += this.PostalBoundary.Locality;
      } else {
        fullAddress += " " + this.PostalBoundary.Locality;
      }

      if (fullAddress === "" && this.PostalBoundary.Country.Name) {
        return fullAddress += this.PostalBoundary.Country.Name;
      } else {
        return fullAddress += " " + this.PostalBoundary.Country.Name;
      }
    },
  });

});
