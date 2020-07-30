import { Component, EventEmitter, Input, OnInit, Optional, Output, ViewChild, DoCheck } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, map, filter } from 'rxjs/operators';
import { MatAutocompleteTrigger, MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';

import { ISessionObject } from '../../../../../framework';
import { ModelField } from '../../../../../angular/core/forms';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-model-autocomplete',
  templateUrl: './autocomplete.component.html',
})
export class AllorsMaterialModelAutocompleteComponent extends ModelField implements OnInit, DoCheck {
  @Input() display = 'display';

  @Input() debounceTime = 400;

  @Input() options: ISessionObject[];

  @Input() filter: (search: string) => Observable<ISessionObject[]>;

  @Output() changed: EventEmitter<ISessionObject | null> = new EventEmitter();

  filteredOptions: Observable<ISessionObject[]>;

  searchControl: FormControl = new FormControl();


  @ViewChild(MatAutocompleteTrigger) private trigger: MatAutocompleteTrigger;

  private focused = false;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public ngOnInit(): void {
    if (this.filter) {
      this.filteredOptions = this.searchControl.valueChanges.pipe(
        filter((v: any) => v != null && v.trim),
        debounceTime(this.debounceTime),
        distinctUntilChanged(),
        switchMap((search: string) => {
          return this.filter(search);
        }),
      );
    } else {
      this.filteredOptions = this.searchControl.valueChanges.pipe(
        filter((v) => v !== null && v !== undefined && v.trim),
        debounceTime(this.debounceTime),
        distinctUntilChanged(),
        map((search: string) => {
          const lowerCaseSearch = search.trim().toLowerCase();
          return this.options
            .filter((v: ISessionObject) => {
              const optionDisplay: string = (v as any)[this.display] ? (v as any)[this.display].toString().toLowerCase() : undefined;

              return optionDisplay ? optionDisplay.indexOf(lowerCaseSearch) !== -1 : false;
            })
            .sort((a: ISessionObject, b: ISessionObject) =>
              (a as any)[this.display] !== (b as any)[this.display] ? ((a as any)[this.display] < (b as any)[this.display] ? -1 : 1) : 0,
            );
        }),
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
        return val ? (val as any)[this.display] : '';
      }
    };
  }

  optionSelected(event: MatAutocompleteSelectedEvent): void {
    this.model = event.option.value;
    this.changed.emit(this.model);
  }

  clear(event: Event) {
    event.stopPropagation();
    this.model = null;
    this.trigger.closePanel();
    this.changed.emit(this.model);
  }
}
