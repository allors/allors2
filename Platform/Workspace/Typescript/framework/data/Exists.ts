import { PropertyType, ObjectType } from '../meta';
import { ParameterizablePredicate } from './ParameterizablePredicate';

export class Exists extends ParameterizablePredicate {
  propertyType: PropertyType;

  constructor(fields?: Partial<Exists> | PropertyType) {
    super();

    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
    } else {
      Object.assign(this, fields);
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
