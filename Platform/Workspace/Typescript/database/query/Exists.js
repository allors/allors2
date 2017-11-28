"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Exists {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "Exists",
            at: this.associationType ? this.associationType.id : undefined,
            rt: this.roleType.id ? this.roleType.id : undefined,
        };
    }
}
exports.Exists = Exists;
