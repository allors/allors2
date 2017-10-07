import { ChangeDetectorRef, Component, Input } from "@angular/core";
import { ISessionObject } from "../../../../allors/domain";
import { MetaDomain, RoleType } from "../../../../allors/meta";

import { Field } from "../../../angular";

@Component({
  selector: "a-mat-slide-toggle",
  template: `
<div fxLayout="row">
  <mat-slide-toggle fxFlex [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
  {{label}}
  </mat-slide-toggle>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</div>
`,
})
export class SlideToggleComponent extends Field {

}
