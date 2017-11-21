import { ISession, ISessionObject, PushResponse } from "@allors/base-domain";

export class Saved {
    constructor(public session: ISession, public response: PushResponse) {
    }
}
