import { Component, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, ViewChild, DoCheck } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, map, filter, tap, startWith } from 'rxjs/operators';

import { RoleField } from '../../../../../angular';
import { ISessionObject } from '../../../../../framework';

import { MatAutocompleteTrigger, MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-autocomplete',
  templateUrl: './autocomplete.component.html',
})
export class AllorsMaterialAutocompleteComponent extends RoleField implements OnInit, DoCheck {
  @Input() display = 'display';

  @Input() debounceTime = 400;

  @Input() options: ISessionObject[];

  @Input() filter: ((search: string) => Observable<ISessionObject[]>);

  @Output() changed: EventEmitter<ISessionObject> = new EventEmitter();

  filteredOptions: Observable<ISessionObject[]>;

  searchControl: FormControl = new FormControl();

  @ViewChild(MatAutocomplete, { static: false }) private autoComplete: MatAutocomplete;

  @ViewChild(MatAutocompleteTrigger, { static: false }) private trigger: MatAutocompleteTrigger;

  private focused = false;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public ngOnInit(): void {
    if (this.filter) {
      this.filteredOptions = this.searchControl.valueChanges
        .pipe(
          filter((v) => v !== null && v !== undefined && v.trim),
          debounceTime(this.debounceTime),
          distinctUntilChanged(),
          switchMap((search: string) => {
            return this.filter(search);
          }))
        ;
    } else {
      this.filteredOptions = this.searchControl.valueChanges
        .pipe(
          filter((v) => v !== null && v !== undefined && v.trim),
          debounceTime(this.debounceTime),
          distinctUntilChanged(),
          map((search: string) => {
            const lowerCaseSearch = search.trim().toLowerCase();
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
        );
    }

  }

  ngDoCheck() {
    if (!this.focused && this.trigger && this.searchControl) {
      if (!this.trigger.panelOpen && this.searchControl.value !== this.model) {
        this.searchControl.setValue(this.model);
      }
    }
  }

  inputBlur() {
    this.focused = false;
  }

  inputFocus() {
    this.focused = true;
    if (!this.model) {
      this.trigger._onChange('');
    }
  }

  displayFn(): (val: ISessionObject) => string {
    return (val: ISessionObject) => {
      if (val) {
        return val ? val[this.display] : '';
      }
    };
  }

  optionSelected(event: MatAutocompleteSelectedEvent): void {
    this.model = event.option.value;
    this.changed.emit(this.model);
  }

  clear(event: Event) {
    event.stopPropagation();
    this.model = undefined;
    this.trigger.closePanel();
    this.changed.emit(this.model);
  }
}
