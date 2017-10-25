import { AfterViewInit, ChangeDetectorRef, Component, Input, Optional, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";
import { ISessionObject } from "@baseDomain";
import { RoleType } from "@baseMeta";

import { Field } from "@baseAngular";

@Component({
  selector: "a-mat-input",
  template: `
<mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <input matInput [type]="textType" [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-input-container>
`,
})
export class InputComponent extends Field implements AfterViewInit {
  @ViewChildren(NgModel) private controls: QueryList<NgModel>;

  constructor( @Optional() private parentForm: NgForm) {
    super();
  }

  public ngAfterViewInit(): void {
    if (!!this.parentForm) {
      this.controls.forEach((control: NgModel) => {
        this.parentForm.addControl(control);
      });
    }
  }
}
