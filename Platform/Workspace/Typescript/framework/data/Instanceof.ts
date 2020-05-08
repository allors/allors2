import { ObjectType, PropertyType } from '../meta';

import { Predicate } from './Predicate';

export class Instanceof implements Predicate {
  dependencies: string[];
  propertyType: PropertyType;
  objectType: ObjectType;

  constructor(fields?: Partial<Instanceof> | PropertyType) {
    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
    } else {
      Object.assign(this, fields);
    }
  }

  toJSON(): any {
    return {
      kind: 'Instanceof',
      dependencies: this.dependencies,
      propertytype: this.propertyType.id,
      objecttype: this.objectType.id,
    };
  }
}
