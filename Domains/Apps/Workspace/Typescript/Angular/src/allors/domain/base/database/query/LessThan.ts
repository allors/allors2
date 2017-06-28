import { RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class LessThan implements Predicate {
  roleType: RoleType;
  value: any;

  constructor(fields?: Partial<LessThan>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'LessThan',
      rt: this.roleType.id,
      v: this.value,
    };
  }
}
