import { FilterFieldType } from './FilterFieldType';
import { FilterFieldPredicate } from './FilterFieldPredicate';

export interface FilterFieldDefinition {

  name: string;
  predicate: FilterFieldPredicate;
  type: FilterFieldType;
}
