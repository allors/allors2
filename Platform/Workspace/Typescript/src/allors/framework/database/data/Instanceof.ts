import { ObjectType, PropertyType } from '../../meta';

import { Predicate } from './Predicate';

export class Instanceof implements Predicate {
  public propertyType: PropertyType;
  public objectType: ObjectType;

  constructor(fields?: Partial<Instanceof>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'Instanceof',
      propertytype: this.propertyType ? this.propertyType.id : undefined,
      objecttype: this.objectType.id,
    };
  }
}
