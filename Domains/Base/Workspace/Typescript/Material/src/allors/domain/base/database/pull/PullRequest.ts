import { Query, Fetch } from '..';

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
