import { ObjectType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Or implements Predicate {
  predicates: Predicate[];

  constructor(fields?: Partial<Or>) {
    Object.assign(this, fields);
  }

  toJSON() {
    return {
      _T: 'Or',
      ps: this.predicates,
    };
  }
}
