import { SessionObject } from "@allors/base-domain";
import { Locale } from './Locale.g';
export interface Localised extends SessionObject {
    Locale: Locale;
}
