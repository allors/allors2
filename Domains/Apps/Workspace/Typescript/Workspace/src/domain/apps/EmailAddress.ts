import { EmailAddress } from "../generated/EmailAddress.g";

declare module "../generated/EmailAddress.g" {
  interface EmailAddress {
    displayName;
  }
}

Object.defineProperty(EmailAddress.prototype, "displayName", {
  get(this: EmailAddress) {
    if (this.ElectronicAddressString) {
        return this.ElectronicAddressString;
    }

    return "N/A";
  },
});
