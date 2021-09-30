import { PushRequestRole } from './PushRequestRole';

export interface PushRequestObject {
  i: string;
  v?: string;
  roles?: PushRequestRole[];
}
