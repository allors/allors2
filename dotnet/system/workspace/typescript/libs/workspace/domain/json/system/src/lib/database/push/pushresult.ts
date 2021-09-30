import { PushResponse } from '@allors/protocol/json/system';
import { IPushResult, ISession } from '@allors/workspace/domain/system';
import { Result } from '../Result';

export class PushResult extends Result implements IPushResult {
  constructor(session: ISession, response: PushResponse) {
    super(session, response);
  }
}
