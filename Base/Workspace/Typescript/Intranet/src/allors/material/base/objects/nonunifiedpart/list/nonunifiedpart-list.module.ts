import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule } from '../../../..';

import { NonUnifiedPartListComponent } from './nonunifiedpart-list.component';
export { NonUnifiedPartListComponent } from './nonunifiedpart-list.component';

@NgModule({
  declarations: [
    NonUnifiedPartListComponent,
  ],
  exports: [
    NonUnifiedPartListComponent,
  ],
  imports: [
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
  ],
  entryComponents: [
  ]
})
export class NonUnifiedPartListModule { }
