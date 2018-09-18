import { PropertyType } from '../../meta';

import { ISessionObject } from './../../workspace/SessionObject';
import { Predicate } from './Predicate';

export class Contains implements Predicate {
  public propertyType: PropertyType;
  public object: ISessionObject | string;

  constructor(fields?: Partial<Contains> | PropertyType, object?: ISessionObject | string) {
    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
      this.object = object;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'Contains',
      propertyType: this.propertyType.id,
      object: this.object ? (this.object as ISessionObject).id ? (this.object as ISessionObject).id : this.object : undefined,
    };
  }
}
