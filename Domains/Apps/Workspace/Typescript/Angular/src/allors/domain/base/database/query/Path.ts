import { AssociationType, RoleType } from '../../../../meta';

export class Path {
    step: AssociationType | RoleType;

    next: Path;

    constructor(fields?: Partial<Path>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        step: this.step.id,
        next: this.next,
      };
    }
}
