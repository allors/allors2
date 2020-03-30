import { PropertyType, ObjectType } from '../meta';
import { ISessionObject, serialize } from '../workspace/SessionObject';
import { ParametrizedPredicate } from './ParametrizedPredicate';
import { UnitTypes, CompositeTypes } from '../workspace/Types';

export class Equals extends ParametrizedPredicate {
  public propertyType: PropertyType;
  public value: UnitTypes;
  public object: CompositeTypes;

  constructor(fields?: Partial<Equals> | PropertyType) {
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
      kind: 'Equals',
      propertytype: this.propertyType.id,
      parameter: this.parameter,
      value: serialize(this.value),
      object: this.object && (this.object as ISessionObject).id ? (this.object as ISessionObject).id : this.object
    };
  }
}
