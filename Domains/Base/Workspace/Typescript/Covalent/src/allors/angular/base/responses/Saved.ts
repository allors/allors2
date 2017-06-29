import { ISession, ISessionObject, PushResponse } from '../../../domain';

export class Saved {
    constructor(public session: ISession, public response: PushResponse) {
    }
}
