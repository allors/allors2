import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule } from '../../../..';

import { SerialisedItemCharacteristicListComponent } from './serialiseditemcharacteristic-list.component';
export { SerialisedItemCharacteristicListComponent } from './serialiseditemcharacteristic-list.component';

@NgModule({
  declarations: [
    SerialisedItemCharacteristicListComponent,
  ],
  exports: [
    SerialisedItemCharacteristicListComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ],
})
export class SerialisedItemCharacteristicListModule { }
