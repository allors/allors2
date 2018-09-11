import { RoleType } from '../../meta';

export class Sort {

  public roleType: RoleType;
  public descending = false;

  constructor(fields?: Partial<Sort> | RoleType) {
    if ((fields as RoleType).id) {
      this.roleType = fields as RoleType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      roletype: this.roleType.id,
      descending: this.descending,
    };
  }
}
