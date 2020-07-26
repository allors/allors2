import { RoleType, ObjectType } from '../meta';
import { ParameterizablePredicate, ParameterizablePredicateArgs } from './ParameterizablePredicate';
import { GreaterThanArgs } from './GreaterThan';

export interface LikeArgs extends ParameterizablePredicateArgs, Pick<Like, 'roleType' | 'value'> {}

export class Like extends ParameterizablePredicate {
  public roleType: RoleType;
  public value?: string;

  constructor(roleType: RoleType);
  constructor(args: GreaterThanArgs);
  constructor(args: GreaterThanArgs | RoleType) {
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

  public toJSON(): any {
    return {
      kind: 'Like',
      dependencies: this.dependencies,
      roleType: this.roleType.id,
      parameter: this.parameter,
      value: this.value,
    };
  }
}
