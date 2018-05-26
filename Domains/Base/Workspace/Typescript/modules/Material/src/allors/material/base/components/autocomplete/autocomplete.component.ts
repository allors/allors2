import {AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, ViewChild } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';

import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/concat';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

import { Field } from '../../../../angular';
import { ISessionObject } from '../../../../framework';

import { MatAutocompleteTrigger } from '@angular/material';

@Component({
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
      this.filteredOptions = Observable.of(new Array<ISessionObject>()).concat(
        this.searchControl.valueChanges
          .debounceTime(this.debounceTime)
          .distinctUntilChanged()
          .switchMap((search: string) => {
            return this.filter(search);
          }),
      );
    } else {
      this.filteredOptions = Observable.of(new Array<ISessionObject>()).concat(
        this.searchControl.valueChanges
          .debounceTime(this.debounceTime)
          .distinctUntilChanged()
          .map((search: string) => {
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
          }),
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

  public selected(option: ISessionObject): void {
    this.model = option;
    this.onChange.emit(option);

    this.searchControl.reset();
  }

  public focusout(event: any): void {
    if (!this.searchControl.value) {
      this.model = undefined;
      this.onChange.emit(undefined);
    } else {
      if (this.trigger.autocomplete.options.length === 1) {
        const option = this.trigger.autocomplete.options.first.value;
        this.model = option;
        this.searchControl.setValue(this.model);
        this.onChange.emit(option);
      } else {
        if (this.model) {
          this.searchControl.setValue(this.model);
        } else {
          this.searchControl.reset();
        }
      }
    }

    this.searchControl.reset();
  }
}
