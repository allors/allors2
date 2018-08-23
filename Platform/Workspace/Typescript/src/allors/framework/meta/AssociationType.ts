import { PropertyType } from './PropertyType';
import { MetaPopulation } from './MetaPopulation';
import { ObjectType } from './ObjectType';

export class AssociationType implements PropertyType {
  public id: string;
  public name: string;
  public objectType: ObjectType;

  constructor(public metaPopulation: MetaPopulation) {
  }
}
