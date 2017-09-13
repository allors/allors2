import { AfterViewInit, ChangeDetectorRef, Component, Input, Optional, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";
import { ISessionObject } from "../../../../allors/domain";
import { MetaDomain, RoleType } from "../../../../allors/meta";

import { Field } from "../../../angular";

@Component({
  selector: "a-md-select",
  template: `
<div class="mat-input-wrapper">
  <div class="mat-input-flex">
    <div class="mat-input-infix">
      <md-select fxFlex [(ngModel)]="model" [name]="name" [placeholder]="label" [multiple]="roleType.isMany" [required]="required" [disabled]="disabled">
        <md-option *ngFor="let option of options" [value]="option">
          {{option[display]}}
        </md-option>
      </md-select>
    </div>
  </div>
  <div class="mat-input-subscript-wrapper" *ngIf="hint">
    <div class="mat-input-hint-wrapper">
      <md-hint>{{hint}}</md-hint>
    </div>
  </div>
</div>
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
