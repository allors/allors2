import { FilterType } from './FilterType';
import { FilterPredicate } from './FilterPredicate';

export interface FilterDefinition {

  name: string;
  predicate: FilterPredicate;
  type: FilterType;
}
