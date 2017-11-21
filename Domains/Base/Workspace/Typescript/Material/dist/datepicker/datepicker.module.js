"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const common_1 = require("@angular/common");
const forms_1 = require("@angular/forms");
const material_1 = require("@angular/material");
const datepicker_component_1 = require("./datepicker.component");
var datepicker_component_2 = require("./datepicker.component");
exports.DatepickerComponent = datepicker_component_2.DatepickerComponent;
let DatepickerModule = class DatepickerModule {
};
DatepickerModule = __decorate([
    core_1.NgModule({
        declarations: [
            datepicker_component_1.DatepickerComponent,
        ],
        exports: [
            datepicker_component_1.DatepickerComponent,
        ],
        imports: [
            forms_1.FormsModule,
            common_1.CommonModule,
            material_1.MatInputModule,
            material_1.MatIconModule,
            material_1.MatDatepickerModule,
        ],
    })
], DatepickerModule);
exports.DatepickerModule = DatepickerModule;
