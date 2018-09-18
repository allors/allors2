import { RoleType } from '../../meta';
import { Predicate } from './Predicate';

export class Between implements Predicate {
  public roleType: RoleType;
  public values: any[];

  constructor(fields?: Partial<Between>| RoleType, ...values: any[]) {
    if ((fields as RoleType).objectType) {
      this.roleType = fields as RoleType;
      this.values = values;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'Between',
      roleType: this.roleType.id,
      values: this.values,
    };
  }
}
