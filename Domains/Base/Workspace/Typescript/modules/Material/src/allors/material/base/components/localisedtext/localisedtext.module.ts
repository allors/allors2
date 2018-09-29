import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../angular';

import { AllorsMaterialLocalisedTextComponent } from './localisedtext.component';
export { AllorsMaterialLocalisedTextComponent } from './localisedtext.component';

@NgModule({
  declarations: [
    AllorsMaterialLocalisedTextComponent,
  ],
  exports: [
    AllorsMaterialLocalisedTextComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialLocalisedTextModule {
}
