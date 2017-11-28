import { SessionObject } from "@allors/framework";
import { Commentable } from './Commentable.g';
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
export interface Payment extends SessionObject, Commentable, UniquelyIdentifiable {
}
