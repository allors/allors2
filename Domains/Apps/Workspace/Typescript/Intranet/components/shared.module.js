"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const forms_1 = require("@angular/forms");
const http_1 = require("@angular/http");
const router_1 = require("@angular/router");
const ANGULAR_MODULES = [
    http_1.HttpModule, forms_1.FormsModule, forms_1.ReactiveFormsModule, router_1.RouterModule,
];
const material_1 = require("@angular/material");
const MATERIAL_MODULES = [
    material_1.MatAutocompleteModule, material_1.MatButtonModule, material_1.MatCardModule, material_1.MatCheckboxModule, material_1.MatDatepickerModule,
    material_1.MatIconModule, material_1.MatInputModule, material_1.MatListModule, material_1.MatMenuModule,
    material_1.MatNativeDateModule, material_1.MatRadioModule, material_1.MatSelectModule,
    material_1.MatSidenavModule, material_1.MatSliderModule, material_1.MatSlideToggleModule,
    material_1.MatSnackBarModule, material_1.MatTabsModule, material_1.MatToolbarModule, material_1.MatTooltipModule,
];
const core_2 = require("@covalent/core");
const COVALENT_MODULES = [
    core_2.CovalentChipsModule, core_2.CovalentCommonModule, core_2.CovalentDataTableModule,
    core_2.CovalentDialogsModule, core_2.CovalentFileModule, core_2.CovalentLayoutModule,
    core_2.CovalentLoadingModule, core_2.CovalentMediaModule, core_2.CovalentMenuModule,
    core_2.CovalentNotificationsModule, core_2.CovalentPagingModule, core_2.CovalentSearchModule,
    core_2.CovalentStepsModule,
];
const base_material_1 = require("@allors/base-material");
const BASE_MATERIAL_MODULES = [
    base_material_1.AutoCompleteModule, base_material_1.CheckboxModule, base_material_1.DatepickerModule, base_material_1.InputModule, base_material_1.LocalisedTextModule,
    base_material_1.RadioGroupModule, base_material_1.SelectModule, base_material_1.SliderModule, base_material_1.SlideToggleModule, base_material_1.StaticModule, base_material_1.TextAreaModule,
];
const base_covalent_1 = require("@allors/base-covalent");
const BASE_COVALENT_MODULES = [
    base_covalent_1.ChipsModule, base_covalent_1.MediaUploadModule,
];
let SharedModule = class SharedModule {
};
SharedModule = __decorate([
    core_1.NgModule({
        exports: [
            ANGULAR_MODULES,
            MATERIAL_MODULES,
            COVALENT_MODULES,
            BASE_MATERIAL_MODULES,
            BASE_COVALENT_MODULES,
        ],
        imports: [
            ANGULAR_MODULES,
            MATERIAL_MODULES,
            COVALENT_MODULES,
            BASE_MATERIAL_MODULES,
            BASE_COVALENT_MODULES,
        ],
    })
], SharedModule);
exports.SharedModule = SharedModule;
