import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

import { FilterFieldDefinition } from './filterFieldDefinition';
import { Predicate, And, Or, Not, ContainedIn, Filter } from '../../../framework';
import { FilterField } from './FilterField';
import { ParametrizedPredicate } from '../../../framework/database/data/ParametrizedPredicate';

import { FilterOptions } from './FilterOptions';
import { Options } from 'selenium-webdriver/safari';

function getParameterizedPredicates(predicate: Predicate, results: ParametrizedPredicate[] = []): ParametrizedPredicate[] {

  if (predicate instanceof Filter) {
    if (predicate.predicate) {
      getParameterizedPredicates(predicate.predicate, results);
    }
  }

  if (predicate instanceof And || predicate instanceof Or) {
    if (predicate.operands) {
      predicate.operands.forEach((v) => getParameterizedPredicates(v, results));
    }
  }

  if (predicate instanceof Not) {
    if (predicate.operand) {
      getParameterizedPredicates(predicate.operand, results);
    }
  }

  if (predicate instanceof ContainedIn) {
    if (predicate.extent) {
      getParameterizedPredicates(predicate.extent, results);
    }
  }

  if (predicate instanceof ParametrizedPredicate) {
    if ((predicate as ParametrizedPredicate).parameter) {
      results.push(predicate);
    }
  }

  return results;
}


@Injectable()
export class AllorsFilterService {

  filterFieldDefinitions: FilterFieldDefinition[];

  readonly filterFields$: Observable<FilterField[]>;
  private readonly filterFieldsSubject: BehaviorSubject<FilterField[]>;

  constructor() {
    this.filterFields$ = this.filterFieldsSubject = new BehaviorSubject([]);
  }

  get filterFields(): FilterField[] {
    return this.filterFieldsSubject.getValue();
  }

  clearFilterFields(): any {
    this.filterFieldsSubject.next([]);
  }

  addFilterField(filterField: FilterField) {
    this.filterFieldsSubject.next([...this.filterFields, filterField]);
  }

  removeFilterField(filterField: FilterField): any {
    this.filterFieldsSubject.next(this.filterFields.filter((v) => v !== filterField));
  }

  init(predicate: Predicate, options: { [parameter: string]: Partial<FilterOptions> } = null) {
    const predicates = getParameterizedPredicates(predicate);
    this.filterFieldDefinitions = predicates.map((v) => new FilterFieldDefinition({ predicate: v, options: new FilterOptions(options && options[v.parameter]) }));
  }

  arguments(filterFields: FilterField[]): any {
    return filterFields.reduce((acc, cur) => {
      acc[cur.definition.predicate.parameter] = (cur.value2 !== undefined && cur.value2 !== null) ? [cur.value, cur.value2] : cur.value;
      return acc;
    }, {});
  }

}
