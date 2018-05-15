import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, QueryList, ViewChildren } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/do';

import { ISessionObject } from '../../../../framework';

import { Field } from '../../../../angular';

@Component({
  selector: 'a-td-chips',
  templateUrl: './chips.component.html',
})
export class ChipsComponent extends Field implements OnInit, OnDestroy {

  @Input()
  public display = 'display';

  @Input()
  public debounceTime = 400;

  @Input()
  public options: ISessionObject[];

  @Input()
  public filter: ((search: string) => Observable<ISessionObject[]>);

  @Output()
  public onChange: EventEmitter<Field> = new EventEmitter();

  public filteredOptions: ISessionObject[];

  public subject: Subject<string>;
  public subscription: Subscription;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public ngOnInit(): void {
    this.subject = new Subject<string>();

    if (this.filter) {
      this.subscription = this.subject
        .debounceTime(this.debounceTime)
        .distinctUntilChanged()
        .switchMap((search: string) => {
          return this.filter(search);
        })
        .do((options: ISessionObject[]) => {
          this.filteredOptions = options.filter((v: any) => this.model.indexOf(v) < 0);
        })
        .subscribe();
    } else {
      this.subscription = this.subject
        .debounceTime(this.debounceTime)
        .distinctUntilChanged()
        .map((search: string) => {
          const lowerCaseSearch: string = search.trim().toLowerCase();
          return this.options
            .filter((v: ISessionObject) => {
              const optionDisplay: string = v[this.display] ? v[this.display].toString().toLowerCase() : undefined;
              if (optionDisplay) {
                return optionDisplay.indexOf(lowerCaseSearch) !== -1;
              }
            })
            .sort((a: ISessionObject, b: ISessionObject) => a[this.display] !== b[this.display]
            ? a[this.display] < b[this.display] ? -1 : 1 : 0);
        })
        .do((options: ISessionObject[]) => {
          this.filteredOptions = options;
        })
        .subscribe();
    }
  }

  public ngOnDestroy(): void {
    super.ngOnDestroy();

    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public add(object: ISessionObject): void {
    this.onChange.emit(this);
  }

  public remove(object: ISessionObject): void {
    this.onChange.emit(this);
  }

  public inputChange(search: string): void {
    this.subject.next(search);
  }
}
