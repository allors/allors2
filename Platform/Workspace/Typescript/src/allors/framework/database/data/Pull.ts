import { Extent } from './Extent';
import { Result } from './Result';
import { ISessionObject } from '../../workspace';
import { MetaObjectType } from '../../meta';

export class Pull {

  public extent: Extent;

  public object: ISessionObject | string;

  public results: Result[];

  constructor(fields?: Partial<Pull>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    let extentObjectType = this.extent ? this.extent.objectType : undefined;
    let objectType = extentObjectType && (extentObjectType as MetaObjectType)._objectType ? (extentObjectType as MetaObjectType)._objectType : undefined;

    const sessionObject = this.object as ISessionObject;
    if (sessionObject && sessionObject.objectType) {
      objectType = sessionObject.objectType;
    }

    return {
      extent: this.extent,
      object: sessionObject && sessionObject.id ? sessionObject.id : this.object,
      results: this.results && this.results.map(v => new Result(Object.assign({}, v, {objectType}))),
    };
  }
}
