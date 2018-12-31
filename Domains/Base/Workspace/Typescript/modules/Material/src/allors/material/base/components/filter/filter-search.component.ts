import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormControl, AbstractControl } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, map, switchMap, scan, tap } from 'rxjs/operators';

import { ISessionObject } from 'src/allors/framework';
import { ContextService } from 'src/allors/angular';
import { FilterFieldDefinition } from 'src/allors/angular/base/filter/filterFieldDefinition';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'a-mat-filter-search',
    templateUrl: './filter-search.component.html',
})
export class AllorsMaterialFilterSearchComponent implements OnInit {

    @Input() debounceTime = 400;

    @Input()
    parent: FormGroup;

    filteredOptions: Observable<ISessionObject[]>;

    display: (v: ISessionObject) => string;

    constructor(
        public allors: ContextService,
    ) { }

    ngOnInit() {
        this.filteredOptions = this.parent.valueChanges
            .pipe(
                tap((v) => {
                    const definition = v['definition'] as FilterFieldDefinition;
                    this.display = definition.options.display;
                }),
                filter((v) => {
                    const value = v['value'];
                    return value && value.trim && value.toLowerCase;
                }),
                debounceTime(this.debounceTime),
                distinctUntilChanged(),
                switchMap((v) => {

                    const definition = v['definition'] as FilterFieldDefinition;
                    const value = v['value'];

                    return definition.options.search.create(this.allors)(value);
                })
            );
    }
}
