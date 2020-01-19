import { RoleType } from "../../framework/meta/RoleType";
import { AssociationType } from "../../framework/meta/AssociationType";

export interface MetaGatsby {

  roleTypes?: RoleType[];

  associationTypes?: AssociationType[];

  properties?: string[];
}
