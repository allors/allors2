import { Response } from '../Response';
import { SyncResponseObject } from './SyncResponseObject';

export interface SyncResponse extends Response {
  accessControls?: string[][];
  objects: SyncResponseObject[];
}
