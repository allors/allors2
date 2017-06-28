import { ObjectType, AssociationType, RoleType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Instanceof implements Predicate {
  associationType: AssociationType;
  roleType: RoleType;
  objectType: ObjectType;

  constructor(fields?: Partial<Instanceof>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'Instanceof',
      at: this.associationType ? this.associationType.id : undefined,
      rt: this.roleType.id ? this.roleType.id : undefined,
      ot: this.objectType.id,
    };
  }
}
