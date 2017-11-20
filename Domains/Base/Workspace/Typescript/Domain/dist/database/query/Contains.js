"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Contains {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "Contains",
            at: this.associationType ? this.associationType.id : undefined,
            o: this.object ? this.object.id : undefined,
            rt: this.roleType.id ? this.roleType.id : undefined,
        };
    }
}
exports.Contains = Contains;
