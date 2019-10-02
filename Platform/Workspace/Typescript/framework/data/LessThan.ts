import { RoleType } from '../meta';
import { UnitTypes } from '../protocol/Types';
import { ParametrizedPredicate } from './ParametrizedPredicate';

export class LessThan extends ParametrizedPredicate {
  public roleType: RoleType;
  public value: UnitTypes;

  constructor(fields?: Partial<LessThan> | RoleType) {
    super();

    if ((fields as RoleType).objectType) {
      this.roleType = fields as RoleType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'LessThan',
      roleType: this.roleType.id,
      parameter: this.parameter,
      value: this.value,
    };
  }
}
