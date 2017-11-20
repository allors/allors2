"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class LessThan {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "LessThan",
            rt: this.roleType.id,
            v: this.value,
        };
    }
}
exports.LessThan = LessThan;
