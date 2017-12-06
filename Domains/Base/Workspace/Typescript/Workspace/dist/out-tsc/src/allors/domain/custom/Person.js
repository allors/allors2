"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const Person_g_1 = require("../generated/Person.g");
Person_g_1.Person.prototype.hello = function () {
    return `Hello ${this.displayName}`;
};
Object.defineProperty(Person_g_1.Person.prototype, "displayName", {
    get() {
        if (this.FirstName || this.LastName) {
            if (this.FirstName && this.LastName) {
                return this.FirstName + " " + this.LastName;
            }
            else if (this.LastName) {
                return this.LastName;
            }
            else {
                return this.FirstName;
            }
        }
        if (this.UserName) {
            return this.UserName;
        }
        return "N/A";
    },
});
//# sourceMappingURL=Person.js.map