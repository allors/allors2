import { Pull } from '@allors/workspace/data';

export class PullRequest {
  public pulls?: Pull[];

  constructor(fields?: Partial<PullRequest>) {
    Object.assign(this, fields);
  }

  public toJSON() {
    return {
      p: this.pulls,
    };
  }
}
