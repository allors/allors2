import { PropertyType } from '../../meta';

import { ISessionObject } from './../../workspace/SessionObject';
import { Predicate } from './Predicate';
import { Extent } from './Extent';

export class ContainedIn implements Predicate {
  public propertyType: PropertyType;
  public extent: Extent;
  public objects: Array<ISessionObject | string>;

  constructor(fields?: Partial<ContainedIn>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'ContainedIn',
      propertytype: this.propertyType ? this.propertyType.id : undefined,
      extent: this.extent,
      objects: this.objects ? this.objects.map((v) => (v as ISessionObject).id ? (v as ISessionObject).id : v) : undefined,
    };
  }
}
