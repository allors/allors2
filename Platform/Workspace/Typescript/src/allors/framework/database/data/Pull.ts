import { Extent } from './Extent';
import { Fetch } from './Fetch';
import { ISessionObject } from '../../workspace';
import { MetaObjectType } from '../../meta';

export class Pull {

  public extent: Extent;

  public object: ISessionObject | string;

  public fetches: Fetch[];

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
      fetches: this.fetches && this.fetches.map(v => new Fetch(Object.assign({}, v, {objectType}))),
    };
  }
}
