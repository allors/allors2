import { Injectable, Component } from '@angular/core';
import { MatDialog } from '@angular/material';

import { ObjectType, ObjectTypeRef } from '../../../framework';
import { FactoryConfig } from './FactoryConfig';

@Injectable({
  providedIn: 'root',
})
export class FactoryService {

  constructor(
    public dialog: MatDialog,
    private factoryConfig: FactoryConfig
  ) {
  }

  create(objectType: ObjectType | ObjectTypeRef) {

    const component = this.component(objectType);
    if (component) {
      const dialogRef = this.dialog.open(component);

      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed');
      });

    }
  }

  hasFactory(objectType: ObjectType | ObjectTypeRef) {
    return !!this.component(objectType);
  }

  private component(objectType: ObjectType | ObjectTypeRef): any {

    const objectTypeId = objectType instanceof ObjectType ? objectType.id : (objectType as ObjectTypeRef).objectType.id;
    const factoryItem = this.factoryConfig.items.find((v) => v.id === objectTypeId);

    if (factoryItem) {
      return factoryItem.component;
    }
  }
}
