"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Instanceof {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "Instanceof",
            at: this.associationType ? this.associationType.id : undefined,
            ot: this.objectType.id,
            rt: this.roleType.id ? this.roleType.id : undefined,
        };
    }
}
exports.Instanceof = Instanceof;
