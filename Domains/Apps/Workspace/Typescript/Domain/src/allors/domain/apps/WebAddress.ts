import { WebAddress } from "@generatedDomain/WebAddress.g";

declare module "@generatedDomain/WebAddress.g" {
  interface WebAddress {
    displayName;
  }
}

Object.defineProperty(WebAddress.prototype, "displayName", {
  get(this: WebAddress) {
    if (this.ElectronicAddressString) {
        return this.ElectronicAddressString;
    }

    return "N/A";
  },
});
