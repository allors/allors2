import { ISession } from '@allors/workspace/domain';
import { PushResponse } from '@allors/workspace/protocol';

export class Saved {
  constructor(public session: ISession, public response: PushResponse) {}
}
