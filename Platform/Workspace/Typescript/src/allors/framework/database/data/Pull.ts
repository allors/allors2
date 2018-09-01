import { Extent } from './Extent';
import { Result } from './Result';
import { ISessionObject } from '../../workspace';

export class Pull {

  public extent: Extent;

  public object: ISessionObject;

  public results: Result[];

  constructor(fields?: Partial<Pull>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      extent: this.extent,
      object: this.object ? this.object.id : undefined,
      results: this.results,
    };
  }
}
