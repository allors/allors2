import { AfterViewInit, ChangeDetectorRef, Component, Input, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";

import { Field } from "@baseAngular/core";
import { ISessionObject } from "@baseDomain";
import { RoleType } from "@baseMeta";

@Component({
  selector: "a-mat-datepicker",
  template: `
<div fxLayout="row">
  <mat-input-container fxLayoutGap="1em">
    <input matInput [matDatepicker]="picker" [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
    <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
    <mat-datepicker #picker></mat-datepicker>
    <mat-hint *ngIf="hint">{{hint}}</mat-hint>
  </mat-input-container>

  <mat-input-container *ngIf="this.model && useTime">
    <input matInput matInput type="number" min="0" max="23" [(ngModel)]="hours" [name]="hours" placeholder="hours" [required]="required" [disabled]="disabled" [readonly]="readonly">
  </mat-input-container>

  <mat-input-container *ngIf="this.model && useTime">
    <input matInput matInput type="number" min="0" max="59" [(ngModel)]="minutes" [name]="minutes" placeholder="minutes" [required]="required" [disabled]="disabled" [readonly]="readonly">
  </mat-input-container>
</div>
<mat-datepicker #picker></mat-datepicker>
`,
})
export class DatepickerComponent extends Field implements AfterViewInit {

  @Input()
  public useTime: boolean;

  @ViewChildren(NgModel)
  public controls: QueryList<NgModel>;

  constructor(private parentForm: NgForm) {
    super();
  }

  public ngAfterViewInit(): void {
    this.controls.forEach((control: NgModel) => {
      this.parentForm.addControl(control);
    });
  }

  get hours(): number {

    if (this.model) {
      return this.model.getHours();
    }
  }

  set hours(value: number) {
    if (this.model) {
      this.model.setHours(value);
    }
  }

  get minutes(): number {

    if (this.model) {
      return this.model.getMinutes();
    }
  }

  set minutes(value: number) {
    if (this.model) {
      this.model.setMinutes(value);
    }
  }
}
