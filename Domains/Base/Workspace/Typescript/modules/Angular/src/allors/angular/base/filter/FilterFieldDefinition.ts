import { ParametrizedPredicate } from '../../../../allors/framework';
import { SearchFactory } from '../data';

export class FilterFieldDefinition {
  predicate: ParametrizedPredicate;
  search: SearchFactory;

  constructor(fields?: Partial<FilterFieldDefinition>) {
    Object.assign(this, fields);
  }
}

