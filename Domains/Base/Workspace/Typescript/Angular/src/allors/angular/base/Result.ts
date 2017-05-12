import { ISession } from '../../domain/base/Session';
import { ISessionObject } from '../../domain/base/SessionObject';

import { PullResponse } from '../../domain/base/data/responses/PullResponse';

export class Result {

    objects: { [name: string]: ISessionObject; } = {};
    collections: { [name: string]: ISessionObject[]; } = {};
    values: { [name: string]: any; } = {};

    constructor(session: ISession, response: PullResponse) {
        // TODO: Deduplicate
        const namedObjects = response.namedObjects;
        const namedCollections = response.namedCollections;
        const namedValues = response.namedValues;

        Object.keys(namedObjects).map((k) => this.objects[k] = session.get(namedObjects[k]));
        Object.keys(namedCollections).map((k) => this.collections[k] = namedCollections[k].map((obj) => { return session.get(obj); }));
        Object.keys(namedValues).map((k) => this.values[k] = namedValues[k]);
    }
}
