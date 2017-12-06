"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const Organisation_g_1 = require("../generated/Organisation.g");
Object.defineProperty(Organisation_g_1.Organisation.prototype, "displayName", {
    get() {
        if (this.Name) {
            return this.Name;
        }
        return "N/A";
    },
});
//# sourceMappingURL=Organisation.js.map