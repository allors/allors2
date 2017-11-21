import { Field } from "@allors/base-angular";
export interface RadioGroupOption {
    label?: string;
    value: any;
}
export declare class RadioGroupComponent extends Field {
    options: RadioGroupOption[];
    readonly keys: string[];
    optionLabel(option: RadioGroupOption): string;
    optionValue(option: RadioGroupOption): any;
}
