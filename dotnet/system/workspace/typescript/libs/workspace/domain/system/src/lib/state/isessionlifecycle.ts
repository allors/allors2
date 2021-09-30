import { ISession } from '../ISession';

export interface ISessionLifecycle {
  onInit(session: ISession): void;
}
