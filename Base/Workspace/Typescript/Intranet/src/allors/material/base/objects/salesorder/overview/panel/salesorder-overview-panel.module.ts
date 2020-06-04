import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
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

import { AllorsMaterialFileModule } from '../../../../../core/components/role/file';
import { AllorsMaterialInputModule } from '../../../../../core/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../core/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../core/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../core/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../core/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../core/components/role/textarea';
import { AllorsMaterialTableModule } from '../../../../../core/components/table';
import { AllorsMaterialFactoryFabModule } from '../../../../../core/components/factoryfab/factoryfab.module';

import { SalesOrderOverviewPanelComponent } from './salesorder-overview-panel.component';
export { SalesOrderOverviewPanelComponent } from './salesorder-overview-panel.component';

@NgModule({
  declarations: [
    SalesOrderOverviewPanelComponent,
  ],
  exports: [
    SalesOrderOverviewPanelComponent,
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
  ],
})
export class SalesOrderOverviewPanelModule { }
