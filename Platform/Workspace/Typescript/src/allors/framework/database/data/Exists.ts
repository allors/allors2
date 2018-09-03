import { PropertyType } from '../../meta';
import { Predicate } from './Predicate';

export class Exists implements Predicate {
  public propertyType: PropertyType;

  constructor(fields?: Partial<Exists>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'Exists',
      propertytype: this.propertyType ? this.propertyType.id : undefined,
    };
  }
}
