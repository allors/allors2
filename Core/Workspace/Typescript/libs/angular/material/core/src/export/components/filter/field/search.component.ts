import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

import { ISessionObject } from '@allors/domain/system';
import { FilterFieldDefinition } from '@allors/angular/core';
import { ContextService } from '@allors/angular/services/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-filter-field-search',
  templateUrl: './search.component.html',
})
export class AllorsMaterialFilterFieldSearchComponent implements OnInit {
  @Input() debounceTime = 400;

  @Input()
  parent: FormGroup;

  @Input()
  filterFieldDefinition: FilterFieldDefinition;

  @Output()
  apply: EventEmitter<any> = new EventEmitter();

  filteredOptions: Observable<ISessionObject[]>;

  display: (v: ISessionObject) => string;

  // TODO: Fix this
  private nothingDisplay = () => '';

  constructor(public allors: ContextService) {}

  ngOnInit() {
    this.display = this.filterFieldDefinition.options?.display ?? this.nothingDisplay;

    this.filteredOptions = this.parent.valueChanges.pipe(
      filter((v) => {
        const value = v.value;
        return value && value.trim && value.toLowerCase;
      }),
      debounceTime(this.debounceTime),
      distinctUntilChanged(),
      switchMap((v) => {
        const value = v.value;
        if (!this.filterFieldDefinition?.options) {
          return of([]);
        } else {
          return this.filterFieldDefinition.options.search().create(this.allors)(value);
        }
      }),
    );
  }
}
