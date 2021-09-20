import { IChangeSet, IStrategy } from '@allors/workspace/domain/system';
import { AssociationType, RelationType, RoleType } from '@allors/workspace/meta/system';
import { Session } from './Session/Session';
import { SessionStateChangeSet } from './Session/SessionStateChangeSet';
import { Strategy } from './Strategy';

export class ChangeSet implements IChangeSet {
  associationsByRoleType: Map<RoleType, Set<IStrategy>>;

  rolesByAssociationType: Map<AssociationType, Set<IStrategy>>;

  constructor(public session: Session, public created: Set<IStrategy>, public instantiated: Set<IStrategy>, sessionStateChangeSet: SessionStateChangeSet) {
    TODO:
    this.associationsByRoleType = sessionStateChangeSet.roleByAssociationByRoleType
        .ToDictionary(
            v => v.Key,
            v => (ISet<IStrategy>)new HashSet<IStrategy>(v.Value.Keys));
    this.rolesByAssociationType = sessionStateChangeSet.AssociationByRoleByRoleType.ToDictionary(
        v => v.Key,
        v => (ISet<IStrategy>)new HashSet<IStrategy>(v.Value.Keys));
  }

  addAssociation(relationType: RelationType, association: Strategy) {
    const roleType = relationType.roleType;

    let associations = this.associationsByRoleType.get(roleType);
    if (!associations) {
      associations = new Set();
      this.associationsByRoleType.set(roleType, associations);
    }

    associations.add(association);
  }

  addRole(relationType: RelationType, role: Strategy) {
    const associationType = relationType.associationType;
    let roles = this.rolesByAssociationType.get(associationType);

    if (!roles) {
      roles = new Set();
      this.rolesByAssociationType.set(associationType, roles);
    }

    roles.add(role);
  }

  diffCookedWithCooked(association: Strategy, relationType: RelationType, current: unknown, previous: unknown) {
    const roleType = relationType.roleType;

    if (roleType.objectType.isUnit) {
      if (current === previous) {
        return;
      }

      this.addAssociation(relationType, association);
    } else {
      if (roleType.isOne) {
        if (current === previous) {
          return;
        }

        if (current != null) {
          this.addRole(relationType, current as Strategy);
        }

        if (previous != null) {
          this.addRole(relationType, previous as Strategy);
        }

        this.addAssociation(relationType, association);
      } else {
        const currentRole = current as Strategy[];
        const previousRole = previous as Strategy[];

        // TODO:
        // if (currentRole?.length > 0 && previousRole?.length > 0)
        // {
        //     var addedRoles = currentRole.Except(previousRole);
        //     var removedRoles = previousRole.Except(currentRole);

        //     var hasChange = false;
        //     foreach (var role in addedRoles.Concat(removedRoles))
        //     {
        //         this.AddRole(relationType, role);
        //         hasChange = true;
        //     }

        //     if (hasChange)
        //     {
        //         this.AddAssociation(relationType, association);
        //     }
        // }
        // else if (currentRole?.Length > 0)
        // {
        //     foreach (var role in currentRole)
        //     {
        //         this.AddRole(relationType, role);
        //     }

        //     this.AddAssociation(relationType, association);
        // }
        // else if (previousRole?.Length > 0)
        // {
        //     foreach (var role in previousRole)
        //     {
        //         this.AddRole(relationType, role);
        //     }

        //     this.AddAssociation(relationType, association);
        // }
      }
    }
  }

  diffCookedWithRaw(association: Strategy, relationType: RelationType, current: unknown, previous: unknown): void {
    const roleType = relationType.roleType;

    if (roleType.objectType.isUnit) {
      if (current !== previous) {
        this.addAssociation(relationType, association);
      }
    } else {
      if (roleType.isOne) {
        if (current == null && previous == null) {
          return;
        }

        const currentRole = current as Strategy;

        if (current != null && previous != null) {
          const previousRole = this.session.getStrategy(previous as number);
          if (current === previousRole) {
            return;
          }

          this.addRole(relationType, previousRole);
          this.addRole(relationType, currentRole);
          this.addAssociation(relationType, association);
        } else if (current != null) {
          this.addRole(relationType, currentRole);
          this.addAssociation(relationType, association);
        } else {
          const previousRole = this.session.getStrategy(previous as number);
          this.addRole(relationType, previousRole);
          this.addAssociation(relationType, association);
        }
      } else {
        if (current == null && previous == null) {
          return;
        }

        const currentRole = current as Strategy[];

        if (previous == null) {
          for (const v of currentRole) {
            this.addRole(relationType, v);
          }

          this.addAssociation(relationType, association);
        } else {
          const previousRole = (previous as number[]).map((v) => this.session.getStrategy(v));

          if (currentRole == null) {
            for (const v of previousRole) {
              this.addRole(relationType, v);
            }

            this.addAssociation(relationType, association);
          } else {
            // TODO:
            // var addedRoles = currentRole.Except(previousRole);
            // var removedRoles = previousRole.Except(currentRole);
            // var hasChange = false;
            // foreach (var role in addedRoles.Concat(removedRoles))
            // {
            //     this.AddRole(relationType, role);
            //     hasChange = true;
            // }
            // if (hasChange)
            // {
            //     this.AddAssociation(relationType, association);
            // }
          }
        }
      }
    }
  }

  diffRawWithRaw(association: Strategy, relationType: RelationType, current: unknown, previous: unknown): void {
    const roleType = relationType.roleType;

    if (roleType.objectType.isUnit) {
      if (current !== previous) {
        this.addAssociation(relationType, association);
      }
    } else {
      if (roleType.isOne) {
        if (current !== previous) {
          if (previous != null) {
            this.addRole(relationType, this.session.getStrategy(previous as number));
          }

          if (current != null) {
            this.addRole(relationType, this.session.getStrategy(current as number));
          }

          this.addAssociation(relationType, association);
        }
      } else {
        if (current == null && previous == null) {
          return;
        }

        const currentRole = current as number[];

        if (previous == null) {
          for (const v of currentRole) {
            this.addRole(relationType, this.session.getStrategy(v));
          }

          this.addAssociation(relationType, association);
        } else {
          const previousRole = previous as number[];

          if (currentRole == null) {
            for (const v of previousRole) {
              this.addRole(relationType, this.session.getStrategy(v));
            }

            this.addAssociation(relationType, association);
          } else {
            // TODO:
            // var addedRoles = currentRole.Except(previousRole);
            // var removedRoles = previousRole.Except(currentRole);
            // var hasChange = false;
            // foreach (var v in addedRoles.Concat(removedRoles))
            // {
            //     this.AddRole(relationType, this.Session.GetStrategy(v));
            //     hasChange = true;
            // }
            // if (hasChange)
            // {
            //     this.AddAssociation(relationType, association);
            // }
          }
        }
      }
    }
  }

  diffRawWithCooked(association: Strategy, relationType: RelationType, current: unknown, previous: unknown): void {
    const roleType = relationType.roleType;

    if (roleType.objectType.isUnit) {
      if (current !== previous) {
        this.addAssociation(relationType, association);
      }
    } else {
      if (roleType.isOne) {
        const previousRole = previous as Strategy;
        if (current !== previousRole?.id) {
          if (previous != null) {
            this.addRole(relationType, previousRole);
          }

          if (current != null) {
            this.addRole(relationType, this.session.getStrategy(current as long));
          }

          this.addAssociation(relationType, association);
        }
      } else {
        if (current == null && previous == null) {
          return;
        }

        const currentRole = current as number[];

        if (previous == null) {
          for (const v of currentRole) {
            this.addRole(relationType, this.session.getStrategy(v));
          }

          this.addAssociation(relationType, association);
        } else {
          const previousRole = previous as Strategy[];

          if (currentRole == null) {
            for (const v of previousRole) {
              this.addRole(relationType, v);
            }

            this.addAssociation(relationType, association);
          } else {
            // TODO:
            // var previousRoleIds = previousRole.Select(v => v.Id).ToArray();
            // var addedRoles = currentRole.Except(previousRoleIds);
            // var removedRoles = previousRoleIds.Except(currentRole);
            // var hasChange = false;
            // foreach (var v in addedRoles.Concat(removedRoles))
            // {
            //     this.AddRole(relationType, this.Session.GetStrategy(v));
            //     hasChange = true;
            // }
            // if (hasChange)
            // {
            //     this.AddAssociation(relationType, association);
            // }
          }
        }
      }
    }
  }
}
