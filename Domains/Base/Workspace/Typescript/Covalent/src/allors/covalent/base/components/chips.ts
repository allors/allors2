import { Component, Input, Output, OnInit, OnDestroy, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';
import { Observable, Subscription, Subject } from 'rxjs';

import { Field } from '../../../angular';

@Component({
  selector: 'a-td-chips',
  template: `
<td-chips
          [items]="filteredOptions"
          [(ngModel)]="model"
          [placeholder]="label"
          (inputChange)="inputChange($event)"
          (add)="add($event)"
          (remove)="remove($event)"
          requireMatch
          [disabled]="disabled"
          [readOnly]="readonly"
          >

  <ng-template td-chip let-chip="chip">
    {{chip[this.display]}}
  </ng-template>
  <ng-template td-autocomplete-option let-option="option">
    <div layout="row" layout-align="start center">
      {{option[this.display]}}
    </div>
  </ng-template>
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</td-chips>
`,
})
export class ChipsComponent extends Field implements OnInit, OnDestroy {

  @Input()
  display: string = 'display';

  @Input()
  debounceTime: number = 200;

  @Input()
  options: ISessionObject[];

  @Input()
  filter: ((search: string) => Observable<ISessionObject[]>);

  @Output()
  onAdd: EventEmitter<ISessionObject> = new EventEmitter();

  @Output()
  onRemove: EventEmitter<ISessionObject> = new EventEmitter();

  filteredOptions: ISessionObject[];

  subject: Subject<string>;
  subscription: Subscription;

  searchControl: FormControl = new FormControl();

  ngOnInit(): void {
    this.subject = new Subject<string>();

    if (this.filter) {
      this.subscription = this.subject
        .debounceTime(this.debounceTime)
        .distinctUntilChanged()
        .mergeMap((search: string) => {
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
            .sort((a: ISessionObject, b: ISessionObject) => a[this.display] !== b[this.display] ? a[this.display] < b[this.display] ? -1 : 1 : 0);
        })
        .do((options: ISessionObject[]) => {
          this.filteredOptions = options;
        })
        .subscribe();
    }
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  add(object: ISessionObject): void {
    this.onAdd.emit(object);
  }

  remove(object: ISessionObject): void {
    this.onRemove.emit(object);
  }

  inputChange(search: string): void {
    this.subject.next(search);
  }
}
