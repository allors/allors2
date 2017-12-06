"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Path {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            next: this.next,
            step: this.step.id,
        };
    }
}
exports.Path = Path;
//# sourceMappingURL=Path.js.map