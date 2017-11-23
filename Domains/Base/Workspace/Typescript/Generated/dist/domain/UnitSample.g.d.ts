import { SessionObject } from "@allors/base-domain";
export declare class UnitSample extends SessionObject {
    readonly CanReadAllorsBinary: boolean;
    readonly CanWriteAllorsBinary: boolean;
    AllorsBinary: any;
    readonly CanReadAllorsDateTime: boolean;
    readonly CanWriteAllorsDateTime: boolean;
    AllorsDateTime: Date;
    readonly CanReadAllorsBoolean: boolean;
    readonly CanWriteAllorsBoolean: boolean;
    AllorsBoolean: boolean;
    readonly CanReadAllorsDouble: boolean;
    readonly CanWriteAllorsDouble: boolean;
    AllorsDouble: number;
    readonly CanReadAllorsInteger: boolean;
    readonly CanWriteAllorsInteger: boolean;
    AllorsInteger: number;
    readonly CanReadAllorsString: boolean;
    readonly CanWriteAllorsString: boolean;
    AllorsString: string;
    readonly CanReadAllorsUnique: boolean;
    readonly CanWriteAllorsUnique: boolean;
    AllorsUnique: string;
    readonly CanReadAllorsDecimal: boolean;
    readonly CanWriteAllorsDecimal: boolean;
    AllorsDecimal: number;
}
