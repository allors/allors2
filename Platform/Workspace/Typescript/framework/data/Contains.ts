import { PropertyType, ObjectType } from '../meta';
import { ISessionObject } from '../workspace/SessionObject';
import { ParametrizedPredicate } from './ParametrizedPredicate';
import { CompositeTypes } from '../workspace/Types';

export class Contains extends ParametrizedPredicate {
  dependencies: string[];
  propertyType: PropertyType;
  parameter: string;
  object: CompositeTypes;

  constructor(fields?: Partial<Contains> | PropertyType) {
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
      kind: 'Contains',
      dependencies: this.dependencies,
      propertyType: this.propertyType.id,
      parameter: this.parameter,
      object: this.object ? ((this.object as ISessionObject).id ? (this.object as ISessionObject).id : this.object) : undefined,
    };
  }
}
