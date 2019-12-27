import { RoleType, ObjectType } from '../meta';
import { UnitTypes, serializeArray } from '../protocol/Serialization';
import { ParametrizedPredicate } from './ParametrizedPredicate';

export class Between extends ParametrizedPredicate {
  public roleType: RoleType;
  public parameter: string;
  public values: UnitTypes[];

  constructor(fields?: Partial<Between>| RoleType) {
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
      kind: 'Between',
      roleType: this.roleType.id,
      parameter: this.parameter,
      values: serializeArray(this.values),
    };
  }
}
