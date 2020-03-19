import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

import { ISessionObject, assert } from '../../../../../allors/framework';
import { ContextService } from '../../../../../allors/angular';
import { FilterFieldDefinition } from '../../../../../allors/angular/core/filter/FilterFieldDefinition';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-filter-search',
  templateUrl: './filter-search.component.html',
})
export class AllorsMaterialFilterSearchComponent implements OnInit {

  @Input() debounceTime = 400;

  @Input()
  parent: FormGroup;

  @Input()
  filterFieldDefinition: FilterFieldDefinition;

  @Output()
  apply: EventEmitter<any> = new EventEmitter();

  filteredOptions: Observable<ISessionObject[]>;

  display: ((v: ISessionObject) => string);

  //TODO: Fix this
  private nothingDisplay = () => '';

  constructor(
    public allors: ContextService,
  ) { }

  ngOnInit() {
    this.display = this.filterFieldDefinition.options?.display ?? this.nothingDisplay;

    this.filteredOptions = this.parent.valueChanges
      .pipe(
        filter((v) => {
          const value = v.value;
          return value && value.trim && value.toLowerCase;
        }),
        debounceTime(this.debounceTime),
        distinctUntilChanged(),
        switchMap((v) => {

          const value = v.value;

          // TODO: ?????
          assert(this.filterFieldDefinition.options);

          return this.filterFieldDefinition.options.search.create(this.allors)(value);
        })
      );
  }
}
