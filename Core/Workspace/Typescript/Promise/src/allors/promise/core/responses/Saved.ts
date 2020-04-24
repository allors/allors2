import { ISession, PushResponse } from '../../../framework';

export class Saved {
  constructor(public session: ISession, public response: PushResponse) {}
}
