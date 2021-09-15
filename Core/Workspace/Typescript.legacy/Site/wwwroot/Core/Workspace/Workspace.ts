namespace Allors {

  export interface IWorkspace {
    objectTypeById: { [id: string]: Meta.ObjectType; };
    objectTypeByName: { [name: string]: Meta.ObjectType; };

    diff(data: Data.PullResponse): Data.SyncRequest;
    sync(data: Data.SyncResponse): void;
    get(id: string): WorkspaceObject;
  }

  export class Workspace implements IWorkspace {
    objectTypeById: { [id: string]: Meta.ObjectType; } = {};
    objectTypeByName: { [name: string]: Meta.ObjectType; } = {};
    userSecurityHash: string;

    private workspaceObjectById: { [id: string]: WorkspaceObject; } = {};

    constructor(metaPopulationData: Data.MetaPopulation) {
      _.forEach(metaPopulationData.classes, classData => {
        var objectType = new Meta.ObjectType(classData);
        this.objectTypeById[objectType.id] = objectType;
        this.objectTypeByName[objectType.name] = objectType;
      });
    }

    diff(response: Data.PullResponse): Data.SyncRequest {
      var userSecurityHash = response.userSecurityHash;

      const requireLoadIdsWithVersion = _.filter(response.objects, idAndVersion => {

        var [id, version] = idAndVersion;
        var workspaceObject = this.workspaceObjectById[id];

        return (workspaceObject === undefined) || (workspaceObject === null) || (workspaceObject.version !== version) || (workspaceObject.userSecurityHash !== userSecurityHash);
      });

      const requireLoadIds = new Data.SyncRequest();
      requireLoadIds.objects = _.map(requireLoadIdsWithVersion, idWithVersion => {
        return idWithVersion[0];
      });

      return requireLoadIds;
    }

    sync(syncResponse: Data.SyncResponse): void {
      _.forEach(syncResponse.objects, objectData => {
        var workspaceObject = new WorkspaceObject(this, syncResponse, objectData);
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
}
