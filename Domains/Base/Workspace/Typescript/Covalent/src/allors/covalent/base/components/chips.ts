import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";
import { Observable, Subject, Subscription } from "rxjs";
import { ISessionObject } from "../../../../allors/domain";
import { MetaDomain, RoleType } from "../../../../allors/meta";

import { Field } from "../../../angular";

@Component({
  selector: "a-td-chips",
  template: `
<td-chips [name]="name"
          [items]="filteredOptions"
          [(ngModel)]="model"
          [placeholder]="label"
          (inputChange)="inputChange($event)"
          (add)="add($event)"
          (remove)="remove($event)"
          requireMatch
          [disabled]="disabled"
          [readOnly]="readonly"
          [required]="required">

  <ng-template td-chip let-chip="chip">
    {{chip[this.display]}}
  </ng-template>
  <ng-template td-autocomplete-option let-option="option">
    <div layout="row" layout-align="start center">
      {{option[this.display]}}
    </div>
  </ng-template>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</td-chips>
`,
})
export class ChipsComponent extends Field implements OnInit, AfterViewInit, OnDestroy {

  @Input()
  public display: string = "display";

  @Input()
  public debounceTime: number = 400;

  @Input()
  public options: ISessionObject[];

  @Input()
  public filter: ((search: string) => Observable<ISessionObject[]>);

  @Output()
  public onAdd: EventEmitter<ISessionObject> = new EventEmitter();

  @Output()
  public onRemove: EventEmitter<ISessionObject> = new EventEmitter();

  public filteredOptions: ISessionObject[];

  public subject: Subject<string>;
  public subscription: Subscription;

  @ViewChildren(NgModel)
  private controls: QueryList<NgModel>;

  constructor(private parentForm: NgForm) {
    super();
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
            .sort((a: ISessionObject, b: ISessionObject) => a[this.display] !== b[this.display] ? a[this.display] < b[this.display] ? -1 : 1 : 0);
        })
        .do((options: ISessionObject[]) => {
          this.filteredOptions = options;
        })
        .subscribe();
    }
  }

  public ngAfterViewInit(): void {
    this.controls.forEach((control: NgModel) => {
      this.parentForm.addControl(control);
    });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public add(object: ISessionObject): void {
    this.onAdd.emit(object);
  }

  public remove(object: ISessionObject): void {
    this.onRemove.emit(object);
  }

  public inputChange(search: string): void {
    this.subject.next(search);
  }
}
