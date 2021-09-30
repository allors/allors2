import { AssociationType,  RoleType} from "@allors/workspace/meta/system";
import { ISession } from "./ISession";
import { IStrategy } from "./IStrategy";

export interface IChangeSet {
  session: ISession;

  created: Set<IStrategy>;

  instantiated: Set<IStrategy>;

  associationsByRoleType: Map<RoleType, Set<IStrategy>>;

  rolesByAssociationType: Map<AssociationType, Set<IStrategy>>;
}
