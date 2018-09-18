import { RoleType } from '../../meta';

import { Predicate } from './Predicate';

export class LessThan implements Predicate {
  public roleType: RoleType;
  public value: string | Date | boolean | number;

  constructor(fields?: Partial<LessThan> | RoleType, value?: string | Date | boolean | number) {
    if ((fields as RoleType).objectType) {
      this.roleType = fields as RoleType;
      this.value = value;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'LessThan',
      roleType: this.roleType.id,
      value: this.value,
    };
  }
}
