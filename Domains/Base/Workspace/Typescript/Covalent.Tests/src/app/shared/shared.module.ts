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
  CovalentNotificationsModule, CovalentPagingModule, CovalentSearchModule,
  CovalentStepsModule,
} from "@covalent/core";

import { AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule } from "@allors/base-material";

import { ChipsModule, MediaUploadModule } from "@allors/base-covalent";

import {
    DashboardModule,
    OrganisationModule, OrganisationOverviewModule, OrganisationsModule,
    PeopleModule, PersonModule, PersonOverviewModule,
    RelationsModule,
 } from "../../allors/covalent/custom/relations";

const ANGULAR_MODULES: any[] = [
  HttpModule, FormsModule, ReactiveFormsModule,
];

const MATERIAL_MODULES: any[] = [
  MatButtonModule, MatCardModule, MatDatepickerModule, MatIconModule, MatAutocompleteModule,
  MatListModule, MatMenuModule, MatTooltipModule,
  MatSlideToggleModule, MatInputModule, MatCheckboxModule,
  MatToolbarModule, MatSnackBarModule, MatSidenavModule,
  MatTabsModule, MatSelectModule, MatRadioModule, MatSliderModule,
];

const COVALENT_MODULES: any[] = [
  CovalentDataTableModule, CovalentMediaModule, CovalentLoadingModule,
  CovalentNotificationsModule, CovalentLayoutModule, CovalentMenuModule,
  CovalentPagingModule, CovalentSearchModule, CovalentStepsModule,
  CovalentCommonModule, CovalentDialogsModule, CovalentFileModule,
  CovalentChipsModule,
];

const BASE_MATERIAL_MODULES: any[] = [
  AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule,
];

const BASE_COVALENT_MODULES: any[] = [
  ChipsModule, MediaUploadModule,
];

const RELATIONS_MODULES: any[] = [
  DashboardModule,
  OrganisationModule, OrganisationOverviewModule, OrganisationsModule,
  PeopleModule, PersonModule, PersonOverviewModule,
  RelationsModule,
];

@NgModule({
  exports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    COVALENT_MODULES,

    BASE_MATERIAL_MODULES,
    BASE_COVALENT_MODULES,
    RELATIONS_MODULES,
  ],
})
export class SharedModule { }
