import { RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Equals implements Predicate {
  roleType: RoleType;
  first: any;
  second: any;

  constructor(fields?: Partial<Equals>) {
    Object.assign(this, fields);
  }

  toJSON() {
    return {
      _T: 'Equals',
      rt: this.roleType.id,
      f: first,
      s: second,
    };
  }
}
