import { WorkspaceObject } from './WorkspaceObject';
import { SessionObject } from './SessionObject';
import { PullResponse } from './data/responses/PullResponse';
import { SyncRequest } from './data/requests/SyncRequest';
import { SyncResponse } from './data/responses/SyncResponse';

import { ObjectType, Population as MetaPopulation } from '../../meta';

import * as _ from 'lodash';

export interface IWorkspace {
    metaPopulation: MetaPopulation;
    constructorByName: { [name: string]: typeof SessionObject};
    diff(data: PullResponse): SyncRequest;
    sync(data: SyncResponse): void;
    get(id: string): WorkspaceObject;
}

export class Workspace implements IWorkspace {
    userSecurityHash: string;

    private workspaceObjectById: { [id: string]: WorkspaceObject; } = {};

    constructor(public metaPopulation: MetaPopulation, public constructorByName: { [name: string]: typeof SessionObject}) {
    }

    diff(response: PullResponse): SyncRequest {
        let userSecurityHash = response.userSecurityHash;

        const requireLoadIdsWithVersion = _.filter(response.objects, idAndVersion => {

            const [id, version] = idAndVersion;
            const workspaceObject = this.workspaceObjectById[id];

            return (workspaceObject === undefined) ||
                (workspaceObject === null) ||
                (workspaceObject.version !== version) ||
                (workspaceObject.userSecurityHash !== userSecurityHash);
        });

        const requireLoadIds = new SyncRequest();
        requireLoadIds.objects = _.map(requireLoadIdsWithVersion, idWithVersion => {
            return idWithVersion[0];
        });

        return requireLoadIds;
    }

    sync(syncResponse: SyncResponse): void {
        _.forEach(syncResponse.objects, objectData => {
            const workspaceObject = new WorkspaceObject(this, syncResponse, objectData);
            this.workspaceObjectById[workspaceObject.id] = workspaceObject;
        });
    }

    get(id: string): WorkspaceObject {
        const workspaceObject = this.workspaceObjectById[id];
        if (workspaceObject === undefined) {
            throw new Error(`Object with id ${id} is not present.`);
        }

        return workspaceObject;
    }
}
