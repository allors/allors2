import { AfterViewInit, ChangeDetectorRef, Component, Input, Optional, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";
import { ISessionObject } from "../../../../allors/domain";
import { MetaDomain, RoleType } from "../../../../allors/meta";

import { Field } from "../../../angular";

@Component({
  selector: "a-mat-select",
  template: `
<mat-form-field>
    <mat-select [(ngModel)]="model" [name]="name" [placeholder]="label" [multiple]="roleType.isMany" [required]="required" [disabled]="disabled">
      <mat-option *ngFor="let option of options" [value]="option">
        {{option[display]}}
      </mat-option>
    </mat-select>
</mat-form-field>
`,
})
export class SelectComponent extends Field implements AfterViewInit {
  @Input()
  public display: string = "display";

  @Input()
  public options: ISessionObject[];

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
