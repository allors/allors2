"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class ExclusiveRoleType {
    get isMany() { return !this.isOne; }
}
exports.ExclusiveRoleType = ExclusiveRoleType;
