import { RoleType } from '../meta';
import { UnitTypes, serialize } from '../protocol/Serialization';
import { ParametrizedPredicate } from './ParametrizedPredicate';

export class GreaterThan extends ParametrizedPredicate {
  public roleType: RoleType;
  public value: UnitTypes;

  constructor(fields?: Partial<GreaterThan> | RoleType) {
    super();

    if ((fields as RoleType).objectType) {
      this.roleType = fields as RoleType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'GreaterThan',
      roleType: this.roleType.id,
      parameter: this.parameter,
      value: serialize(this.value),
    };
  }
}
