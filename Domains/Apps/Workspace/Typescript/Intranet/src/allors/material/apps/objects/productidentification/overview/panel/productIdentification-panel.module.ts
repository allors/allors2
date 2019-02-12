import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';

import { AllorsMaterialFileModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule } from '../../../../..';

import { ProductIdentificationsPanelComponent } from './productIdentification-panel.component';
export { ProductIdentificationsPanelComponent as ProductIdentificationsPanel } from './productIdentification-panel.component';

@NgModule({
  declarations: [
    ProductIdentificationsPanelComponent,
  ],
  exports: [
    ProductIdentificationsPanelComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,

    AllorsMaterialFactoryFabModule,
    AllorsMaterialFileModule,
    AllorsMaterialTableModule,
  ],
})
export class ProductIdentificationPanelModule { }
