import { ObjectType } from '../../../../meta';
import { Predicate } from './Predicate';

export class And implements Predicate {
  predicates: Predicate[];

  constructor(fields?: Partial<And>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'And',
      ps: this.predicates,
    };
  }
}
