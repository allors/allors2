import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatAutocompleteModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from '@angular/material';

import { StaticModule } from '../../../material';

import { RelationsComponent } from './relations.component';
export { RelationsComponent } from './relations.component';

@NgModule({
  declarations: [
    RelationsComponent,
  ],
  exports: [
    RelationsComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatAutocompleteModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    StaticModule,
  ],
})
export class RelationsModule {}
