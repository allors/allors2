import { SyncResponseRole } from './SyncResponseRole';

export interface SyncResponseObject {
  i: string;
  t: string;
  v: string;
  a: string;
  d: string;
  r?: SyncResponseRole[];
}
