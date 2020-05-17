import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatExpansionModule } from '@angular/material/expansion';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialMarkdownComponent } from './markdown.component';
export { AllorsMaterialMarkdownComponent } from './markdown.component';

@NgModule({
  declarations: [AllorsMaterialMarkdownComponent],
  exports: [AllorsMaterialMarkdownComponent],
  imports: [
    FormsModule,
    CommonModule,
    MatCardModule,
    MatGridListModule,
    MatInputModule,
    MatExpansionModule,
    AllorsFocusModule,
  ],
})
export class AllorsMaterialMarkdownModule {}
