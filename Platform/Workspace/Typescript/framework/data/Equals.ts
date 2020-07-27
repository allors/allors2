import { PropertyType, ObjectType } from '../meta';
import { ISessionObject } from '../workspace/ISessionObject';
import { serialize } from '../workspace/SessionObject';
import { ParameterizablePredicate, ParameterizablePredicateArgs } from './ParameterizablePredicate';
import { UnitTypes, CompositeTypes } from '../workspace/Types';

export interface EqualsArgs extends ParameterizablePredicateArgs, Pick<Equals, 'propertyType' | 'value' | 'object'> {}

export class Equals extends ParameterizablePredicate {
  public propertyType: PropertyType;
  public value?: UnitTypes;
  public object?: CompositeTypes;

  constructor(propertyType: PropertyType);
  constructor(args: EqualsArgs);
  constructor(args: EqualsArgs | PropertyType) {
    super();

    if ((args as PropertyType).objectType) {
      this.propertyType = args as PropertyType;
    } else if (args) {
      Object.assign(this, args);
      this.propertyType = (args as EqualsArgs).propertyType;
    }
  }

  get objectType(): ObjectType {
    return this.propertyType.objectType;
  }

  public toJSON(): any {
    return {
      kind: 'Equals',
      dependencies: this.dependencies,
      propertytype: this.propertyType.id,
      parameter: this.parameter,
      value: serialize(this.value),
      object: this.object && (this.object as ISessionObject).id ? (this.object as ISessionObject).id : this.object,
    };
  }
}
