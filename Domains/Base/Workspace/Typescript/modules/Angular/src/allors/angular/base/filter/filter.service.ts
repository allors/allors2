import { Injectable } from '@angular/core';
import { FilterFieldDefinition } from './FilterFieldDefinition';
import { FilterField } from './FilterField';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable()
export class AllorsFilterService {
  readonly filterFieldDefinitions: FilterFieldDefinition[] = [];

  readonly filterFields$: Observable<FilterField[]>;

  private readonly filterFieldsSubject: BehaviorSubject<FilterField[]>;

  constructor() {
    this.filterFields$ = this.filterFieldsSubject = new BehaviorSubject([]);
  }

  get filterFields(): FilterField[] {
    return this.filterFieldsSubject.getValue();
  }

  cleaerFilterFields(): any {
    this.filterFieldsSubject.next([]);
  }

  addFilterField(filterField: FilterField) {
    this.filterFieldsSubject.next([...this.filterFields, filterField]);
  }

  removeFilterField(filterField: FilterField): any {
    this.filterFieldsSubject.next(this.filterFields.filter((v) => v !== filterField));
  }

}
