import { ObjectType, PropertyType } from '@allors/workspace/meta';
import { ISessionObject, CompositeTypes, UnitTypes,serialize } from '@allors/workspace/domain';

import { ParameterizablePredicateArgs, ParameterizablePredicate } from './ParameterizablePredicate';

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
