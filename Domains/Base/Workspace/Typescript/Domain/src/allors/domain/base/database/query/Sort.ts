import { RoleType } from "@baseMeta";

export class Sort {

    public roleType: RoleType;
    public direction?: "Asc" | "Desc" = "Asc";

    constructor(fields?: Partial<Sort>) {
       Object.assign(this, fields);
    }

    public toJSON(): any {
      return {
        d: this.direction,
        rt: this.roleType.id,
      };
    }
}
