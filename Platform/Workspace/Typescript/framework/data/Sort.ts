import { RoleType } from '../meta';

export interface SortArgs {
  roleType: RoleType;
  descending?: boolean;
}

export class Sort {
  public roleType: RoleType;
  public descending: boolean;

  constructor(args: SortArgs);
  constructor(roleType: RoleType, descending?: boolean);
  constructor(args: SortArgs | RoleType, descending?: boolean) {
    if (args instanceof RoleType) {
      this.roleType = args;
      this.descending = descending ?? false;
    } else if (args) {
      this.roleType = args.roleType;
      this.descending = args.descending ?? false;
    }
  }

  public toJSON(): any {
    return {
      roleType: this.roleType.id,
      descending: this.descending,
    };
  }
}
