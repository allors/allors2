import { AssociationType, RoleType } from "../../../../meta";

export class Path {
    public step: AssociationType | RoleType;

    public next: Path;

    constructor(fields?: Partial<Path>) {
       Object.assign(this, fields);
    }

    public toJSON(): any {
      return {
        next: this.next,
        step: this.step.id,
      };
    }
}
