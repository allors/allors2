"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Fetch {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            id: this.id,
            include: this.include,
            name: this.name,
            path: this.path,
        };
    }
}
exports.Fetch = Fetch;
