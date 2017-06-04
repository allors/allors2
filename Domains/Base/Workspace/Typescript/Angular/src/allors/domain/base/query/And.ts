import { ObjectType } from '../../../meta';
import { Predicate } from './Predicate';

export class And extends Predicate {
  predicates: Predicate[];

  toJSON() {
      return {
        _T: 'And',
        ps: this.predicates,
      };
    }
}
