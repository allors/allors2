import { OnChanges, SimpleChanges } from "@angular/core";
import { Field } from "@allors/base-angular";
import { Locale } from "@allors/generated";
import { LocalisedTextModel } from "./LocalisedTextModel";
export declare class LocalisedTextComponent extends Field implements OnChanges {
    locales: Locale[];
    models: LocalisedTextModel[];
    ngOnChanges(changes: SimpleChanges): void;
}
