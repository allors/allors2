import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material';

import { AllorsMaterialLocalisedTextComponent } from './localisedtext.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AllorsMaterialLocalisedTextComponent } from './localisedtext.component';

@NgModule({
  declarations: [
    AllorsMaterialLocalisedTextComponent,
  ],
  exports: [
    AllorsMaterialLocalisedTextComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
  ],
})
export class AllorsMaterialLocalisedTextModule {
}
