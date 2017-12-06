"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Not {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "Not",
            p: this.predicate,
        };
    }
}
exports.Not = Not;
//# sourceMappingURL=Not.js.map