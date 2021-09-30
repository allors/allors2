namespace Allors.Data {
    export interface SyncResponseObject {
        i: string;
        v: string;
        t: string;

        roles?: any[][];
        methods?: string[][];
    }

    export interface SyncResponse extends Response {
        userSecurityHash: string;
        objects: SyncResponseObject[];
    }
}