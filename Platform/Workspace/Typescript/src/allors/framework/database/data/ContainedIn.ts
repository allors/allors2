import { PropertyType } from '../../meta';

import { ISessionObject } from './../../workspace/SessionObject';
import { Predicate } from './Predicate';
import { Extent } from './Extent';

export class ContainedIn implements Predicate {
  public propertyType: PropertyType;
  public extent: Extent;
  public objects: Array<ISessionObject | string>;

  constructor(fields?: Partial<ContainedIn> | PropertyType, value?: Extent | Array<ISessionObject | string>) {
    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
      if (value instanceof Array) {
        this.objects = value;
      } else {
        this.extent = value;
      }
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'ContainedIn',
      propertytype: this.propertyType.id,
      extent: this.extent,
      objects: this.objects ? this.objects.map((v) => (v as ISessionObject).id ? (v as ISessionObject).id : v) : undefined,
    };
  }
}
