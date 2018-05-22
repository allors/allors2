import { RoleType } from '../../meta';
import { Predicate } from './Predicate';

export class Between implements Predicate {
  public roleType: RoleType;
  public first: any;
  public second: any;

  constructor(fields?: Partial<Between>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: 'Between',
      f: this.first,
      rt: this.roleType.id,
      s: this.second,
    };
  }
}
