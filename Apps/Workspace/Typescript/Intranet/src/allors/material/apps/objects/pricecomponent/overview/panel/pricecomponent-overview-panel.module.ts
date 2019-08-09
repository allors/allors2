import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatOptionModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AllorsMaterialFactoryFabModule } from '../../../../../core/components/factoryfab/factoryfab.module';
import { AllorsMaterialFileModule } from '../../../../../core/components/role/file';
import { AllorsMaterialInputModule } from '../../../../../core/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../core/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../core/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../core/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../core/components/role/static';
import { AllorsMaterialTableModule } from '../../../../../core/components/table';
import { AllorsMaterialTextAreaModule } from '../../../../../core/components/role/textarea';

import { PriceComponentOverviewPanelComponent } from './pricecomponent-overview-panel.component';
export { PriceComponentOverviewPanelComponent } from './pricecomponent-overview-panel.component';

@NgModule({
  declarations: [
    PriceComponentOverviewPanelComponent,
  ],
  exports: [
    PriceComponentOverviewPanelComponent,
  ],
  imports: [
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTableModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
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
    MatOptionModule,
    MatButtonToggleModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PriceComponentOverviewPanelModule { }
