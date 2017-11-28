import { SessionObject } from "@allors/framework";
import { Locale } from './Locale.g';
export interface Localised extends SessionObject {
    Locale: Locale;
}
