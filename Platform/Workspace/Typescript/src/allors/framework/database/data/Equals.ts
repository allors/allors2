import { PropertyType } from '../../meta';
import { ISessionObject } from '../../workspace/SessionObject';
import { Predicate } from './Predicate';

export class Equals implements Predicate {
  public propertyType: PropertyType;
  public value: string | Date | boolean | number;
  public object: ISessionObject | string;

  constructor(fields?: Partial<Equals> | PropertyType, valueOrObject?: ISessionObject | string | Date | boolean | number) {
    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
      if (this.propertyType.objectType.isUnit) {
        this.value = valueOrObject as any;
      } else {
        this.object = valueOrObject as any;
      }
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    return {
      kind: 'Equals',
      propertytype: this.propertyType.id,
      value: this.value,
      object: this.object && (this.object as ISessionObject).id ? (this.object as ISessionObject).id : this.object
    };
  }
}
