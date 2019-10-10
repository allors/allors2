namespace Allors.Data {
    export interface SyncResponse extends Response {
        accessControls?: string[][];
        objects: SyncResponseObject[];
    }

    export interface SyncResponseObject {
        i: string;
        t: string;
        v: string;
        a?: string;
        d?: string;
        r?: SyncResponseRole[];
    }

    export interface SyncResponseRole {
        t: string;
        v: string;
    }
}
