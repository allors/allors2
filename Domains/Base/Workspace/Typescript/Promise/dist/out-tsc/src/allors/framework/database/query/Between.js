"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Between {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "Between",
            f: this.first,
            rt: this.roleType.id,
            s: this.second,
        };
    }
}
exports.Between = Between;
//# sourceMappingURL=Between.js.map