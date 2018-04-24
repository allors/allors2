import { MetaObject } from "./MetaObject";
import { MetaPopulation } from "./MetaPopulation";
import { ObjectType } from "./ObjectType";

export class AssociationType implements MetaObject {
  public id: string;
  public name: string;
  public objectType: ObjectType;

  constructor(public metaPopulation: MetaPopulation) {
  }
}
