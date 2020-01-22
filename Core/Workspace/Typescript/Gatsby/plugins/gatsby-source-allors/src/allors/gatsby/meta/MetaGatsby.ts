import { RoleType } from "../../framework/meta/RoleType";
import { AssociationType } from "../../framework/meta/AssociationType";

export interface MetaGatsby {

  roleTypes?: RoleType[];

  associationTypes?: AssociationType[];

  properties?: string[];

  // TODO: Move to separate schema generator
  schema?: RoleType[];
}
