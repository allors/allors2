import { Response } from '../Response';

export interface PullResponse extends Response {
  accessControls?: string[][];
  namedCollections?: { [id: string]: string[]; };
  namedObjects?: { [id: string]: string; };
  namedValues?: { [id: string]: any; };
  objects?: string[][];
}
