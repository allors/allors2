import { SessionObject } from "@allors/framework";
import { GeoLocatable } from './GeoLocatable.g';
import { FacilityType } from './FacilityType.g';
export declare class Facility extends SessionObject implements GeoLocatable {
    readonly CanReadFacilityType: boolean;
    readonly CanWriteFacilityType: boolean;
    FacilityType: FacilityType;
    readonly CanReadLatitude: boolean;
    readonly Latitude: number;
    readonly CanReadLongitude: boolean;
    readonly Longitude: number;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
