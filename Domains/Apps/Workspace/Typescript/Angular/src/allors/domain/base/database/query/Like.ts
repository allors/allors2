import { RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Like implements Predicate {
  roleType: RoleType;
  value: any;

  constructor(fields?: Partial<Like>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'Like',
      rt: this.roleType.id,
      v: this.value,
    };
  }
}
