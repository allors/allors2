import { RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Between implements Predicate {
  roleType: RoleType;
  first: any;
  second: any;

  constructor(fields?: Partial<Between>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'Between',
      rt: this.roleType.id,
      f: this.first,
      s: this.second,
    };
  }
}
