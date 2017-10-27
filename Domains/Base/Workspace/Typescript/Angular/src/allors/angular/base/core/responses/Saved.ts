import { ISession, ISessionObject, PushResponse } from "@baseDomain";

export class Saved {
    constructor(public session: ISession, public response: PushResponse) {
    }
}
