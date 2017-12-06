"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Or {
    constructor(fields) {
        Object.assign(this, fields);
        this.predicates = this.predicates ? this.predicates : [];
    }
    toJSON() {
        return {
            _T: "Or",
            ps: this.predicates,
        };
    }
}
exports.Or = Or;
//# sourceMappingURL=Or.js.map