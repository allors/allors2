import { AssociationType, RoleType } from '@allors/workspace/meta/system';
import { Strategy } from '../Strategy';
import { SessionStateChangeSet } from './SessionStateChangeSet';

export class SessionState {
  private readonly roleByAssociationByRoleType: Map<RoleType, Map<Strategy, unknown>>;
  private readonly associationByRoleByAssociationType: Map<AssociationType, Map<Strategy, unknown>>;

  private changedRoleByAssociationByRoleType: Map<RoleType, Map<Strategy, unknown>>;
  private changedAssociationByRoleByAssociationType: Map<AssociationType, Map<Strategy, unknown>>;

  constructor() {
    this.roleByAssociationByRoleType = new Map();
    this.associationByRoleByAssociationType = new Map();

    this.changedRoleByAssociationByRoleType = new Map();
    this.changedAssociationByRoleByAssociationType = new Map();
  }

  checkpoint(): SessionStateChangeSet | undefined {
    // for (const roleType of this.changedRoleByAssociationByRoleType.keys())
    // {
    //     const changedRoleByAssociation = this.changedRoleByAssociationByRoleType.get(roleType) as Map<Strategy, unknown>;
    //     const roleByAssociation = this.roleByAssociation(roleType);
    //     for (const association of changedRoleByAssociation.keys())
    //     {
    //         const role = changedRoleByAssociation.get(association);
    //         const originalRole = roleByAssociation.get(association);
    //         var areEqual = originalRole === role ||
    //                        (roleType.isMany && ((IStructuralEquatable)originalRole)?.Equals((IStructuralEquatable)role) == true);
    //         if (areEqual)
    //         {
    //             _ = changedRoleByAssociation.Remove(association);
    //             continue;
    //         }
    //         roleByAssociation[association] = role;
    //     }
    //     if (roleByAssociation.Count == 0)
    //     {
    //         _ = this.changedRoleByAssociationByRoleType.Remove(roleType);
    //     }
    // }

    // foreach (var associationType in this.changedAssociationByRoleByAssociationType.Keys.ToArray())
    // {
    //     var changedAssociationByRole = this.changedAssociationByRoleByAssociationType[associationType];
    //     var associationByRole = this.AssociationByRole(associationType);
    //     foreach (var role in changedAssociationByRole.Keys.ToArray())
    //     {
    //         var changedAssociation = changedAssociationByRole[role];
    //         _ = associationByRole.TryGetValue(role, out var originalRole);
    //         var areEqual = ReferenceEquals(originalRole, changedAssociation) ||
    //                        (associationType.IsOne && Equals(originalRole, changedAssociation)) ||
    //                        (associationType.IsMany && ((IStructuralEquatable)originalRole)?.Equals((IStructuralEquatable)changedAssociation) == true);
    //         if (areEqual)
    //         {
    //             _ = changedAssociationByRole.Remove(role);
    //             continue;
    //         }
    //         associationByRole[role] = changedAssociation;
    //     }
    //     if (associationByRole.Count == 0)
    //     {
    //         _ = this.changedAssociationByRoleByAssociationType.Remove(associationType);
    //     }
    // }
    // var changeSet = new SessionStateChangeSet(this.changedRoleByAssociationByRoleType, this.changedAssociationByRoleByAssociationType);
    // this.changedRoleByAssociationByRoleType = new Dictionary<IRoleType, IDictionary<Strategy, object>>();
    // this.changedAssociationByRoleByAssociationType = new Dictionary<IAssociationType, IDictionary<Strategy, object>>();
    // return changeSet;

    return undefined;
  }

  getRole(association: Strategy, roleType: RoleType): unknown {
    // if (this.changedRoleByAssociationByRoleType.TryGetValue(roleType, out var changedRoleByAssociation) &&
    //     changedRoleByAssociation.TryGetValue(association, out role))
    // {
    //     return;
    // }
    // _ = this.RoleByAssociation(roleType).TryGetValue(association, out role);

    return undefined;
  }

  setUnitRole(association: Strategy, roleType: RoleType, role: unknown): void {
    // if (role == null)
    // {
    //     this.removeRole(association, roleType);
    //     return;
    // }
    // var unitRole = roleType.normalizeUnit(role);
    // this.changedRoleByAssociation(roleType)[association] = unitRole;
  }

  setCompositeRole(association: Strategy, roleType: RoleType, role: unknown): void {
    // if (role == null) {
    //   this.removeRole(association, roleType);
    //   return;
    // }

    // var associationType = roleType.associationType;
    // let role = this.getRole(association, roleType);
    // let roleIdentity = role;
    // let previousAssociation = this.getAssociation(roleIdentity, associationType);

    // // Role
    // var changedRoleByAssociation = this.changedRoleByAssociation(roleType);
    // changedRoleByAssociation[association] = roleIdentity;

    // // Association
    // var changedAssociationByRole = this.changedAssociationByRole(associationType);
    // if (associationType.isOne) {
    //   // One to One
    //   var previousAssociationObject = previousAssociation;
    //   if (previousAssociationObject != null) {
    //     changedRoleByAssociation[previousAssociationObject] = null;
    //   }

    //   if (previousRole != null) {
    //     var previousRoleObject = previousRole;
    //     changedAssociationByRole[previousRoleObject] = null;
    //   }

    //   changedAssociationByRole[roleIdentity] = association;
    // } else {
    //   changedAssociationByRole[roleIdentity] = NullableSortableArraySet.Remove(previousAssociation, roleIdentity);
    // }
  }

  public setCompositesRole(association: Strategy, roleType: RoleType, role: unknown) {
    // if (role == null) {
    //   this.removeRole(association, roleType);
    //   return;
    // }

    // let previousRole = this.getRole(association, roleType);
    // let compositesRole = role;
    // let previousRoles = previousRole ?? [];

    // // Use Diff (Add/Remove)
    // var addedRoles = compositesRole.Except(previousRoles);
    // var removedRoles = previousRoles.Except(compositesRole);

    // for (const addedRole of addedRoles) {
    //   this.addRole(association, roleType, addedRole);
    // }

    // for (const removeRole of removedRoles) {
    //   this.removeRole(association, roleType, removeRole);
    // }
  }

  addRole(association: Strategy, roleType: RoleType, role: Strategy): void {
    // var associationType = roleType.associationType;
    // var previousAssociation = this.getAssociation(role, associationType);

    // // Role
    // var changedRoleByAssociation = this.changedRoleByAssociation(roleType);
    // let previousRole = this.getRole(association, roleType);
    // var roleArray = previousRole;
    // roleArray = NullableSortableArraySet.Add(roleArray, role);
    // changedRoleByAssociation[association] = roleArray;

    // // Association
    // var changedAssociationByRole = this.changedAssociationByRole(associationType);
    // if (associationType.isOne) {
    //   // One to Many
    //   var previousAssociationObject = previousAssociation;
    //   if (previousAssociationObject != null) {
    //     let previousAssociationRole = this.getRole(previousAssociationObject, roleType);
    //     changedRoleByAssociation[previousAssociationObject] = NullableSortableArraySet.Remove(previousAssociationRole, role);
    //   }

    //   changedAssociationByRole[role] = association;
    // } else {
    //   // Many to Many
    //   changedAssociationByRole[role] = NullableSortableArraySet.Add(previousAssociation, association);
    // }
  }

  removeRole(association: Strategy, roleType: RoleType, role?: Strategy): void {
    // var associationType = roleType.associationType;
    // let previousAssociation = this.getAssociation(role, associationType);

    // let previousRole = this.getRole(association, roleType);
    // if (previousRole != null) {
    //   // Role
    //   var changedRoleByAssociation = this.changedRoleByAssociation(roleType);
    //   changedRoleByAssociation.set(association, NullableSortableArraySet.Remove(previousRole, role));

    //   // Association
    //   var changedAssociationByRole = this.changedAssociationByRole(associationType);
    //   if (associationType.isOne) {
    //     // One to Many
    //     changedAssociationByRole[role] = null;
    //   } else {
    //     // Many to Many
    //     changedAssociationByRole[role] = NullableSortableArraySet.Add(previousAssociation, association);
    //   }
    // }
  }

  // removeRole(association: Strategy, roleType: RoleType): void {
  //   if (roleType.objectType.IsUnit) {
  //     // Role
  //     this.changedRoleByAssociation(roleType)[association] = null;
  //   } else {
  //     var associationType = roleType.associationType;
  //     let previousRole = this.getRole(association, roleType);

  //     if (roleType.isOne) {
  //       // Role
  //       var changedRoleByAssociation = this.changedRoleByAssociation(roleType);
  //       changedRoleByAssociation[association] = null;

  //       // Association
  //       var changedAssociationByRole = this.changedAssociationByRole(associationType);
  //       if (associationType.isOne) {
  //         // One to One
  //         if (previousRole != null) {
  //           var previousRoleObject = previousRole;
  //           changedAssociationByRole[previousRoleObject] = null;
  //         }
  //       }
  //     } else {
  //       var previousRoles = previousRole;
  //       if (previousRoles != null) {
  //         // Use Diff (Remove)
  //         for (let removeRole of previousRoles) {
  //           this.removeRole(association, roleType, removeRole);
  //         }
  //       }
  //     }
  //   }
  // }

  getAssociation(role: Strategy, associationType: AssociationType): unknown {
    return this.changedAssociationByRoleByAssociationType?.get(associationType)?.get(role);
  }

  associationByRole(associationType: AssociationType): Map<Strategy, unknown> {
    let associationByRole = this.associationByRoleByAssociationType.get(associationType);
    if (!associationByRole) {
      associationByRole = new Map();
      this.associationByRoleByAssociationType.set(associationType, associationByRole);
    }

    return associationByRole;
  }

  roleByAssociation(roleType: RoleType): Map<Strategy, unknown> {
    let roleByAssociation = this.roleByAssociationByRoleType.get(roleType);
    if (!roleByAssociation) {
      roleByAssociation = new Map();
      this.roleByAssociationByRoleType.set(roleType, roleByAssociation);
    }

    return roleByAssociation;
  }

  changedAssociationByRole(associationType: AssociationType): Map<Strategy, unknown> {
    let changedAssociationByRole = this.changedAssociationByRoleByAssociationType.get(associationType);
    if (!changedAssociationByRole) {
      changedAssociationByRole = new Map();
      this.changedAssociationByRoleByAssociationType.set(associationType, changedAssociationByRole);
    }

    return changedAssociationByRole;
  }

  changedRoleByAssociation(roleType: RoleType): Map<Strategy, unknown> {
    let changedRoleByAssociation = this.changedRoleByAssociationByRoleType.get(roleType);
    if (!changedRoleByAssociation) {
      changedRoleByAssociation = new Map();
      this.changedRoleByAssociationByRoleType.set(roleType, changedRoleByAssociation);
    }

    return changedRoleByAssociation;
  }

  isAssociationForRole(association: Strategy, roleType: RoleType, forRole: Strategy): boolean {
    // return this.getRole(association) === forRole;
    return false;
  }
}
