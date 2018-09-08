import { PropertyType } from '../../meta';

import { ISessionObject } from './../../workspace/SessionObject';
import { Predicate } from './Predicate';

export class Contains implements Predicate {
  public propertyType: PropertyType;
  public object: ISessionObject | string;

  constructor(fields?: Partial<Contains>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'Contains',
      propertytype: this.propertyType ? this.propertyType.id : undefined,
      o: this.object ? (this.object as ISessionObject).id ? (this.object as ISessionObject).id : this.object : undefined,
    };
  }
}
