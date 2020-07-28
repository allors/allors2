import { PropertyType, ObjectType } from '../meta';
import { ParameterizablePredicate, ParameterizablePredicateArgs } from './ParameterizablePredicate';

export interface ExistArgs extends ParameterizablePredicateArgs, Pick<Exists, 'propertyType'> {}

export class Exists extends ParameterizablePredicate {
  propertyType: PropertyType;

  constructor(propertyType: PropertyType);
  constructor(args: ExistArgs);
  constructor(args: ExistArgs | PropertyType) {
    super();

    if ((args as PropertyType).objectType) {
      this.propertyType = args as PropertyType;
    } else {
      Object.assign(this, args);
      this.propertyType = (args as ExistArgs).propertyType;
    }
  }

  get objectType(): ObjectType {
    return this.propertyType.objectType;
  }

  public toJSON(): any {
    return {
      kind: 'Exists',
      dependencies: this.dependencies,
      propertytype: this.propertyType.id,
      parameter: this.parameter,
    };
  }
}
