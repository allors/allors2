import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, ViewChild } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { concat, debounceTime, distinctUntilChanged, switchMap, map } from 'rxjs/operators';

import { Field } from '../../../../angular';
import { ISessionObject } from '../../../../framework';

import { MatAutocompleteTrigger, MatAutocompleteSelectedEvent } from '@angular/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-autocomplete',
  templateUrl: './autocomplete.component.html',
})
export class AllorsMaterialAutocompleteComponent extends Field implements OnInit {
  @Input() public display = 'display';

  @Input() public debounceTime = 400;

  @Input() public options: ISessionObject[];

  @Input() public filter: ((search: string) => Observable<ISessionObject[]>);

  @Output() public onChange: EventEmitter<ISessionObject> = new EventEmitter();

  public filteredOptions: Observable<ISessionObject[]>;

  public searchControl: FormControl = new FormControl();

  @ViewChild(MatAutocompleteTrigger) private trigger: MatAutocompleteTrigger;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public ngOnInit(): void {
    if (this.filter) {
      this.filteredOptions = of(new Array<ISessionObject>())
        .pipe(
          concat(
            this.searchControl.valueChanges
              .pipe(
                debounceTime(this.debounceTime),
                distinctUntilChanged(),
                switchMap((search: string) => {
                  return this.filter(search);
                }))
          ));
    } else {
      this.filteredOptions = of(new Array<ISessionObject>())
        .pipe(
          concat(
            this.searchControl.valueChanges
              .pipe(
                debounceTime(this.debounceTime),
                distinctUntilChanged(),
                map((search: string) => {
                  const lowerCaseSearch: string = search.trim().toLowerCase();
                  return this.options
                    .filter((v: ISessionObject) => {
                      const optionDisplay: string = v[this.display]
                        ? v[this.display].toString().toLowerCase()
                        : undefined;
                      if (optionDisplay) {
                        return optionDisplay.indexOf(lowerCaseSearch) !== -1;
                      }
                    })
                    .sort(
                      (a: ISessionObject, b: ISessionObject) =>
                        a[this.display] !== b[this.display]
                          ? a[this.display] < b[this.display] ? -1 : 1
                          : 0,
                    );
                })
              )
          )
        );
    }

    this.searchControl.setValue(this.model);
  }

  public displayFn(): (val: ISessionObject) => string {
    return (val: ISessionObject) => {
      if (val) {
        return val ? val[this.display] : '';
      }
    };
  }

  public optionSelected(event: MatAutocompleteSelectedEvent): void {
    this.model = event.option.value;
    this.onChange.emit(this.model);
  }
}
