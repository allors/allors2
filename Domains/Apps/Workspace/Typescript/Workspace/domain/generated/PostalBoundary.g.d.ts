import { SessionObject, Method } from "@allors/framework";
import { Deletable } from './Deletable.g';
import { Country } from './Country.g';
export declare class PostalBoundary extends SessionObject implements Deletable {
    readonly CanReadPostalCode: boolean;
    readonly CanWritePostalCode: boolean;
    PostalCode: string;
    readonly CanReadLocality: boolean;
    readonly CanWriteLocality: boolean;
    Locality: string;
    readonly CanReadCountry: boolean;
    readonly CanWriteCountry: boolean;
    Country: Country;
    readonly CanReadRegion: boolean;
    readonly CanWriteRegion: boolean;
    Region: string;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
