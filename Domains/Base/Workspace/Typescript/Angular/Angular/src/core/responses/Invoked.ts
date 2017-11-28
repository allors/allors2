import { InvokeResponse, ISession, ISessionObject } from "@allors/framework";

export class Invoked {
    constructor(public session: ISession, public response: InvokeResponse) {
    }
}
