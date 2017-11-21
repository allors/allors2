import { Locale, LocalisedText } from "@allors/generated";
import { LocalisedTextComponent } from "./localisedtext.component";
export declare class LocalisedTextModel {
    component: LocalisedTextComponent;
    locale: Locale;
    constructor(component: LocalisedTextComponent, locale: Locale);
    readonly object: LocalisedText;
    text: string;
    readonly name: string;
    readonly label: string;
}
