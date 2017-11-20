"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Equals {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        let value;
        if (this.roleType.objectType.isUnit) {
            return {
                _T: "Equals",
                at: this.associationType ? this.associationType.id : undefined,
                rt: this.roleType ? this.roleType.id : undefined,
                v: this.value,
            };
        }
        else {
            return {
                _T: "Equals",
                at: this.associationType ? this.associationType.id : undefined,
                rt: this.roleType ? this.roleType.id : undefined,
                o: this.value ? this.value.id : undefined,
            };
        }
    }
}
exports.Equals = Equals;
