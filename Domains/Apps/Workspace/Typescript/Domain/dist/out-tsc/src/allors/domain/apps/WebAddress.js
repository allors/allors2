"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const WebAddress_g_1 = require("../generated/WebAddress.g");
Object.defineProperty(WebAddress_g_1.WebAddress.prototype, "displayName", {
    get() {
        if (this.ElectronicAddressString) {
            return this.ElectronicAddressString;
        }
        return "N/A";
    },
});
//# sourceMappingURL=WebAddress.js.map