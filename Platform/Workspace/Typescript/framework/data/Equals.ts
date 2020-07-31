import { serialize } from '../workspace/SessionObject';
import { ParameterizablePredicateArgs, ParameterizablePredicate } from './ParameterizablePredicate';
import { PropertyType } from '../meta/PropertyType';
import { UnitTypes, CompositeTypes } from '../workspace/Types';
import { ObjectType } from '../meta/ObjectType';
import { ISessionObject } from '../workspace/ISessionObject';

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
    } else {
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
