"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Query {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        function isObjectTyped(objectType) {
            return objectType.ObjectType !== undefined;
        }
        return {
            ex: this.except,
            i: this.include,
            in: this.intersect,
            n: this.name,
            ot: isObjectTyped(this.objectType) ? this.objectType.ObjectType.id : this.objectType.id,
            p: this.predicate,
            pa: this.page,
            s: this.sort,
            un: this.union,
        };
    }
}
exports.Query = Query;
