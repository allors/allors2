import { RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class GreaterThan implements Predicate {
  roleType: RoleType;
  value: any;

  constructor(fields?: Partial<GreaterThan>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'GreaterThan',
      rt: this.roleType.id,
      v: this.value,
    };
  }
}
