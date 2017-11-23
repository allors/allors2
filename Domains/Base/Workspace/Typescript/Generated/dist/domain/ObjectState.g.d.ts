import { SessionObject } from "@allors/base-domain";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
export interface ObjectState extends SessionObject, UniquelyIdentifiable {
    Name: string;
}
