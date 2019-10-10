export class SyncRequest {
    public objects: string[];

    constructor(fields?: Partial<SyncRequest>) {
      Object.assign(this, fields);
    }
}
