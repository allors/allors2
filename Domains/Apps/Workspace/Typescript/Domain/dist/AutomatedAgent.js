"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const AutomatedAgent_g_1 = require("@allors/generated/dist/domain/AutomatedAgent.g");
Object.defineProperty(AutomatedAgent_g_1.AutomatedAgent.prototype, "displayName", {
    get() {
        if (this.UserName) {
            return this.UserName;
        }
        return "N/A";
    },
});
