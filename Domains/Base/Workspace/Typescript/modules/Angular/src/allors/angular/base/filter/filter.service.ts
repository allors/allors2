import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

import { Predicate } from '../../../framework';
import { FilterField } from './FilterField';
import { parametrizedPredicates, ParametrizedPredicate } from '../../../framework/database/data/ParametrizedPredicate';

@Injectable()
export class AllorsFilterService {
  readonly filterFieldPredicates: ParametrizedPredicate[] = [];

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

  init(predicate: Predicate) {
    parametrizedPredicates(predicate).forEach((v) => {
      this.filterFieldPredicates.push(v);
    });
  }

  arguments(filterFields: FilterField[]): any {
    return filterFields.reduce((acc, cur) => {
      acc[cur.predicate.parameter] = (cur.value2 !== undefined && cur.value2 !== null) ? [cur.value, cur.value2] : cur.value;
      return acc;
    }, {});
  }

}
