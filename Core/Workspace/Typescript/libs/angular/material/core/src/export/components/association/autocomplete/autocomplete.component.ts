import { Component, EventEmitter, Input, Optional, Output, ViewChild } from '@angular/core';
import { NgForm, FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, switchMap, map } from 'rxjs/operators';

import { ISessionObject, ParameterTypes } from '@allors/domain/system';
import { AssociationField } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-association-autocomplete',
  templateUrl: './autocomplete.component.html',
})
export class AllorsMaterialAssociationAutoCompleteComponent extends AssociationField {
  @Input() display = 'display';

  @Input() debounceTime = 400;

  @Input() options: ISessionObject[];

  @Input() filter: (search: string, parameters?: { [id: string]: ParameterTypes }) => Observable<ISessionObject[]>;

  @Input() filterParameters: ({ [id: string]: string });

  @Output() changed: EventEmitter<ISessionObject> = new EventEmitter();

  filteredOptions: Observable<ISessionObject[]>;

  searchControl: FormControl = new FormControl();

  @ViewChild(MatAutocompleteTrigger) private trigger: MatAutocompleteTrigger;

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
            if (this.filterParameters) {
              return this.filter(search, this.filterParameters);
            } else {
              return this.filter(search);
            }
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
                const optionDisplay: string = (v as any)[this.display]
                  ? (v as any)[this.display].toString().toLowerCase()
                  : undefined;
                if (optionDisplay) {
                  return optionDisplay.indexOf(lowerCaseSearch) !== -1;
                }

                return false;
              })
              .sort(
                (a: ISessionObject, b: ISessionObject) =>
                  (a as any)[this.display] !== (b as any)[this.display]
                    ? (a as any)[this.display] < (b as any)[this.display] ? -1 : 1
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
    return (val: ISessionObject) => val ? (val as any)[this.display] : '';
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
