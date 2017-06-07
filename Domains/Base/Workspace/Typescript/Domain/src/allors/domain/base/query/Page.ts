export class Page {

    skip: number;
    take: number;

    constructor(fields?: Partial<Page>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        s: this.skip,
        t: this.take,
      };
    }
}
