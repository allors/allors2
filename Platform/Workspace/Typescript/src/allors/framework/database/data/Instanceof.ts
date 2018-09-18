import { ObjectType, PropertyType, MetaObjectType } from '../../meta';

import { Predicate } from './Predicate';

export class Instanceof implements Predicate {
  public propertyType: PropertyType;
  public objectType: ObjectType | MetaObjectType;

  constructor(fields?: Partial<Instanceof> | PropertyType, objectType?: ObjectType | MetaObjectType) {
    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
      this.objectType = objectType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    const metaObjectType = this.objectType as MetaObjectType;
    const objectType = metaObjectType._objectType ? metaObjectType._objectType : this.objectType as ObjectType;

    return {
      kind: 'Instanceof',
      propertytype: this.propertyType.id,
      objecttype: objectType.id,
    };
  }
}
