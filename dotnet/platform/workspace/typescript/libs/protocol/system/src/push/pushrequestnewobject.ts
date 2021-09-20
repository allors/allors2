import { PushRequestRole } from './PushRequestRole';

export interface PushRequestNewObject {
  ni: string;
  t: string;
  roles?: PushRequestRole[];
}
