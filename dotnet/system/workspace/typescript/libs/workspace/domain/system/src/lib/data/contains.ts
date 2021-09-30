import { PropertyType } from '@allors/workspace/meta/system';
import { CompositeTypes } from "../Types";
import { ParameterizablePredicate } from './ParameterizablePredicate';

export interface Contains extends ParameterizablePredicate {
  propertyType: PropertyType;
  object?: CompositeTypes;
}
