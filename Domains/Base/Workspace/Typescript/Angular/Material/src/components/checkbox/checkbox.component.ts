import { Component } from "@angular/core";

import { Field } from "@allors/base-angular";

@Component({
  selector: "a-mat-checkbox",
  template: `
<div fxLayout="row">
  <mat-checkbox [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
  {{label}}
  </mat-checkbox>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</div>
`,
})
export class CheckboxComponent extends Field {
}
