import { Fetch } from "../../database/query/Fetch";
import { Query } from "../../database/query/Query";

export class PullRequest {

  public fetches: Fetch[];

  public queries: Query[];

  constructor(fields?: Partial<PullRequest>) {
    Object.assign(this, fields);
  }

  public toJSON() {
    return {
      f: this.fetches,
      q: this.queries,
    };
  }
}
