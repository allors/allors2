import { ISession, ISessionObject, InvokeResponse } from '../../../domain';

export class Invoked {
    constructor(public session: ISession, public response: InvokeResponse) {
    }
}
