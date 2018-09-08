import { PropertyType } from '../../meta';
import { ISessionObject } from '../../workspace/SessionObject';
import { Predicate } from './Predicate';

export class Equals implements Predicate {
  public propertyType: PropertyType;
  public value: ISessionObject | string | Date | boolean | number;

  constructor(fields?: Partial<Equals>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    var value = this.value;

    if (!this.propertyType.objectType.isUnit) {
      var object = value as ISessionObject;
      if (object && object.id) {
        value = object.id
      }
    }

    return {
      kind: 'Equals',
      propertytype: this.propertyType ? this.propertyType.id : undefined,
      value,
    };
  }
}
