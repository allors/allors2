import { IObject } from '@allors/workspace/domain/system';
import { Class, RelationType, RoleType } from '@allors/workspace/meta/system';
import { ChangeSet } from '../ChangeSet';
import { difference, Numbers } from '../collections/Numbers';
import { Session } from '../Session/Session';
import { Strategy } from '../Strategy';
import { Workspace } from './Workspace';
import { WorkspaceObject } from './WorkspaceObject';

export class WorkspaceState {
  workspaceObject: WorkspaceObject;
  changedRoleByRelationType: Map<RelationType, unknown> | undefined;

  previousWorkspaceObject: WorkspaceObject;
  previousChangedRoleByRelationType: Map<RelationType, unknown> | undefined;

  constructor(private strategy: Strategy) {
    this.workspaceObject = this.workspace.get(this.identity) as WorkspaceObject;
    this.previousWorkspaceObject = this.workspaceObject;
  }

  get hasWorkspaceChanges(): boolean {
    return !!this.changedRoleByRelationType;
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

  get workspace(): Workspace {
    return this.session.workspace;
  }

  getRole(roleType: RoleType): unknown {
    if (roleType.objectType.isUnit) {
      if (this.changedRoleByRelationType?.has(roleType.relationType)) {
        return this.changedRoleByRelationType.get(roleType.relationType);
      }

      return this.workspaceObject?.getRole(roleType);
    }

    if (roleType.isOne) {
      if (this.changedRoleByRelationType?.has(roleType.relationType)) {
        const workspaceRole = this.changedRoleByRelationType.get(roleType.relationType) as Strategy;
        return workspaceRole?.object;
      }

      const identity = this.workspaceObject?.getRole(roleType) as number;
      const workspaceRole = this.session.getStrategy(identity);
      return workspaceRole?.object;
    }

    if (this.changedRoleByRelationType?.has(roleType.relationType)) {
      const workspaceRoles = this.changedRoleByRelationType.get(roleType.relationType) as Strategy[];
      return workspaceRoles?.map((v) => v.object) ?? [];
    }

    const identities = this.workspaceObject?.getRole(roleType) as Numbers;
    return identities?.map((v) => this.session.getOne(v)) ?? [];
  }

  setUnitRole(roleType: RoleType, role: unknown): void {
    const previousRole = this.getRole(roleType);
    if (previousRole === role) {
      return;
    }

    this.changedRoleByRelationType ??= new Map();
    this.changedRoleByRelationType.set(roleType.relationType, role);

    this.session.onWorkspaceChange(this);
  }

  setCompositeRole(roleType: RoleType, value?: IObject): void {
    const role = value;
    const previousRole = this.getRole(roleType) as IObject;
    if (previousRole === role) {
      return;
    }

    // OneToOne
    if (previousRole) {
      const associationType = roleType.associationType;
      if (associationType.isOne) {
        const previousAssociationObject = this.session.getCompositeAssociation(previousRole.strategy as Strategy, associationType);
        previousAssociationObject?.strategy.set(roleType, null);
      }
    }

    this.changedRoleByRelationType ??= new Map();
    this.changedRoleByRelationType.set(roleType.relationType, role?.strategy);

    this.session.onWorkspaceChange(this);
  }

  setCompositesRole(roleType: RoleType, value: IObject[]): void {
    const previousRole = Numbers((this.getRole(roleType) as IObject[])?.map((v) => v.id));

    const role = Numbers(value?.map((v) => v.id));

    const addedRoles = difference(role, previousRole);
    const removedRoles = difference(previousRole, role);

    if (addedRoles === undefined && removedRoles === undefined) {
      return;
    }

    // OneToMany
    if (addedRoles !== undefined) {
      const associationType = roleType.associationType;
      if (associationType.isOne) {
        const addedObjects = this.session.getMany(addedRoles as number[]);
        for (const addedObject of addedObjects) {
          const previousAssociationObject = this.session.getCompositeAssociation(addedObject.strategy as Strategy, associationType);
          previousAssociationObject?.strategy.remove(roleType, addedObject);
        }
      }
    }

    this.changedRoleByRelationType ??= new Map();
    this.changedRoleByRelationType.set(roleType.relationType, role);

    this.session.onWorkspaceChange(this);
  }

  push(): void {
    if (this.hasWorkspaceChanges) {
      this.workspace.push(this.identity, this.class, this.workspaceObject?.version ?? 0, this.changedRoleByRelationType);
    }

    this.workspaceObject = this.workspace.get(this.identity) as WorkspaceObject;
    delete this.changedRoleByRelationType;
  }

  reset(): void {
    this.workspaceObject = this.workspace.get(this.identity) as WorkspaceObject;
    delete this.changedRoleByRelationType;
  }

  checkpoint(changeSet: ChangeSet): void {
    // Same workspace object
    if (this.workspaceObject.version == this.previousWorkspaceObject.version) {
      // No previous changed roles
      if (this.previousChangedRoleByRelationType == null) {
        if (this.changedRoleByRelationType != null) {
          // Changed roles
          for (const kvp of this.changedRoleByRelationType) {
            const [relationType, cooked] = kvp;
            const raw = this.workspaceObject.getRole(relationType.roleType);

            changeSet.diffCookedWithRaw(this.strategy, relationType, cooked, raw);
          }
        }
      }
      // Previous changed roles
      else {
        if (this.changedRoleByRelationType !== undefined) {
          for (const kvp of this.changedRoleByRelationType) {
            const [relationType, role] = kvp;

            const previousRole = this.previousChangedRoleByRelationType.get(relationType);
            changeSet.diffCookedWithCooked(this.strategy, relationType, role, previousRole);
          }
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
            const raw = this.workspaceObject.getRole(roleType);
            changeSet.diffRawWithCooked(this.strategy, relationType, raw, previousCooked);
          }
        } else {
          const previousRaw = this.previousWorkspaceObject?.getRole(roleType);
          if (hasCooked && this.changedRoleByRelationType?.has(relationType)) {
            const cooked = this.changedRoleByRelationType.get(relationType);

            changeSet.diffCookedWithRaw(this.strategy, relationType, cooked, previousRaw);
          } else {
            const raw = this.workspaceObject.getRole(roleType);
            changeSet.diffRawWithRaw(this.strategy, relationType, raw, previousRaw);
          }
        }
      }
    }

    this.previousWorkspaceObject = this.workspaceObject;
    this.previousChangedRoleByRelationType = this.changedRoleByRelationType;
  }

  isAssociationForRole(roleType: RoleType, forRole: Strategy): boolean {
    if (roleType.objectType.isUnit) {
      return false;
    }

    if (roleType.isOne) {
      if (this.changedRoleByRelationType != null && this.changedRoleByRelationType.has(roleType.relationType)) {
        const workspaceRole = this.changedRoleByRelationType.get(roleType.relationType);
        return workspaceRole === forRole;
      }

      const identity = this.workspaceObject?.getRole(roleType);
      return identity === forRole.id;
    }

    if (this.changedRoleByRelationType != null && this.changedRoleByRelationType.get(roleType.relationType)) {
      const workspaceRoles = this.changedRoleByRelationType.get(roleType.relationType) as Strategy[];
      return workspaceRoles.includes(forRole);
    }

    const identities = this.workspaceObject?.getRole(roleType) as Numbers;
    return identities?.includes(forRole.id) ?? false;
  }

  diff(): RelationType[] | undefined {
    if (this.changedRoleByRelationType !== undefined) {
      return Array.from(this.changedRoleByRelationType.keys());
    }
  }
}
