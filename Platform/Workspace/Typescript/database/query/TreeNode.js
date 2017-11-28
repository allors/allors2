"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class TreeNode {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            n: this.nodes,
            rt: this.roleType.id,
        };
    }
}
exports.TreeNode = TreeNode;
