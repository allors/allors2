import { ObjectType } from '../../../../meta';
import { Predicate } from './Predicate';

export class And implements Predicate {
  predicates: Predicate[];

  constructor(fields?: Partial<And>) {
    Object.assign(this, fields);
    this.predicates = this.predicates ? this.predicates : [];
  }

  toJSON(): any {
    return {
      _T: 'And',
      ps: this.predicates,
    };
  }
}
