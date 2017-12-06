import { TelecommunicationsNumber } from "../generated/TelecommunicationsNumber.g";

declare module "../generated/TelecommunicationsNumber.g" {
  interface TelecommunicationsNumber {
    displayName;
  }
}

Object.defineProperty(TelecommunicationsNumber.prototype, "displayName", {
  get(this: TelecommunicationsNumber) {
    let numberString: string = "";
    if (this.CountryCode || this.AreaCode) {
      if (this.CountryCode && this.AreaCode) {
        numberString = this.CountryCode + " " + this.AreaCode;
      } else if (this.CountryCode) {
        numberString = this.CountryCode;
      } else {
        numberString = this.AreaCode;
      }
    }

    if (numberString === "" && this.ContactNumber) {
      return numberString = this.ContactNumber;
    } else {
      return numberString += " " + this.ContactNumber;
    }
  },
});
