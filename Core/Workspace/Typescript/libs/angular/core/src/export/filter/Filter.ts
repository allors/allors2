import { Observable, BehaviorSubject } from 'rxjs';

import { FilterField } from './FilterField';
import { FilterDefinition } from './FilterDefinition';

export class Filter {
  readonly fields$: Observable<FilterField[]>;
  private readonly fieldsSubject: BehaviorSubject<FilterField[]>;

  constructor(public definition: FilterDefinition) {
    this.fields$ = this.fieldsSubject = new BehaviorSubject([]);
  }

  get fields(): FilterField[] {
    return this.fieldsSubject.getValue();
  }

  clearFields(): any {
    this.fieldsSubject.next([]);
  }

  addField(field: FilterField) {
    this.fieldsSubject.next([...this.fields, field]);
  }

  removeField(field: FilterField): any {
    this.fieldsSubject.next(this.fields.filter((v) => v !== field));
  }

  parameters(fields: FilterField[]): any {
    return fields.reduce((acc, cur) => {
      if (cur.definition.predicate.parameter) {
        acc[cur.definition.predicate.parameter] = cur.argument;
      }
      return acc;
    }, {} as { [key: string]: any });
  }
}
