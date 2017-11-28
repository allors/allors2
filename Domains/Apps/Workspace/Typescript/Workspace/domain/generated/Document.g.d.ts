import { SessionObject } from "@allors/framework";
import { Printable } from './Printable.g';
import { Commentable } from './Commentable.g';
export interface Document extends SessionObject, Printable, Commentable {
}
