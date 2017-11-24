"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const WebAddress_g_1 = require("@allors/generated/dist/domain/WebAddress.g");
Object.defineProperty(WebAddress_g_1.WebAddress.prototype, "displayName", {
    get() {
        if (this.ElectronicAddressString) {
            return this.ElectronicAddressString;
        }
        return "N/A";
    },
});
