import { ChangeDetectorRef, Component , Input } from "@angular/core";
import { ISessionObject } from "../../../../allors/domain";
import { MetaDomain, RoleType } from "../../../../allors/meta";

import { Field } from "../../../angular";

@Component({
  selector: "a-mat-checkbox",
  template: `
<div fxLayout="row">
  <mat-checkbox fxFlex [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
  {{label}}
  </mat-checkbox>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</div>
`,
})
export class CheckboxComponent extends Field {
}
