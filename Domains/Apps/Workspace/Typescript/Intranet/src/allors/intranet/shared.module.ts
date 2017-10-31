import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";

const ANGULAR_MODULES: any[] = [
  HttpModule, FormsModule, ReactiveFormsModule, RouterModule,
];

import {
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
} from "@angular/material";

const MATERIAL_MODULES: any[] = [
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
];

import {
  CovalentChipsModule, CovalentCommonModule, CovalentDataTableModule,
  CovalentDialogsModule, CovalentFileModule, CovalentLayoutModule,
  CovalentLoadingModule, CovalentMediaModule, CovalentMenuModule,
  CovalentNotificationsModule, CovalentPagingModule, CovalentSearchModule,
  CovalentStepsModule,
} from "@covalent/core";

const COVALENT_MODULES: any[] = [
  CovalentChipsModule, CovalentCommonModule, CovalentDataTableModule,
  CovalentDialogsModule, CovalentFileModule, CovalentLayoutModule,
  CovalentLoadingModule, CovalentMediaModule, CovalentMenuModule,
  CovalentNotificationsModule, CovalentPagingModule, CovalentSearchModule,
  CovalentStepsModule,
];

import { NgxChartsModule } from "@swimlane/ngx-charts";

const CHART_MODULES: any[] = [
  NgxChartsModule,
];

import { AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule } from "@baseMaterial/index";

const BASE_MATERIAL_MODULES: any[] = [
  AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule,
];

import { ChipsModule, MediaUploadModule } from "@baseCovalent/index";

const BASE_COVALENT_MODULES: any[] = [
  ChipsModule, MediaUploadModule,
];

@NgModule({
  exports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    COVALENT_MODULES,
    CHART_MODULES,

    BASE_MATERIAL_MODULES,
    BASE_COVALENT_MODULES,
  ],
  imports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    COVALENT_MODULES,
    CHART_MODULES,

    BASE_MATERIAL_MODULES,
    BASE_COVALENT_MODULES,
  ],
})
export class SharedModule { }
