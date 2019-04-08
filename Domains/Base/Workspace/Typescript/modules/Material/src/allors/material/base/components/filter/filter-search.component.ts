import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, AbstractControl } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, map, switchMap, scan, tap } from 'rxjs/operators';

import { ISessionObject } from '../../../../../allors/framework';
import { ContextService } from '../../../../../allors/angular';
import { FilterFieldDefinition } from '../../../../../allors/angular/base/filter/filterFieldDefinition';

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

    display: (v: ISessionObject) => string;

    constructor(
        public allors: ContextService,
    ) { }

    ngOnInit() {
        this.display = this.filterFieldDefinition.options.display;

        this.filteredOptions = this.parent.valueChanges
            .pipe(
                filter((v) => {
                    const value = v['value'];
                    return value && value.trim && value.toLowerCase;
                }),
                debounceTime(this.debounceTime),
                distinctUntilChanged(),
                switchMap((v) => {

                    const value = v['value'];
                    return this.filterFieldDefinition.options.search.create(this.allors)(value);
                })
            );
    }
}
