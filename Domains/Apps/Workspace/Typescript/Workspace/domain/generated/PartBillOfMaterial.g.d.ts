import { SessionObject } from "@allors/framework";
import { Commentable } from './Commentable.g';
import { Period } from './Period.g';
export interface PartBillOfMaterial extends SessionObject, Commentable, Period {
}
