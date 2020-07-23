import { RoleType, ObjectType } from '../meta';

import { ParameterizablePredicate } from './ParameterizablePredicate';

export class Like extends ParameterizablePredicate {
  public roleType: RoleType;
  public value: string;

  constructor(fields?: Partial<Like> | RoleType) {
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
      kind: 'Like',
      dependencies: this.dependencies,
      roleType: this.roleType.id,
      parameter: this.parameter,
      value: this.value,
    };
  }
}
