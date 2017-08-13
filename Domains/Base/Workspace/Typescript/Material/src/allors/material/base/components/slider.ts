import { ChangeDetectorRef, Component , Input } from "@angular/core";
import { ISessionObject } from "../../../../allors/domain";
import { MetaDomain, RoleType } from "../../../../allors/meta";

import { Field } from "../../../angular";

@Component({
  selector: "a-md-slider",
  template: `
<md-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <md-slider fxFlex [(ngModel)]="model" [name]="name" [invert]="invert" [min]="min" [max]="max" [step]="step" [thumbLabel]="thumbLabel" [tickInterval]="tickInterval" [vertical]="vertical" [color]="color" [required]="required" [disabled]="disabled"></md-slider>

  <input fxFlex mdInput [type]="textType"    >
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</md-input-container>
`,
})
export class SliderComponent extends Field {

  @Input()
  public invert: boolean;

  @Input()
  public max: number;

  @Input()
  public min: number;

  @Input()
  public step: number;

  @Input()
  public thumbLabel: boolean;

  @Input()
  public tickInterval: "auto" | number;

  @Input()
  public vertical: boolean;

  @Input()
  public color: "primary" | "accent" | "warn" = "accent";
}
