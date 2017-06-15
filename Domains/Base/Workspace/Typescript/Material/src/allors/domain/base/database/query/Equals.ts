import { RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Equals implements Predicate {
  roleType: RoleType;
  value: any;

  constructor(fields?: Partial<Equals>) {
    Object.assign(this, fields);
  }

  toJSON() {
    let value = null;
    if (this.roleType.objectType.isUnit) {
      value = this.value;
    } else {
      value = this.value ? this.value.id : null;
    }

    return {
      _T: 'Equals',
      rt: this.roleType.id,
      v: value,
    };
  }
}
