import { Fetch } from './Fetch';

export class Result {
  public fetchRef: string;

  public fetch: Fetch;

  public name: string;

  public skip: number;

  public take: number;

  constructor(fields?: Partial<Result>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      name: this.name,
      skip: this.skip,
      take: this.take,
      fetchRef: this.fetchRef,
      fetch: this.fetch,
    };

  }
}
