import { RoleType } from '../../meta';

import { Predicate } from './Predicate';

export class GreaterThan implements Predicate {
  public roleType: RoleType;
  public value: string | Date | boolean | number;

  constructor(fields?: Partial<GreaterThan> | RoleType, value?: string | Date | boolean | number) {
    if ((fields as RoleType).objectType) {
      this.roleType = fields as RoleType;
      this.value = value;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    return {
      kind: 'GreaterThan',
      roleType: this.roleType.id,
      value: this.value,
    };
  }
}
