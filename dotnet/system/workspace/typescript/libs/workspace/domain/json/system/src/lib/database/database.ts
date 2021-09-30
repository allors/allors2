import { PullResponse, SecurityRequest, SyncRequest, SyncResponse, SecurityResponse } from '@allors/protocol/json/system';
import { IObject, Operations } from '@allors/workspace/domain/system';
import { Class, MetaPopulation, MethodType, OperandType, RelationType } from '@allors/workspace/meta/system';
import { Observable } from 'rxjs';
import { equals, properSubset } from '../collections/Numbers';
import { Identities } from '../Identities';
import { Client } from './Client';
import { DatabaseObject } from './DatabaseObject';
import { AccessControl } from './Security/AccessControl';
import { Permission } from './Security/Permission';
import { ResponseContext } from './Security/ResponseContext';
import { MapMap } from '../collections/MapMap';

export class Database {
  objectsById: Map<number, DatabaseObject>;

  accessControlById: Map<number, AccessControl>;
  permissionById: Map<number, Permission>;

  readPermissionByOperandTypeByClass: MapMap<Class, OperandType, Permission>;
  writePermissionByOperandTypeByClass: MapMap<Class, OperandType, Permission>;
  executePermissionByOperandTypeByClass: MapMap<Class, OperandType, Permission>;

  constructor(public metaPopulation: MetaPopulation, public client: Client, public identities: Identities) {
    this.objectsById = new Map();

    this.accessControlById = new Map();
    this.permissionById = new Map();

    this.readPermissionByOperandTypeByClass = new MapMap();
    this.writePermissionByOperandTypeByClass = new MapMap();
    this.executePermissionByOperandTypeByClass = new MapMap();
  }

  pushResponse(identity: number, cls: Class): DatabaseObject {
    const databaseObject = new DatabaseObject(this, identity, cls);
    this.objectsById.set(identity, databaseObject);
    return databaseObject;
  }

  syncResponse(syncResponse: SyncResponse): SecurityRequest | null {
    const ctx = new ResponseContext(this.accessControlById, this.permissionById);
    for (const syncResponseObject of syncResponse.o) {
      const databaseObjects = DatabaseObject.fromResponse(this, ctx, syncResponseObject);
      this.objectsById.set(databaseObjects.identity, databaseObjects);
    }

    if (ctx.missingAccessControlIds.size > 0 || ctx.missingPermissionIds.size > 0) {
      return {
        a: Array.from(ctx.missingAccessControlIds),
        p: Array.from(ctx.missingPermissionIds),
      };
    }

    return null;
  }

  diff(response: PullResponse): SyncRequest {
    return {
      o: response.p
        .filter((v) => {
          const obj = this.objectsById.get(v.i);

          if (!obj) {
            return true;
          }

          if (obj.version === v.v) {
            return true;
          }

          if (!equals(v.a, obj.accessControlIds)) {
            if (properSubset(v.a, obj.accessControlIds)) {
              obj.updateAccessControlIds(v.a);
            } else {
              return true;
            }
          }

          if (!equals(v.d, obj.deniedPermissionIds)) {
            if (properSubset(v.d, obj.deniedPermissionIds)) {
              obj.updateDeniedPermissionIds(v.d);
            } else {
              return true;
            }
          }

          return false;
        })
        .map((v) => v.i),
    };
  }

  get(identity: number): DatabaseObject | undefined {
    return this.objectsById.get(identity);
  }

  securityResponse(securityResponse: SecurityResponse): SecurityRequest | undefined {
    if (securityResponse.p != null) {
      for (const syncResponsePermission of securityResponse.p) {
        const id = syncResponsePermission[0];
        const cls = this.metaPopulation.metaObjectByTag.get(syncResponsePermission[1]) as Class;
        const metaObject = this.metaPopulation.metaObjectByTag.get(syncResponsePermission[2]);
        const operandType: OperandType = (metaObject as RelationType)?.roleType ?? (metaObject as MethodType);
        const operation = syncResponsePermission[3];
        const permission = new Permission(id, cls, operandType, operation);
        this.permissionById.set(id, permission);
        switch (operation) {
          case Operations.Read:
            this.readPermissionByOperandTypeByClass.set(cls, operandType, permission);
            break;
          case Operations.Write:
            this.writePermissionByOperandTypeByClass.set(cls, operandType, permission);
            break;
          case Operations.Execute:
            this.executePermissionByOperandTypeByClass.set(cls, operandType, permission);
            break;
        }
      }
    }

    let missingPermissionIds: Set<number> | undefined = undefined;
    if (securityResponse.a != null) {
      for (const syncResponseAccessControl of securityResponse.a) {
        const id = syncResponseAccessControl.i;
        const version = syncResponseAccessControl.v;
        const permissionsIds = syncResponseAccessControl.p?.map((v) => {
          if (this.permissionById.has(v)) {
            return v;
          }

          (missingPermissionIds ??= new Set()).add(v);

          return v;
        });

        const permissionIdSet = permissionsIds != null ? new Set(permissionsIds) : new Set();

        this.accessControlById.set(id, new AccessControl(id, version, permissionIdSet));
      }
    }

    if (missingPermissionIds) {
      return {
        p: Array.from(missingPermissionIds),
      };
    }

    return undefined;
  }

  getPermission(cls: Class, operandType: OperandType, operation: Operations): Permission | undefined {
    switch (operation) {
      case Operations.Read:
        return this.readPermissionByOperandTypeByClass.get(cls, operandType);
      case Operations.Write:
        return this.writePermissionByOperandTypeByClass.get(cls, operandType);
      default:
        return this.executePermissionByOperandTypeByClass.get(cls, operandType);
    }
  }

  pull(pullRequest: PullRequest): Observable<PullResponse> {
    // var uri = new Uri("pull", UriKind.Relative);
    // var response = await this.PostAsJsonAsync(uri, pullRequest);
    // _ = response.EnsureSuccessStatusCode();
    // return await this.ReadAsAsync<PullResponse>(response);

    // TODO:
    return undefined;
  }

  pull(name: string, values?: Map<string, object>, objects: Map<string, IObject>, collections: Map<string, IObject[]>): Observable<PullResponse> {
    // const pullArgs: PullArgs = {
    //     v: values?.ToDictionary(v => v.Key, v => v.Value),
    //     o: objects?.ToDictionary(v => v.Key, v => v.Value.Id),
    //     c: collections?.ToDictionary(v => v.Key, v => v.Value.Select(v => v.Id).ToArray()),
    // };
    // var uri = new Uri(name + "/pull", UriKind.Relative);
    // var response = await this.PostAsJsonAsync(uri, pullArgs);
    // _ = response.EnsureSuccessStatusCode();
    // return await this.ReadAsAsync<PullResponse>(response);

    // TODO:
    return undefined;
  }

  sync(syncRequest: SyncRequest): Observable<SyncResponse> {
    // var uri = new Uri("sync", UriKind.Relative);
    // var response = await this.PostAsJsonAsync(uri, syncRequest);
    // _ = response.EnsureSuccessStatusCode();
    // return await this.ReadAsAsync<SyncResponse>(response);

    // TODO:
    return undefined;
  }

  push(pushRequest: PushRequest): Observable<PushResponse> {
    // var uri = new Uri("push", UriKind.Relative);
    // var response = await this.PostAsJsonAsync(uri, pushRequest);
    // _ = response.EnsureSuccessStatusCode();
    // return await this.ReadAsAsync<PushResponse>(response);

    // TODO:
    return undefined;
  }

  invoke(invokeRequest: InvokeRequest): Observable<InvokeResponse> {
    // var uri = new Uri("invoke", UriKind.Relative);
    // var response = await this.PostAsJsonAsync(uri, invokeRequest);
    // _ = response.EnsureSuccessStatusCode();
    // return await this.ReadAsAsync<InvokeResponse>(response);

    // TODO:
    return undefined;
  }

  security(securityRequest: SecurityRequest): Observable<SecurityResponse> {
    // var uri = new Uri("security", UriKind.Relative);
    // var response = await this.PostAsJsonAsync(uri, securityRequest);
    // _ = response.EnsureSuccessStatusCode();
    // return await this.ReadAsAsync<SecurityResponse>(response);

    // TODO:
    return undefined;
  }
}
