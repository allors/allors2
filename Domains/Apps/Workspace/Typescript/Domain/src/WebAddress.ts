import { WebAddress } from "@allors/generated/dist/domain/WebAddress.g";

declare module "@allors/generated/dist/domain/WebAddress.g" {
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
