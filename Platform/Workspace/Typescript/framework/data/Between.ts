import { RoleType, ObjectType } from '../meta';
import { ParameterizablePredicate, ParameterizablePredicateArgs } from './ParameterizablePredicate';
import { UnitTypes } from '../workspace';
import { serialize, serializeAllDefined } from '../workspace/SessionObject';

export interface BetweenArgs extends ParameterizablePredicateArgs, Pick<Between, 'roleType' | 'values'> {}

export class Between extends ParameterizablePredicate {
  roleType: RoleType;
  values?: UnitTypes[];

  constructor(roleType: RoleType);
  constructor(args: BetweenArgs);
  constructor(args: BetweenArgs | RoleType) {
    super();

    if (args instanceof RoleType) {
      this.roleType = args;
    } else if (args) {
      Object.assign(this, args);
      this.roleType = args.roleType;
    }
  }

  get objectType(): ObjectType {
    return this.roleType.objectType;
  }

  toJSON(): any {

    return {
      kind: 'Between',
      dependencies: this.dependencies,
      roleType: this.roleType.id,
      parameter: this.parameter,
      values: this.values ? serialize(this.values) : undefined,
    };

    function serialize(values: UnitTypes[]): string[] | undefined {
      return values.map((v) => serializeAllDefined(v));
    }
  }
}
