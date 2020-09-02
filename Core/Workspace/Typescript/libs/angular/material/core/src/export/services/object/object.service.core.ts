import { Injectable, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, throwError } from 'rxjs';

import { IObject, ISessionObject } from '@allors/domain/system';
import { Context } from '@allors/angular/services/core';
import { ObjectType } from '@allors/meta/system';
import { ObjectService, OBJECT_CREATE_TOKEN, OBJECT_EDIT_TOKEN, ObjectData } from '@allors/angular/material/services/core';


@Injectable()
export class ObjectServiceCore extends ObjectService {

  constructor(
    public dialog: MatDialog,
    @Inject(OBJECT_CREATE_TOKEN) private createControlByObjectTypeId: { [id: string]: any },
    @Inject(OBJECT_EDIT_TOKEN) private editControlByObjectTypeId: { [id: string]: any }
  ) {
    super();
  }

  create(objectType: ObjectType, createData?: ObjectData): Observable<IObject> {

    const data: ObjectData = Object.assign({ objectType }, createData);

    const component = this.createControlByObjectTypeId[objectType.id];
    if (component) {
      const dialogRef = this.dialog.open(component, { data, minWidth: '80vw', maxHeight: '90vh' });

      return dialogRef
        .afterClosed();
    }

    return throwError('Missing component');
  }

  hasCreateControl(objectType: ObjectType, createData: ObjectData, context: Context) {
    const createControl = this.createControlByObjectTypeId[objectType.id];
    if (createControl) {
      if (createControl.canCreate) {
        return createControl.canCreate(createData, context);
      }

      return true;
    }

    return false;
  }

  edit(object: ISessionObject, createData?: ObjectData): Observable<IObject> {

    const data: ObjectData = Object.assign({
      id: object.id,
      objectType: object.objectType,
    }, createData);

    const component = this.editControlByObjectTypeId[object.objectType.id];
    if (component) {
      const dialogRef = this.dialog.open(component, { data, minWidth: '80vw', maxHeight: '90vh' });
      return dialogRef.afterClosed();
    }

    return throwError('Missing component');
  }

  hasEditControl(object: ISessionObject) {
    return !!this.editControlByObjectTypeId[object.objectType.id];
  }

}
