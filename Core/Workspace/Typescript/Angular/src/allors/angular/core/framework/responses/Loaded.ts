import { ISession, ISessionObject, PullResponse } from '../../../../framework';

export class Loaded {

  public objects: { [name: string]: ISessionObject; } = {};
  public collections: { [name: string]: ISessionObject[]; } = {};
  public values: { [name: string]: any; } = {};

  constructor(public session: ISession, public response: PullResponse) {
    const namedObjects = response.namedObjects;
    const namedCollections = response.namedCollections;
    const namedValues = response.namedValues;

    if (namedObjects) {
      Object.keys(namedObjects).forEach((key: string) => this.objects[key] = session.get(namedObjects[key]));
    }

    if (namedCollections) {
      Object.keys(namedCollections).forEach((key: string) => this.collections[key] = namedCollections[key].map((id: string) => session.get(id)));
    }

    if (namedValues) {
      Object.keys(namedValues).forEach((key: string) => this.values[key] = namedValues[key]);
    }
  }
}
