import { Extent } from './Extent';
import { PullArgument } from './PullArgument';
import { PullResult } from './PullResult';

export class PullExtent {

  public id: string;

  public name: string;

  public extent: Extent;

  public arguments: PullArgument[];

  public results: PullResult[];

  constructor(fields?: Partial<PullExtent>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      name: this.name,
      extent: this.extent
    };
  }
}
