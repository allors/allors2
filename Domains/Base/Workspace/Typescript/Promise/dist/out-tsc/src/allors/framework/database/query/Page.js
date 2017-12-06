"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Page {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            s: this.skip,
            t: this.take,
        };
    }
}
exports.Page = Page;
//# sourceMappingURL=Page.js.map