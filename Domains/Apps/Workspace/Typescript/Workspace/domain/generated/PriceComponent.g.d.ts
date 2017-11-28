import { SessionObject } from "@allors/framework";
import { Period } from './Period.g';
import { Commentable } from './Commentable.g';
export interface PriceComponent extends SessionObject, Period, Commentable {
}
