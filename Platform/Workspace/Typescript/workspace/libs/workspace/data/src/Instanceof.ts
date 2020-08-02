import { ObjectType, PropertyType } from '@allors/workspace/meta';

import { ParameterizablePredicate, ParameterizablePredicateArgs } from './ParameterizablePredicate';

export interface InstanceofArgs extends ParameterizablePredicateArgs, Pick<Instanceof, 'propertyType' | 'objectType'> {}

export class Instanceof extends ParameterizablePredicate {
  propertyType: PropertyType;
  instanceObjectType?: ObjectType;

  constructor(propertyType: PropertyType);
  constructor(args: InstanceofArgs);
  constructor(args: InstanceofArgs | PropertyType) {
    super();

    if ((args as PropertyType).objectType) {
      this.propertyType = args as PropertyType;
    } else {
      Object.assign(this, args);
      this.propertyType = (args as InstanceofArgs).propertyType;
    }
  }

  get objectType(): ObjectType {
    return this.propertyType.objectType;
  }

  toJSON(): any {
    return {
      kind: 'Instanceof',
      dependencies: this.dependencies,
      propertytype: this.propertyType.id,
      parameter: this.parameter,
      objecttype: this.instanceObjectType?.id,
    };
  }
}
