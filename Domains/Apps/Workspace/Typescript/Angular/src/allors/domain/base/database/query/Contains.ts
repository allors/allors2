import { AssociationType, RoleType } from '../../../../meta';
import { ISessionObject } from '../../workspace';
import { Predicate } from './Predicate';
import { Query } from './Query';

export class Contains implements Predicate {
  associationType: AssociationType;
  roleType: RoleType;
  object: ISessionObject;

  constructor(fields?: Partial<Contains>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'Contains',
      at: this.associationType ? this.associationType.id : undefined,
      rt: this.roleType.id ? this.roleType.id : undefined,
      o: this.object ? this.object.id : undefined,
    };
  }
}
