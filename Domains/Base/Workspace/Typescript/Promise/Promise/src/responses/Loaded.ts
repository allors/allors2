import { ISession, ISessionObject, PullResponse } from "@allors/base-domain";

export class Loaded {

    public objects: { [name: string]: ISessionObject; } = {};
    public collections: { [name: string]: ISessionObject[]; } = {};
    public values: { [name: string]: any; } = {};

    constructor(public session: ISession, public response: PullResponse) {
        const namedObjects: { [id: string]: string; } = response.namedObjects;
        const namedCollections: { [id: string]: string[]; } = response.namedCollections;
        const namedValues: { [id: string]: any; } = response.namedValues;

        Object.keys(namedObjects).map((key: string) => this.objects[key] = session.get(namedObjects[key]));
        Object.keys(namedCollections).map((key: string) => this.collections[key] = namedCollections[key].map((id: string) => session.get(id)));
        Object.keys(namedValues).map((key: string) => this.values[key] = namedValues[key]);
    }
}
