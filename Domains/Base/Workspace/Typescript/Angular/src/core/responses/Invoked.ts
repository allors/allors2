import { InvokeResponse, ISession, ISessionObject } from "@allors/base-domain";

export class Invoked {
    constructor(public session: ISession, public response: InvokeResponse) {
    }
}
