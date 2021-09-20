import { Component, EventEmitter, Input, OnInit, Optional, Output, ViewChild, ElementRef, DoCheck, NgZone } from '@angular/core';
import { NgForm, FormControl } from '@angular/forms';
import { Observable, BehaviorSubject, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, map, filter } from 'rxjs/operators';
import { MatAutocompleteTrigger, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

import { RoleField } from '@allors/angular/core';
import { ISessionObject, ParameterTypes } from '@allors/domain/system';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-chips',
  templateUrl: './chips.component.html',
})
export class AllorsMaterialChipsComponent extends RoleField implements OnInit, DoCheck {

  @Input() display = 'display';

  @Input() debounceTime = 400;

  @Input() options: ISessionObject[];

  @Input() filter: (search: string, parameters?: { [id: string]: ParameterTypes }) => Observable<ISessionObject[]>;

  @Input() filterParameters: ({ [id: string]: ParameterTypes });

  @Output() changed: EventEmitter<ISessionObject> = new EventEmitter();

  filteredOptions: Observable<ISessionObject[]>;

  searchControl: FormControl = new FormControl();

  @ViewChild('searchInput') searchInput: ElementRef;

  @ViewChild(MatAutocompleteTrigger) private trigger: MatAutocompleteTrigger;

  private focused = false;

  focus$: BehaviorSubject<Date>;

  constructor(
    @Optional() parentForm: NgForm  ) {
    super(parentForm);

    this.focus$ = new BehaviorSubject<Date>(new Date());
  }

  public ngOnInit(): void {
    if (this.filter) {
      this.filteredOptions = combineLatest([this.searchControl.valueChanges, this.focus$])
        .pipe(
          filter(([search]) => search !== null && search !== undefined && search.trim),
          debounceTime(this.debounceTime),
          distinctUntilChanged(),
          switchMap(([search]) => {
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
    this.trigger._onChange('');
    this.focus$.next(new Date());
  }

  displayFn(): (val: ISessionObject) => string {
    return (val: ISessionObject) => {
      if (val) {
        return val ? (val as any)[this.display] : '';
      }
    };
  }

  optionSelected(event: MatAutocompleteSelectedEvent): void {
    this.add(event.option.value);
    this.changed.emit(this.model);

    this.searchControl.reset();
    this.searchInput.nativeElement.value = '';
  }

  clear(event: Event) {
    event.stopPropagation();
    this.model = undefined;
    this.trigger.closePanel();
    this.changed.emit(this.model);
  }
}
