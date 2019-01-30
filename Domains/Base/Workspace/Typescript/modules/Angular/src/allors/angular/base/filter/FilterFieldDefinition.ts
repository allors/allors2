import { ParametrizedPredicate, Like } from '../../../../allors/framework';
import { FilterOptions } from './FilterOptions';
import { humanize } from '../humanize';

export class FilterFieldDefinition {
  predicate: ParametrizedPredicate;
  options: FilterOptions;

  get isLike() {
    return this.predicate instanceof Like;
  }

  get fieldName(): string {
    return humanize(this.predicate.parameter);
  }

  get criteria(): string {
    if (this.isLike) {
      return 'Starts with';
    }

    return 'Select';
  }

  constructor(fields?: Partial<FilterFieldDefinition>) {
    Object.assign(this, fields);
  }
}

