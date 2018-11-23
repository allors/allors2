import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatCheckboxModule, MatMenuModule, MatIconModule, MatButtonModule, MatPaginatorModule, MatSortModule } from '@angular/material';

import { AllorsMaterialTableComponent } from './table.component';
export { AllorsMaterialTableComponent } from './table.component';

@NgModule({
  declarations: [
    AllorsMaterialTableComponent,
  ],
  exports: [
    AllorsMaterialTableComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,

    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    MatMenuModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
  ],
})
export class AllorsMaterialTableModule {
}

export * from './Table';
export * from './TableRow';
export * from './Column';
