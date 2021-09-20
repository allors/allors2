import { PropertyType } from '@allors/workspace/meta/system';
import { CompositeTypes, UnitTypes } from "../Types";
import { ParameterizablePredicate } from "./ParameterizablePredicate";

export interface Equals extends ParameterizablePredicate {
  propertyType: PropertyType;
  value?: UnitTypes;
  object?: CompositeTypes;
}
