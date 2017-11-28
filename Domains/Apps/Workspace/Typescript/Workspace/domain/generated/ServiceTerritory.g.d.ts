import { SessionObject } from "@allors/framework";
import { GeographicBoundaryComposite } from './GeographicBoundaryComposite.g';
export declare class ServiceTerritory extends SessionObject implements GeographicBoundaryComposite {
    readonly CanReadLatitude: boolean;
    readonly Latitude: number;
    readonly CanReadLongitude: boolean;
    readonly Longitude: number;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
