import { ResponseDerivationError } from '@allors/protocol/json/system';
import { IDerivationError, Role } from '@allors/workspace/domain/system';
import { Session } from '../Session/Session';

export class DerivationError implements IDerivationError {
  constructor(private session: Session, private responseDerivationError: ResponseDerivationError) {}

  get message() {
    return this.responseDerivationError.m;
  }

  get roles(): Role[] {
    return this.responseDerivationError.r.map((r) => {
      return {
        object: this.session.getOne(r[0]),
        relationType: this.session.workspace.metaPopulation.metaObjectByTag.get(r[1]),
      } as Role;
    });
  }
}
