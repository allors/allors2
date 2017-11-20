import { Fetch } from "../query/Fetch";
import { Query } from "../query/Query";

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
