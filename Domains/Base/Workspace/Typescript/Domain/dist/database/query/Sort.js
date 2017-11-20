"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Sort {
    constructor(fields) {
        this.direction = "Asc";
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            d: this.direction,
            rt: this.roleType.id,
        };
    }
}
exports.Sort = Sort;
