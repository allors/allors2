import { RoleType } from '../../../meta';
import { Predicate } from './Predicate';

export class Equals extends Predicate {
  roleType: RoleType;
  value: any;

  toJSON() {

    let value = null;
    if (this.roleType.objectType.isUnit) {
      value = this.value;
    } else {
      value = this.value ? this.value.id : null;
    }

    return {
      rt: this.roleType.name,
      v: value,
    };
  }
}
