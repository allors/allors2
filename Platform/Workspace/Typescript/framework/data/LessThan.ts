import { RoleType, ObjectType } from '@allors/meta';
import { ParameterizablePredicate, ParameterizablePredicateArgs } from './ParameterizablePredicate';
import { UnitTypes } from '../workspace/Types';
import { serialize } from '../workspace/SessionObject';

export interface LessThanArgs extends ParameterizablePredicateArgs, Pick<LessThan, 'roleType' | 'value'> {}

export class LessThan extends ParameterizablePredicate {
  public roleType: RoleType;
  public value?: UnitTypes;

  constructor(roleType: RoleType);
  constructor(args: LessThanArgs);
  constructor(args: LessThanArgs | RoleType) {
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
