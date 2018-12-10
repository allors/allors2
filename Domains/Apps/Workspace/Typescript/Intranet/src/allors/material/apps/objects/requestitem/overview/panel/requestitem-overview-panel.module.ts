import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatButtonToggleModule } from '@angular/material';

import { AllorsMaterialFileModule, AllorsMaterialTableModule } from '../../../../..';

import { RequestItemOverviewPanelComponent } from './requestitem-overview-panel.component';
export { RequestItemOverviewPanelComponent } from './requestitem-overview-panel.component';

@NgModule({
  declarations: [
    RequestItemOverviewPanelComponent,
  ],
  exports: [
    RequestItemOverviewPanelComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatButtonModule,
    MatButtonToggleModule,
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
    MatButtonToggleModule,
    MatOptionModule,
    AllorsMaterialFileModule,
    AllorsMaterialTableModule,
  ],
})
export class RequestItemOverviewPanelModule { }
