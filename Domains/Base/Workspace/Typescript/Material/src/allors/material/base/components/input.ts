import { AfterViewInit, ChangeDetectorRef, Component, Input, Optional, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";
import { ISessionObject } from "../../../../allors/domain";
import { MetaDomain, RoleType } from "../../../../allors/meta";

import { Field } from "../../../angular";

@Component({
  selector: "a-md-input",
  template: `
<md-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <input fxFlex mdInput [type]="textType" [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</md-input-container>
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
