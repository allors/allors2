import { ObjectType } from '../../../meta';
import { Predicate } from './Predicate';

export class Query {
    name: String;

    type: ObjectType;

    predicate: Predicate;

    toJSON() {
      return {
        n: this.name,
        t: this.type.name,
        p: this.predicate,
      };
    }
}
