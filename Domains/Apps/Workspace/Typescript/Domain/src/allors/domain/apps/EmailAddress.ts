import { EmailAddress } from "@generatedDomain/EmailAddress.g";

declare module "@generatedDomain/EmailAddress.g" {
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
