import { PushRequestNewObject, PushRequestObject, PushRequestRole } from '@allors/protocol/json/system';
import { IObject, ISession, Operations, UnitTypes } from '@allors/workspace/domain/system';
import { Class, MethodType, RelationType, RoleType } from '@allors/workspace/meta/system';
import { Session } from '../session/Session';
import { ChangeSet } from '../ChangeSet';
import { Strategy } from '../Strategy';
import { Database } from './Database';
import { DatabaseObject } from './DatabaseObject';
import { Permission } from './Security/Permission';
import { difference, Numbers } from '../collections/Numbers';

export class DatabaseState {
  databaseObject: DatabaseObject;

  private changedRoleByRelationType: Map<RelationType, any> | undefined;

  private previousDatabaseObject: DatabaseObject;
  private previousChangedRoleByRelationType: Map<RelationType, unknown> | undefined;

  constructor(private readonly strategy: Strategy, databaseObject?: DatabaseObject) {
    this.strategy = strategy;
    this.databaseObject = databaseObject ?? (this.database.get(this.identity) as DatabaseObject);
    this.previousDatabaseObject = this.databaseObject;
  }

  get hasDatabaseChanges(): boolean {
    return !!this.databaseObject || !!this.changedRoleByRelationType;
  }

  get existDatabaseObjects(): boolean {
    return !!this.databaseObject;
  }

  get version(): number {
    return this.databaseObject?.version ?? 0;
  }

  get identity(): number {
    return this.strategy.id;
  }

  get class(): Class {
    return this.strategy.class;
  }

  get session(): Session {
    return this.strategy.session;
  }

  get database(): Database {
    return this.session.database;
  }

  canRead(roleType: RoleType): boolean {
    if (!this.existDatabaseObjects) {
      return true;
    }

    const permission = this.session.workspace.database.getPermission(this.class, roleType, Operations.Read) as Permission;
    return this.databaseObject.isPermitted(permission);
  }

  canWrite(roleType: RoleType): boolean {
    if (!this.existDatabaseObjects) {
      return true;
    }

    const permission = this.session.workspace.database.getPermission(this.class, roleType, Operations.Write) as Permission;
    return this.databaseObject.isPermitted(permission);
  }

  canExecute(methodType: MethodType): boolean {
    if (!this.existDatabaseObjects) {
      return true;
    }

    const permission = this.session.workspace.database.getPermission(this.class, methodType, Operations.Execute) as Permission;
    return this.databaseObject.isPermitted(permission);
  }

  getRole(roleType: RoleType): unknown {
    if (roleType.objectType.isUnit) {
      if (this.changedRoleByRelationType?.has(roleType.relationType)) {
        return this.changedRoleByRelationType?.get(roleType.relationType);
      }

      return this.databaseObject?.getRole(roleType);
    }

    if (roleType.isOne) {
      if (this.changedRoleByRelationType?.has(roleType.relationType)) {
        return (this.changedRoleByRelationType?.get(roleType.relationType) as Strategy)?.object;
      }

      const identity = this.databaseObject?.getRole(roleType) as number;
      return this.session.getStrategy(identity)?.object;
    }

    if (this.changedRoleByRelationType?.has(roleType.relationType)) {
      const workspaceRoles = this.changedRoleByRelationType?.get(roleType.relationType) as Strategy[];
      return workspaceRoles != null ? workspaceRoles.map((v) => v.object) : [];
    }

    const identities = this.databaseObject?.getRole(roleType) as Numbers;
    return identities !== undefined ? this.session.getMany<IObject>(identities) : [];
  }

  setUnitRole(roleType: RoleType, role: unknown): void {
    const previousRole = this.getRole(roleType);
    if (previousRole === role) {
      return;
    }

    this.changedRoleByRelationType ??= new Map();
    this.changedRoleByRelationType.set(roleType.relationType, role);

    this.session.onDatabaseChange(this);
  }

  setCompositeRole(roleType: RoleType, value: IObject) {
    const role = value;
    const previousRole = this.getRole(roleType) as IObject;
    if (previousRole === role) {
      return;
    }

    // OneToOne
    if (previousRole != null) {
      const associationType = roleType.associationType;
      if (associationType.isOne) {
        const previousAssociationObject = this.session.getCompositeAssociation<IObject>(previousRole.strategy as Strategy, associationType);
        previousAssociationObject?.strategy.set(roleType, null);
      }
    }

    this.changedRoleByRelationType ??= new Map();
    this.changedRoleByRelationType.set(roleType.relationType, role?.strategy);

    this.session.onDatabaseChange(this);
  }

  setCompositesRole(roleType: RoleType, value: IObject[]): void {
    const previousRole = this.getRole(roleType) as IObject[];

    let role: IObject[] = [];
    if (value != null) {
      role = value;
    }

    const addedRoles = role.filter((v) => !previousRole.includes(v));
    const removedRoles = previousRole.filter((v) => !role.includes(v));

    if (addedRoles.length === 0 && removedRoles.length === 0) {
      return;
    }

    // OneToMany
    if (previousRole.length > 0) {
      const associationType = roleType.associationType;
      if (associationType.isOne) {
        const addedObjects = this.session.getMany<IObject>(addedRoles);
        for (const addedObject of addedObjects) {
          const previousAssociationObject = this.session.getCompositeAssociation<IObject>(addedObject.strategy as Strategy, associationType);
          previousAssociationObject?.strategy.remove(roleType, addedObject);
        }
      }
    }

    this.changedRoleByRelationType ??= new Map();
    this.changedRoleByRelationType.set(
      roleType.relationType,
      role.map((v) => v.strategy)
    );

    this.session.onDatabaseChange(this);
  }

  reset(): void {
    this.databaseObject = this.database.get(this.identity) as DatabaseObject;
    delete this.changedRoleByRelationType;
  }

  checkpoint(changeSet: ChangeSet): void {
    // Same workspace object
    if (this.databaseObject.version === this.previousDatabaseObject.version) {
      // No previous changed roles
      if (this.previousChangedRoleByRelationType == null) {
        if (this.changedRoleByRelationType != null) {
          // Changed roles
          for (const kvp of this.changedRoleByRelationType) {
            const [relationType, cooked] = kvp;
            const raw = this.databaseObject.getRole(relationType.roleType);
            changeSet.diffCookedWithRaw(this.strategy, relationType, cooked, raw);
          }
        }
      }
      // Previous changed roles
      else {
        for (const kvp in this.changedRoleByRelationType) {
          const [relationType, role] = kvp;

          const previousRole = this.previousChangedRoleByRelationType.get(relationType);
          changeSet.diffCookedWithCooked(this.strategy, relationType, role, previousRole);
        }
      }
    }
    // Different workspace objects
    else {
      const hasPreviousCooked = this.previousChangedRoleByRelationType != null;
      const hasCooked = this.changedRoleByRelationType != null;

      for (const roleType of this.class.roleTypes) {
        const relationType = roleType.relationType;

        if (hasPreviousCooked && this.previousChangedRoleByRelationType?.has(relationType)) {
          const previousCooked = this.previousChangedRoleByRelationType.get(relationType);
          if (hasCooked && this.changedRoleByRelationType?.has(relationType)) {
            const cooked = this.changedRoleByRelationType.get(relationType);
            changeSet.diffCookedWithCooked(this.strategy, relationType, cooked, previousCooked);
          } else {
            const raw = this.databaseObject.getRole(roleType);
            changeSet.diffRawWithCooked(this.strategy, relationType, raw, previousCooked);
          }
        } else {
          const previousRaw = this.previousDatabaseObject?.getRole(roleType);
          if (hasCooked && this.changedRoleByRelationType?.has(relationType)) {
            const cooked = this.changedRoleByRelationType.get(relationType);
            changeSet.diffCookedWithRaw(this.strategy, relationType, cooked, previousRaw);
          } else {
            const raw = this.databaseObject.getRole(roleType);
            changeSet.diffRawWithRaw(this.strategy, relationType, raw, previousRaw);
          }
        }
      }
    }

    this.previousDatabaseObject = this.databaseObject;
    this.previousChangedRoleByRelationType = this.changedRoleByRelationType;
  }

  pushResponse(newDatabaseObject: DatabaseObject): void {
    this.databaseObject = newDatabaseObject;
  }

  pushNew(): PushRequestNewObject {
    return {
      w: this.identity,
      t: this.class.tag,
      r: this.pushRoles(),
    };
  }

  pushExisting(): PushRequestObject {
    return {
      d: this.identity,
      v: this.version,
      r: this.pushRoles(),
    };
  }

  pushRoles(): PushRequestRole[] | undefined {
    if (this.changedRoleByRelationType && this.changedRoleByRelationType.size > 0) {
      const roles: PushRequestRole[] = [];

      for (const kvp of this.changedRoleByRelationType) {
        const [relationType, roleValue] = kvp;

        const pushRequestRole: PushRequestRole = { t: relationType.tag };

        if (relationType.roleType.objectType.isUnit) {
          pushRequestRole.u = roleValue as UnitTypes;
        } else {
          if (relationType.roleType.isOne) {
            pushRequestRole.c = (roleValue as Strategy)?.id;
          } else {
            const roleIds = (roleValue as Strategy[]).map((v) => v.id);
            if (!this.existDatabaseObjects) {
              pushRequestRole.a = roleIds;
            } else {
              const databaseRole = this.databaseObject.getRole(relationType.roleType) as Numbers;
              if (databaseRole == null) {
                pushRequestRole.a = roleIds;
              } else {
                pushRequestRole.a = difference(roleIds, databaseRole);
                pushRequestRole.r = difference(databaseRole, roleIds);
              }
            }
          }
        }

        roles.push(pushRequestRole);
      }

      return roles;
    }

    return undefined;
  }

  isAssociationForRole(roleType: RoleType, forRole: Strategy): boolean {
    if (roleType.objectType.isUnit) {
      return false;
    }

    if (roleType.isOne) {
      if (this.changedRoleByRelationType?.has(roleType.relationType)) {
        const workspaceRole = this.changedRoleByRelationType.get(roleType.relationType);
        return workspaceRole?.Equals(forRole) == true;
      }

      const identity = this.databaseObject?.getRole(roleType);
      return identity === forRole.id;
    }

    if (this.changedRoleByRelationType?.has(roleType.relationType)) {
      const workspaceRoles = this.changedRoleByRelationType.get(roleType.relationType);
      return workspaceRoles?.Contains(forRole) == true;
    }

    const identities = this.databaseObject?.getRole(roleType) as Numbers;
    return identities?.includes(forRole.id) ?? false;
  }

  diff(): RelationType[] {
    if (!this.changedRoleByRelationType) {
      return [];
    }

    return Array.from(this.changedRoleByRelationType.keys());
  }
}
