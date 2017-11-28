import { SessionObject } from "@allors/framework";
import { Commentable } from './Commentable.g';
import { Period } from './Period.g';
export interface ProductAssociation extends SessionObject, Commentable, Period {
}
