import { Extent } from './Extent';
import { ISessionObject } from '../../workspace';
import { Result } from './result';

export class Pull {

  public extentRef: string;

  public extent: Extent;

  public object: ISessionObject | string;

  public results: Result[];

  constructor(fields?: Partial<Pull>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    const sessionObject = this.object as ISessionObject;

    return {
      extentRef: this.extentRef,
      extent: this.extent,
      object: sessionObject && sessionObject.id ? sessionObject.id : this.object,
      results: this.results,
    };
  }
}
