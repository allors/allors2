import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialLocalisedMarkdownComponent } from './localisedmarkdown.component';
export { AllorsMaterialLocalisedMarkdownComponent } from './localisedmarkdown.component';

@NgModule({
  declarations: [
    AllorsMaterialLocalisedMarkdownComponent,
  ],
  exports: [
    AllorsMaterialLocalisedMarkdownComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatCardModule,
    MatGridListModule,
    MatInputModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialLocalisedMarkdownModule {
}
