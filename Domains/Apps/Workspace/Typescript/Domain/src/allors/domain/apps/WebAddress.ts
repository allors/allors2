import { WebAddress } from "../generated/WebAddress.g";

declare module "../generated/WebAddress.g" {
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
