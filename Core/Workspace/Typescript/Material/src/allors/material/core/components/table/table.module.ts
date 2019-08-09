import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';

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
    MatInputModule,
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
