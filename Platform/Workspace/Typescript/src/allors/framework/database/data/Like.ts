import { RoleType } from '../../meta';

import { Predicate } from './Predicate';

export class Like implements Predicate {
  public roleType: RoleType;
  public value: any;

  constructor(fields?: Partial<Like>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'Like',
      roletype: this.roleType.id,
      value: this.value,
    };
  }
}
