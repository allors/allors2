import { InvokeResponse, ISession, ISessionObject } from "@baseDomain";

export class Invoked {
    constructor(public session: ISession, public response: InvokeResponse) {
    }
}
