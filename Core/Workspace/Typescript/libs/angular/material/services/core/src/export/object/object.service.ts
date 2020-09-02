import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { IObject, ISessionObject } from '@allors/domain/system';
import { Context } from '@allors/angular/services/core';
import { ObjectType } from '@allors/meta/system';

import { ObjectData } from './object.data';

@Injectable()
export abstract class ObjectService {
  abstract create(objectType: ObjectType, createData?: ObjectData): Observable<IObject>;

  abstract hasCreateControl(objectType: ObjectType, createData: ObjectData, context: Context);

  abstract edit(object: ISessionObject, createData?: ObjectData): Observable<IObject>;

  abstract hasEditControl(object: ISessionObject);
}
