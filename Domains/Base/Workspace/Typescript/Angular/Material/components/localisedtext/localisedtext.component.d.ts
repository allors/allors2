import { OnChanges, SimpleChanges } from "@angular/core";
import { Locale } from "@allors/workspace";
import { Field } from "@allors/base-angular";
import { LocalisedTextModel } from "./LocalisedTextModel";
export declare class LocalisedTextComponent extends Field implements OnChanges {
    locales: Locale[];
    models: LocalisedTextModel[];
    ngOnChanges(changes: SimpleChanges): void;
}
