import { InvokeResponse, ISession, ISessionObject } from "../../../domain";

export class Invoked {
    constructor(public session: ISession, public response: InvokeResponse) {
    }
}
