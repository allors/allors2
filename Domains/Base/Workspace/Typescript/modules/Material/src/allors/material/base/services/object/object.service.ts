import { Injectable, Inject } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Observable, throwError } from 'rxjs';

import { ObjectType, ISessionObject } from '../../../../framework';

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

  create(objectType: ObjectType, createData?: CreateData): Observable<ObjectData> {

    const data: CreateData = Object.assign({ objectType }, createData);

    const component = this.createControlByObjectTypeId[data.objectType.id];
    if (component) {
      const dialogRef = this.dialog.open(component, {data, minWidth: '80vw'});

      return dialogRef
        .afterClosed();
    }

    return throwError('Missing component');
  }

  hasCreateControl(objectType: ObjectType) {
    return !!this.createControlByObjectTypeId[objectType.id];
  }

  edit(object: ISessionObject): Observable<ObjectData> {

    const data: EditData = {
      id: object.id,
      objectType: object.objectType,
    };

    const component = this.editControlByObjectTypeId[object.objectType.id];
    if (component) {
      const dialogRef = this.dialog.open(component, { data, minWidth: '80vw' });
      return dialogRef.afterClosed();
    }

    return throwError('Missing component');
  }

  hasEditControl(object: ISessionObject) {
    return !!this.editControlByObjectTypeId[object.objectType.id];
  }

}
