"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class ContainedIn {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            _T: "ContainedIn",
            at: this.associationType ? this.associationType.id : undefined,
            rt: this.roleType ? this.roleType.id : undefined,
            q: this.query,
            o: this.objects ? this.objects.map((v) => v.id) : undefined,
        };
    }
}
exports.ContainedIn = ContainedIn;
