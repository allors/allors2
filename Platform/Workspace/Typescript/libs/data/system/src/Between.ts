import { ObjectType, RoleType } from '@allors/meta/system';
import { UnitTypes, serializeAllDefined } from '@allors/domain/system';

import { ParameterizablePredicateArgs, ParameterizablePredicate } from './ParameterizablePredicate';

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
    } else {
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
