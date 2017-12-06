"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class PullRequest {
    constructor(fields) {
        Object.assign(this, fields);
    }
    toJSON() {
        return {
            f: this.fetch,
            q: this.query,
        };
    }
}
exports.PullRequest = PullRequest;
//# sourceMappingURL=PullRequest.js.map