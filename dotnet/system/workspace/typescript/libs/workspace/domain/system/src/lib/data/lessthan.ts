import { RoleType } from '@allors/workspace/meta/system';
import { UnitTypes } from '../Types';
import { ParameterizablePredicate } from './ParameterizablePredicate';

export interface LessThan extends ParameterizablePredicate {
  roleType: RoleType;
  value?: UnitTypes;
}
