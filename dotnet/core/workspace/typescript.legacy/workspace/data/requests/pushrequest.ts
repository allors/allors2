namespace Allors.Data {
    export class PushRequestRole {
        t: string;
        s: any;
        a: string[];
        r: string[];
    }

    export class PushRequestObject {
        i: string;
        v: string;
        roles: PushRequestRole[];
    }

    export class PushRequestNewObject {
        ni: string;
        t: string;
        roles: PushRequestRole[];
    }

    export class PushRequest {
        newObjects: PushRequestNewObject[];
        objects: PushRequestObject[];
    }
}