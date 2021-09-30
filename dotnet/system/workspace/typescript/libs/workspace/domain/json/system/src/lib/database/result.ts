import { Response } from '@allors/protocol/json/system';
import { IDerivationError, IObject, IResult, ISession } from '@allors/workspace/domain/system';
import { DerivationError } from './DerivationError';

export abstract class Result implements IResult {
  constructor(public readonly session: ISession, public readonly response: Response) {}

  get hasErrors(): boolean {
    return this.response._v?.length > 0 || this.response._a?.length > 0 || this.response._m?.length > 0 || this.response._d?.length > 0 || !!this.response._e;
  }

  get errorMessage(): string {
    return this.response._e;
  }

  get versionErrors(): IObject[] {
    return this.session.getMany<IObject>(this.response._v);
  }

  get accessErrors(): IObject[] {
    return this.session.getMany<IObject>(this.response._a);
  }

  get missingErrors(): IObject[] {
    return this.session.getMany<IObject>(this.response._m);
  }

  get derivationErrors(): IDerivationError[] {
    return this.response._d?.map((v) => new DerivationError(this.session, v)) ?? [];
  }
}
