"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const Rx_1 = require("rxjs/Rx");
const framework_1 = require("@allors/framework");
class Filter {
    constructor(options) {
        this.options = options;
    }
    create() {
        return (search) => {
            if (!search.trim) {
                return Rx_1.Observable.empty();
            }
            const terms = search.trim().split(" ");
            const and = new framework_1.And();
            if (this.options.existRoletypes) {
                this.options.existRoletypes.forEach((roleType) => {
                    and.predicates.push(new framework_1.Exists({ roleType }));
                });
            }
            if (this.options.notExistRoletypes) {
                this.options.notExistRoletypes.forEach((roleType) => {
                    const not = new framework_1.Not();
                    and.predicates.push(not);
                    not.predicate = new framework_1.Exists({ roleType });
                });
            }
            terms.forEach((term) => {
                const or = new framework_1.Or();
                and.predicates.push(or);
                this.options.roleTypes.forEach((roleType) => {
                    or.predicates.push(new framework_1.Like({ roleType, value: term + "%" }));
                });
            });
            const query = [
                new framework_1.Query({
                    name: "results",
                    objectType: this.options.objectType,
                    predicate: and,
                    sort: this.options.roleTypes.map((roleType) => new framework_1.Sort({ roleType })),
                }),
            ];
            return this.options.scope
                .load("Pull", new framework_1.PullRequest({ query }))
                .map((loaded) => loaded.collections.results);
        };
    }
}
exports.Filter = Filter;
