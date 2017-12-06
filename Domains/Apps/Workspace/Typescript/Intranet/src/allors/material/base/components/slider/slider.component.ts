import { Component , Input } from "@angular/core";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-slider",
  template: `
<mat-form-field style="width: 100%;" fxLayout="column" fxLayoutAlign="top stretch">
  <mat-slider [(ngModel)]="model" [name]="name" [invert]="invert" [min]="min" [max]="max" [step]="step" [thumbLabel]="thumbLabel" [tickInterval]="tickInterval" [vertical]="vertical" [color]="color" [required]="required" [disabled]="disabled"></mat-slider>
  <input matInput [type]="textType"    >
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-form-field>
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
