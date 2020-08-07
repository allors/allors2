import { Predicate, And, Or, Not, ContainedIn, Extent, IExtent, ParameterizablePredicate } from '@allors/data/system';

import { FilterOptions } from './FilterOptions';
import { FilterFieldDefinition } from './FilterFieldDefinition';

function parametrize(predicate: Predicate | IExtent, results: ParameterizablePredicate[] = []): ParameterizablePredicate[] {
  if (predicate instanceof Extent) {
    if (predicate.predicate) {
      parametrize(predicate.predicate, results);
    }
  }

  if (predicate instanceof And || predicate instanceof Or) {
    if (predicate.operands) {
      predicate.operands.forEach((v) => parametrize(v, results));
    }
  }

  if (predicate instanceof Not) {
    if (predicate.operand) {
      parametrize(predicate.operand, results);
    }
  }

  if (predicate instanceof ContainedIn) {
    if (predicate.extent) {
      parametrize(predicate.extent, results);
    }
  }

  if (predicate instanceof ParameterizablePredicate) {
    if ((predicate as ParameterizablePredicate).parameter) {
      results.push(predicate);
    }
  }

  return results;
}

export class FilterDefinition {
  fieldDefinitions: FilterFieldDefinition[];

  constructor(public predicate: Predicate, options?: { [parameter: string]: Partial<FilterOptions> }) {
    const predicates = parametrize(predicate);
    this.fieldDefinitions = predicates.map(
      (v) =>
        new FilterFieldDefinition({
          predicate: v,
          options: options && v.parameter ? new FilterOptions(options[v.parameter]) : undefined,
        }),
    );
  }
}
