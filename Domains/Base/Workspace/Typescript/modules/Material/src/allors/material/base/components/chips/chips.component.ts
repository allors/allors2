import {
  Component, EventEmitter, Input, OnDestroy, OnInit, Optional, Output,
  ViewChild, ElementRef
} from '@angular/core';
import { NgForm, FormControl } from '@angular/forms';

import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/do';

import { ISessionObject } from '../../../../framework';

import { Field } from '../../../../angular';
import { MatAutocompleteTrigger } from '@angular/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-chips',
  templateUrl: './chips.component.html',
})
export class AllorsMaterialChipsComponent extends Field implements OnInit, OnDestroy {

  @Input() public display = 'display';

  @Input() public debounceTime = 400;

  @Input() public options: ISessionObject[];

  @Input() public filter: ((search: string) => Observable<ISessionObject[]>);

  @Output() public onChange: EventEmitter<ISessionObject> = new EventEmitter();

  public filteredOptions: Observable<ISessionObject[]>;

  public searchControl: FormControl = new FormControl();
  @ViewChild('searchInput') searchInput: ElementRef;

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
            if (search) {
              const lowerCaseSearch: string = search.trim ? search.trim().toLowerCase() : '';
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
            }
          }),
      );
    }
  }

  public displayFn(): (val: ISessionObject) => string {
    return (val: ISessionObject) => {
      if (val) {
        return val ? val[this.display] : '';
      }
    };
  }

  public selected(option: ISessionObject): void {
    this.add(option);
    this.onChange.emit(option);

    this.searchControl.reset();
    this.searchInput.nativeElement.value = '';
  }

  public focusout(): void {
    if (this.searchControl.value && this.trigger.autocomplete.options.length === 1) {
      const option = this.trigger.autocomplete.options.first.value;
      this.add(option);
      this.onChange.emit(option);

    } else {
      this.onChange.emit(undefined);
    }

    this.searchControl.reset();
    this.searchInput.nativeElement.value = '';
  }
}
