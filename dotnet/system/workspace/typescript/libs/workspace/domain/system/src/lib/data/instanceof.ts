import { ObjectType, PropertyType } from '@allors/workspace/meta/system';
import { ParameterizablePredicate } from './ParameterizablePredicate';

export interface Instanceof extends ParameterizablePredicate {
  propertyType: PropertyType;
  instanceObjectType?: ObjectType;
}
