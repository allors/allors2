import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material';

import { StaticComponent } from './static.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { StaticComponent } from './static.component';

@NgModule({
  declarations: [
    StaticComponent,
  ],
  exports: [
    StaticComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
  ],
})
export class StaticModule {
}
