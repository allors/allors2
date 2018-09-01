import { RoleType } from '../../meta';
import { Predicate } from './Predicate';

export class Between implements Predicate {
  public roleType: RoleType;
  public values: any[];
 
  constructor(fields?: Partial<Between>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'Between',
      roletype: this.roleType.id,
      values: this.values,
    };
  }
}
