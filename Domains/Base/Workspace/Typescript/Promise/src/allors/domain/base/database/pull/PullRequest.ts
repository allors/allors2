import { Fetch, Query } from "..";

export class PullRequest {
    public query: Query[];

    public fetch: Fetch[];

    constructor(fields?: Partial<PullRequest>) {
       Object.assign(this, fields);
    }

    public toJSON() {
      return {
        f: this.fetch,
        q: this.query,
      };
    }
}
