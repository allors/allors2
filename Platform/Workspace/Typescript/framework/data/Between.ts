import { RoleType } from '../meta';
import { UnitTypes, serializeArray } from '../protocol/Serialization';
import { Predicate } from './Predicate';

export class Between implements Predicate {
  public roleType: RoleType;
  public param: string;
  public values: UnitTypes[];

  constructor(fields?: Partial<Between>| RoleType) {
    if ((fields as RoleType).objectType) {
      this.roleType = fields as RoleType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'Between',
      roleType: this.roleType.id,
      param: this.param,
      values: serializeArray(this.values),
    };
  }
}
