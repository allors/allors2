export class Page {

  skip: number;
  take: number;

  constructor(fields?: Partial<Page>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      s: this.skip,
      t: this.take,
    };
  }
}
