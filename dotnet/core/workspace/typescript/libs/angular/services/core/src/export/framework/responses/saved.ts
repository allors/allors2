import { ISession } from '@allors/domain/system';
import { PushResponse } from '@allors/protocol/system';

export class Saved {
  constructor(public session: ISession, public response: PushResponse) {}
}
