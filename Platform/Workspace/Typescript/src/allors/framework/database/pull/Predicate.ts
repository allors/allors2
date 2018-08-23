import { ObjectType, RoleType, PropertyType } from '../../meta';
import { Extent } from './Extent';
import { PredicateKind } from '../data/PredicateKind';

export class Predicate {

  public kind: PredicateKind;

  public propertyType: PropertyType;

  public roleType: RoleType;

  public objectType: ObjectType;

  public parameter: string;

  public operand: Predicate; 

  public operands: Predicate[];

  public object: string;

  public objects: string[];

  public value: string;

  public values: string[];

  public Extent: Extent;

  constructor(fields?: Partial<Predicate>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      kind: this.kind,
      objectType: this.objectType.id,
    };
  }
}
