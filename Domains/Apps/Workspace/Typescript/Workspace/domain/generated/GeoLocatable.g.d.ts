import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
export interface GeoLocatable extends SessionObject, UniquelyIdentifiable {
    Latitude: number;
    Longitude: number;
}
