"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class And {
    constructor(fields) {
        Object.assign(this, fields);
        this.predicates = this.predicates ? this.predicates : [];
    }
    toJSON() {
        return {
            _T: "And",
            ps: this.predicates,
        };
    }
}
exports.And = And;
