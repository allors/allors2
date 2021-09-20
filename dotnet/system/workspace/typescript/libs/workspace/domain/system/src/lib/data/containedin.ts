import { PropertyType } from '@allors/workspace/meta/system';
import { CompositeTypes } from '../Types';
import { ParameterizablePredicate } from './ParameterizablePredicate';
import { IExtent } from './IExtent';

export interface ContainedIn extends ParameterizablePredicate {
  propertyType: PropertyType;
  extent?: IExtent;
  objects?: Array<CompositeTypes>;
}
