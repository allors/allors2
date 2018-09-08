import { RoleType } from '../../meta';

export class Sort {

    public roleType: RoleType;
    public descending = false;

    constructor(fields?: Partial<Sort>) {
      Object.assign(this, fields);
    }
    
    public toJSON(): any {
      return {
        roletype: this.roleType.id,
        descending: this.descending,
      };
    }
}
