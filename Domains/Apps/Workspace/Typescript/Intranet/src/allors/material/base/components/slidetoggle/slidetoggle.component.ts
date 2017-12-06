import { Component } from "@angular/core";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-slide-toggle",
  template: `
<div style="width: 100%;" fxLayout="row">
  <mat-slide-toggle [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
  {{label}}
  </mat-slide-toggle>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</div>
`,
})
export class SlideToggleComponent extends Field {

}
