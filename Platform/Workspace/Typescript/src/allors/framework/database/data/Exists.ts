import { PropertyType } from '../../meta';
import { Predicate } from './Predicate';

export class Exists implements Predicate {
  public propertyType: PropertyType;

  constructor(fields?: Partial<Exists> | PropertyType) {
    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'Exists',
      propertytype: this.propertyType.id,
    };
  }
}
