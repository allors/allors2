import { ObjectType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Or implements Predicate {
  predicates: Predicate[];

  constructor(fields?: Partial<Or>) {
    Object.assign(this, fields);
    this.predicates = this.predicates ? this.predicates : [];
   }

  toJSON(): any {
    return {
      _T: 'Or',
      ps: this.predicates,
    };
  }
}
