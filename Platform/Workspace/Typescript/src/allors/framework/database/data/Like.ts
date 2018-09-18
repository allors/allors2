import { RoleType } from '../../meta';

import { Predicate } from './Predicate';

export class Like implements Predicate {
  public roleType: RoleType;
  public value: any;

  constructor(fields?: Partial<Like> | RoleType, value?: string) {
    if ((fields as RoleType).objectType) {
      this.roleType = fields as RoleType;
      this.value = value;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'Like',
      roleType: this.roleType.id,
      value: this.value,
    };
  }
}
