import { EmailAddress } from "@allors/generated/dist/domain/EmailAddress.g";

declare module "@allors/generated/dist/domain/EmailAddress.g" {
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
