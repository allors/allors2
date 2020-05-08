import { RoleType, ObjectType } from '../meta';
import { ParametrizedPredicate } from './ParametrizedPredicate';
import { UnitTypes } from '../workspace/Types';
import { serialize } from '../workspace/SessionObject';

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

  get objectType(): ObjectType {
    return this.roleType.objectType;
  }

  public toJSON(): any {
    return {
      kind: 'LessThan',
      dependencies: this.dependencies,
      roleType: this.roleType.id,
      parameter: this.parameter,
      value: serialize(this.value),
    };
  }
}
