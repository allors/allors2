import { ObjectType } from '../../../../meta';
import { Predicate } from './Predicate';

export class Not implements Predicate {
  predicate: Predicate;

  constructor(fields?: Partial<Not>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      _T: 'Not',
      p: this.predicate,
    };
  }
}
