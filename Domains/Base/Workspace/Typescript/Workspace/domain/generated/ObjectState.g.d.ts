import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
export interface ObjectState extends SessionObject, UniquelyIdentifiable {
    Name: string;
}
