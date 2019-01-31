import { ParametrizedPredicate, Like, Equals } from '../../../../allors/framework';
import { FilterOptions } from './FilterOptions';
import { humanize } from '../humanize';

export class FilterFieldDefinition {
  predicate: ParametrizedPredicate;
  options: FilterOptions;

  get isLike() {
    return this.predicate instanceof Like;
  }

  get isBoolean() {
    if (this.predicate instanceof Equals) {
      const equals = this.predicate as Equals;
      return equals.propertyType.objectType.isBoolean;
    }
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

