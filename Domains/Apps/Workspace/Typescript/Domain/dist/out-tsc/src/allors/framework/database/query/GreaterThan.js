"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class GreaterThan {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "GreaterThan",
            rt: this.roleType.id,
            v: this.value,
        };
    }
}
exports.GreaterThan = GreaterThan;
//# sourceMappingURL=GreaterThan.js.map