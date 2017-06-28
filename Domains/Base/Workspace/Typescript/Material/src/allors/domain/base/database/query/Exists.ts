import { AssociationType, RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Exists implements Predicate {
  associationType: AssociationType;
  roleType: RoleType;

  constructor(fields?: Partial<Exists>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'Exists',
      at: this.associationType ? this.associationType.id : undefined,
      rt: this.roleType.id ? this.roleType.id : undefined,
    };
  }
}
