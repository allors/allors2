import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { Period } from './Period.g';
export interface Agreement extends SessionObject, UniquelyIdentifiable, Period {
}
