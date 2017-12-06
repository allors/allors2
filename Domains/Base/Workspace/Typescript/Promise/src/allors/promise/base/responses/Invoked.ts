import { InvokeResponse, ISession, ISessionObject } from "../../../framework";

export class Invoked {
    constructor(public session: ISession, public response: InvokeResponse) {
    }
}
