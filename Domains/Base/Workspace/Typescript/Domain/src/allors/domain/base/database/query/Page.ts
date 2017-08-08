export class Page {

  public skip: number;
  public take: number;

  constructor(fields?: Partial<Page>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      s: this.skip,
      t: this.take,
    };
  }
}
