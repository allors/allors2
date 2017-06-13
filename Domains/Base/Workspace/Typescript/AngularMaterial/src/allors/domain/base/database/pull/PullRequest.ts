import { Query } from './Query';
import { Fetch } from './Fetch';

export class PullRequest {
    query: Query[];

    fetch: Fetch[];

    constructor(fields?: Partial<PullRequest>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        q: this.query,
        f: this.fetch
      };
    }
}
