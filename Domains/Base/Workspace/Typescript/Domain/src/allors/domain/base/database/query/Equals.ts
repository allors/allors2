import { AssociationType, RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Equals implements Predicate {
  associationType: AssociationType;
  roleType: RoleType;
  value: any;

  constructor(fields?: Partial<Equals>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    let value: any;
    if (this.roleType.objectType.isUnit) {
      value = this.value;
    } else {
      value = this.value ? this.value.id : undefined;
    }

    return {
      _T: 'Equals',
      at: this.associationType ? this.associationType.id : undefined,
      rt: this.roleType.id ? this.roleType.id : undefined,
      v: this.roleType.objectType.isUnit ? value : undefined,
      o: !this.roleType.objectType.isUnit ? value : undefined,
    };
  }
}
