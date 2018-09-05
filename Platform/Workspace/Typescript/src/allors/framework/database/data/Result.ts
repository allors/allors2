import { ObjectType } from "../../meta";
import { Fetch } from "./Fetch";

export class Result {
  public objectType: ObjectType;

  public name: string;

  public skip: number;

  public take: number;

  public fetchRef: string

  public fetch: Fetch;

  constructor(fields?: Partial<Result>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      name: this.name,
      skip: this.skip,
      take: this.take,
      fetchRef: this.fetchRef,
      fetch: this.fetch ? new Fetch(Object.assign({}, this.fetch, { objectType: this.objectType })) : undefined,
    };
    
  }
}
