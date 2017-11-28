import { SessionObject } from "@allors/framework";
import { GeographicBoundary } from './GeographicBoundary.g';
export declare class City extends SessionObject implements GeographicBoundary {
    readonly CanReadLatitude: boolean;
    readonly Latitude: number;
    readonly CanReadLongitude: boolean;
    readonly Longitude: number;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
