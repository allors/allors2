import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import {
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
} from "@angular/material";
import {
  CovalentChipsModule, CovalentCommonModule, CovalentDataTableModule,
  CovalentDialogsModule, CovalentFileModule, CovalentLayoutModule,
  CovalentLoadingModule, CovalentMediaModule, CovalentMenuModule,
  CovalentNotificationsModule, CovalentPagingModule, CovalentSearchModule, CovalentStepsModule,
} from "@covalent/core";
import { NgxChartsModule } from "@swimlane/ngx-charts";

const ANGULAR_MODULES: any[] = [
  HttpModule, FormsModule, ReactiveFormsModule, MatNativeDateModule,
];

const MATERIAL_MODULES: any[] = [
  MatButtonModule, MatCardModule, MatDatepickerModule, MatIconModule, MatAutocompleteModule,
  MatListModule, MatMenuModule, MatTooltipModule,
  MatSlideToggleModule, MatInputModule, MatCheckboxModule,
  MatToolbarModule, MatSnackBarModule, MatSidenavModule,
  MatTabsModule, MatSelectModule, MatRadioModule, MatSliderModule,
];

const COVALENT_MODULES: any[] = [
  CovalentChipsModule, CovalentDataTableModule, CovalentMediaModule, CovalentLoadingModule,
  CovalentNotificationsModule, CovalentLayoutModule, CovalentMenuModule,
  CovalentPagingModule, CovalentSearchModule, CovalentStepsModule,
  CovalentCommonModule, CovalentDialogsModule, CovalentFileModule, CovalentChipsModule,
];

const CHART_MODULES: any[] = [
  NgxChartsModule,
];

@NgModule({
  declarations: [
  ],
  exports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    COVALENT_MODULES,
    CHART_MODULES,
  ],
  imports: [
    CommonModule,
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    COVALENT_MODULES,
    CHART_MODULES,
  ],
})
export class SharedModule { }
