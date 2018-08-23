import { Fetch } from '../query/Fetch';
import { Query } from '../query/Query';
import { PullExtent } from "./PullExtent";

export class PullRequest {

  public fetches: Fetch[];

  public queries: Query[];

  public extents: PullExtent[];

  constructor(fields?: Partial<PullRequest>) {
    Object.assign(this, fields);
  }

  public toJSON() {
    return {
      f: this.fetches,
      q: this.queries,
      e: this.extents,
    };
  }
}
