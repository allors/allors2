import { AssociationType, RoleType } from '../../../../meta';
import { ISessionObject } from '../../workspace';
import { Predicate } from './Predicate';
import { Query } from './Query';

export class ContainedIn implements Predicate {
  associationType: AssociationType;
  roleType: RoleType;
  query: Query;
  objects: ISessionObject[];

  constructor(fields?: Partial<ContainedIn>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'ContainedIn',
      at: this.associationType ? this.associationType.id : undefined,
      rt: this.roleType.id ? this.roleType.id : undefined,
      q: this.query,
      o: this.objects ? this.objects.map((v: ISessionObject) => v.id) : undefined,
    };
  }
}
