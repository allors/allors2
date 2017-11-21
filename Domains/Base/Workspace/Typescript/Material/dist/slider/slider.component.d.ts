import { Field } from "@allors/base-angular";
export declare class SliderComponent extends Field {
    invert: boolean;
    max: number;
    min: number;
    step: number;
    thumbLabel: boolean;
    tickInterval: "auto" | number;
    vertical: boolean;
    color: "primary" | "accent" | "warn";
}
