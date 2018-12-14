import { Injectable, Inject } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Observable, EMPTY, throwError } from 'rxjs';

import { ObjectType, ObjectTypeRef, ISessionObject } from '../../../framework';
import { asObjectType, asObjectTypeId } from '../../../../allors/framework';

import { OBJECT_CREATE_TOKEN, OBJECT_EDIT_TOKEN } from './object.tokens';
import { CreateData, EditData, ObjectData } from './object.data';

@Injectable({
  providedIn: 'root',
})
export class ObjectService {

  constructor(
    public dialog: MatDialog,
    @Inject(OBJECT_CREATE_TOKEN) private createControlByObjectTypeId: { [id: string]: any },
    @Inject(OBJECT_EDIT_TOKEN) private editControlByObjectTypeId: { [id: string]: any }
  ) {
  }

  create(objectType: ObjectType | ObjectTypeRef, createData?: CreateData): Observable<ObjectData> {

    const data: CreateData = Object.assign({ objectType: asObjectType(objectType) }, createData);

    const component = this.createControlByObjectTypeId[data.objectType.id];
    if (component) {
      const dialogRef = this.dialog.open(component, {data});

      return dialogRef
        .afterClosed();
    }

    return throwError('Missing component');
  }

  hasCreateControl(objectType: ObjectType | ObjectTypeRef) {
    return !!this.createControlByObjectTypeId[asObjectTypeId(objectType)];
  }

  edit(object: ISessionObject): Observable<ObjectData> {

    const data: EditData = {
      id: object.id,
      objectType: object.objectType,
    };

    const component = this.editControlByObjectTypeId[object.objectType.id];
    if (component) {
      const dialogRef = this.dialog.open(component, { data });
      return dialogRef.afterClosed();
    }

    return throwError('Missing component');
  }

  hasEditControl(object: ISessionObject) {
    return !!this.editControlByObjectTypeId[object.objectType.id];
  }

}
