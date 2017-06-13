import { RoleType } from '../../../../meta';

export class Sort {

    roleType: RoleType;
    direction?: 'Asc' | 'Desc' = 'Asc';

    constructor(fields?: Partial<Sort>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        rt: this.roleType.id,
        d: this.direction,
      };
    }
}
