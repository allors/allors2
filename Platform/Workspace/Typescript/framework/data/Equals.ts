import { PropertyType } from '../meta';
import { UnitTypes, CompositeTypes } from '../protocol/Types';
import { ISessionObject } from '../workspace/SessionObject';
import { ParametrizedPredicate } from './ParametrizedPredicate';

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

  public toJSON(): any {

    return {
      kind: 'Equals',
      propertytype: this.propertyType.id,
      parameter: this.parameter,
      value: this.value,
      object: this.object && (this.object as ISessionObject).id ? (this.object as ISessionObject).id : this.object
    };
  }
}
