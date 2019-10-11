namespace Allors.Protocol {
    export class PushRequest {
        newObjects: PushRequestNewObject[];
        objects: PushRequestObject[];

        constructor(fields?: Partial<PushRequest>) {
          Object.assign(this, fields);
        }
    }

    export class PushRequestNewObject {
      ni: string;
      t: string;
      roles: PushRequestRole[];
    }

    export class PushRequestObject {
      i: string;
      v: string;
      roles: PushRequestRole[];
    }

    export class PushRequestRole {
        t: string;
        s: any;
        a: string[];
        r: string[];
    }
}
