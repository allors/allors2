export class SyncRequest {
    public objects: string[];

    constructor(fields?: Partial<SyncRequest>) {
      Object.assign(this, fields);
    }

    public toJSON() {
      return {
        objects: this.objects,
      };
    }
}
