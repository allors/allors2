import { PropertyType } from './PropertyType';
import { RelationType } from './RelationType';
import { MetaPopulation } from './MetaPopulation';
import { ObjectType } from './ObjectType';

export class AssociationType implements PropertyType {
  metaPopulation: MetaPopulation;

  id: string;
  objectType: ObjectType;
  name: string;
  singular: string;
  isOne: boolean;

  constructor(public relationType: RelationType) {
    relationType.associationType = this;
    this.metaPopulation = relationType.metaPopulation;
  }

  get isMany(): boolean { return !this.isOne; }
}
