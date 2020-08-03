import { ISession } from '@allors/workspace/domain';
import { InvokeResponse } from '@allors/workspace/protocol';

export class Invoked {
  constructor(public session: ISession, public response: InvokeResponse) {}
}
