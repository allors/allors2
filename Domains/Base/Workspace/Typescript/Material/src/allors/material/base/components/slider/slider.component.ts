import { Component , Input } from "@angular/core";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-slider",
  templateUrl: "./slider.component.html",
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
