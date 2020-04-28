import { Pull } from '../../data';

export class PullRequest {
  public pulls: Pull[];

  constructor(fields?: Partial<PullRequest>) {
    Object.assign(this, fields);
  }

  public toJSON() {
    return {
      p: this.pulls,
    };
  }
}
