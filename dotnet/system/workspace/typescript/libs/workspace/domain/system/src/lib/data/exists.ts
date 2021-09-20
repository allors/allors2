import { PropertyType } from '@allors/workspace/meta/system';
import { ParameterizablePredicate } from "./ParameterizablePredicate";

export interface Exists extends ParameterizablePredicate {
  propertyType: PropertyType;
}
