import { Response } from '../Response';

export interface PullResponse extends Response {
  namedCollections?: { [id: string]: string[]; };
  namedObjects?: { [id: string]: string; };
  namedValues?: { [id: string]: any; };

  accessControls?: string[][];
  objects?: string[][];
}
