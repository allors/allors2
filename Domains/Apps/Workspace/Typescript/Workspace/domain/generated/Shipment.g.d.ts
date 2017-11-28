import { SessionObject } from "@allors/framework";
import { Printable } from './Printable.g';
import { Commentable } from './Commentable.g';
import { Auditable } from './Auditable.g';
export interface Shipment extends SessionObject, Printable, Commentable, Auditable {
}
