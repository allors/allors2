"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const EmailAddress_g_1 = require("@allors/generated/dist/domain/EmailAddress.g");
Object.defineProperty(EmailAddress_g_1.EmailAddress.prototype, "displayName", {
    get() {
        if (this.ElectronicAddressString) {
            return this.ElectronicAddressString;
        }
        return "N/A";
    },
});
