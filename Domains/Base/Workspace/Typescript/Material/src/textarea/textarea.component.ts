import { AfterViewInit, Component, Optional, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";

import { Field } from "@allors/base-angular";

@Component({
  selector: "a-mat-textarea",
  template: `
<mat-form-field fxLayout="column" fxLayoutAlign="top stretch">
  <textarea matInput [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  </textarea>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-form-field>
`,
})
export class TextareaComponent extends Field implements AfterViewInit {
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
