"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const TelecommunicationsNumber_g_1 = require("../generated/TelecommunicationsNumber.g");
Object.defineProperty(TelecommunicationsNumber_g_1.TelecommunicationsNumber.prototype, "displayName", {
    get() {
        let numberString = "";
        if (this.CountryCode || this.AreaCode) {
            if (this.CountryCode && this.AreaCode) {
                numberString = this.CountryCode + " " + this.AreaCode;
            }
            else if (this.CountryCode) {
                numberString = this.CountryCode;
            }
            else {
                numberString = this.AreaCode;
            }
        }
        if (numberString === "" && this.ContactNumber) {
            return numberString = this.ContactNumber;
        }
        else {
            return numberString += " " + this.ContactNumber;
        }
    },
});
//# sourceMappingURL=TelecommunicationsNumber.js.map