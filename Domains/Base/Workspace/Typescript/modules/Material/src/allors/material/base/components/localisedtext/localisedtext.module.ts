import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material';

import { LocalisedTextComponent } from './localisedtext.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { LocalisedTextComponent } from './localisedtext.component';

@NgModule({
  declarations: [
    LocalisedTextComponent,
  ],
  exports: [
    LocalisedTextComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
  ],
})
export class LocalisedTextModule {
}
