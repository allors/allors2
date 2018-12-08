import { ObjectType, PropertyType, ObjectTypeRef } from '../../meta';

import { Predicate } from './Predicate';

export class Instanceof implements Predicate {
  public propertyType: PropertyType;
  public objectType: ObjectType | ObjectTypeRef;

  constructor(fields?: Partial<Instanceof> | PropertyType) {
    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    const metaObjectType = this.objectType as ObjectTypeRef;
    const objectType = metaObjectType.objectType ? metaObjectType.objectType : this.objectType as ObjectType;

    return {
      kind: 'Instanceof',
      propertytype: this.propertyType.id,
      objecttype: objectType.id,
    };
  }
}
