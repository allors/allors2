namespace Allors.Data {

    export interface SecurityResponse extends Response {
        accessControls?: SecurityResponseAccessControl[];
        permissions?: string[][];
    }

    export interface SecurityResponseAccessControl {
        i: string;
        v: string;
        p: string[];
    }
}
