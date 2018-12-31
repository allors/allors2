import { ParametrizedPredicate } from '../../../../allors/framework';
import { FilterOptions } from './FilterOptions';

export class FilterFieldDefinition {
  predicate: ParametrizedPredicate;
  options: FilterOptions;

  constructor(fields?: Partial<FilterFieldDefinition>) {
    Object.assign(this, fields);
  }
}

