import { ISession, ISessionObject, PushResponse } from "@allors/framework";

export class Saved {
    constructor(public session: ISession, public response: PushResponse) {
    }
}
