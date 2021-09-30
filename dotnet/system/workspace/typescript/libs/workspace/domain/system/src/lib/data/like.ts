import { RoleType } from '@allors/workspace/meta/system';
import { ParameterizablePredicate } from './ParameterizablePredicate';

export interface Like extends ParameterizablePredicate {
  roleType: RoleType;
  value?: string;
}
