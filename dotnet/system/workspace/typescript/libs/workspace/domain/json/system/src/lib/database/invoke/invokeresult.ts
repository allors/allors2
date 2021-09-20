import { InvokeResponse } from '@allors/protocol/json/system';
import { IInvokeResult, ISession } from '@allors/workspace/domain/system';
import { Result } from '../Result';

export class InvokeResult extends Result implements IInvokeResult {
  constructor(session: ISession, invokeResponse: InvokeResponse) {
    super(session, invokeResponse);
  }
}
