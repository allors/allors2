"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Like {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "Like",
            rt: this.roleType.id,
            v: this.value,
        };
    }
}
exports.Like = Like;
//# sourceMappingURL=Like.js.map