import { ChangeDetectorRef, Component, Input } from "@angular/core";
import { Field } from "@baseAngular";
import { ISessionObject } from "@baseDomain";
import { RoleType } from "@baseMeta";

@Component({
  selector: "a-mat-slide-toggle",
  template: `
<div fxLayout="row">
  <mat-slide-toggle [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
  {{label}}
  </mat-slide-toggle>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</div>
`,
})
export class SlideToggleComponent extends Field {

}
