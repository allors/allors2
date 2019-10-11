namespace Allors.Protocol {
    export class SyncRequest {
        objects: string[];

        constructor(fields?: Partial<SyncRequest>) {
          Object.assign(this, fields);
        }
    }
}
