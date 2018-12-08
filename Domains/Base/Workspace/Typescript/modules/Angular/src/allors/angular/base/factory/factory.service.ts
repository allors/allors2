import { Injectable, InjectionToken, Inject, Injector } from '@angular/core';
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

  add(objectType: ObjectType | ObjectTypeRef | string) {

    const objectTypeId = objectType instanceof ObjectType ? objectType.id : (objectType as ObjectTypeRef).objectType ? (objectType as ObjectTypeRef).objectType.id : objectType as string;

    const factoryItem = this.factoryConfig.items.find((v) => v.id === objectTypeId);
    if (factoryItem) {
      const dialogRef = this.dialog.open(factoryItem.component);

      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed');
      });

    }
  }
}
