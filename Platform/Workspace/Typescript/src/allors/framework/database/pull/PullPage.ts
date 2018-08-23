export class PullPage {

  public skip: number;
  public take: number;

  constructor(fields?: Partial<PullPage>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      s: this.skip,
      t: this.take,
    };
  }
}
