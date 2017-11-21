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
const radiogroup_component_1 = require("./radiogroup.component");
var radiogroup_component_2 = require("./radiogroup.component");
exports.RadioGroupComponent = radiogroup_component_2.RadioGroupComponent;
let RadioGroupModule = class RadioGroupModule {
};
RadioGroupModule = __decorate([
    core_1.NgModule({
        declarations: [
            radiogroup_component_1.RadioGroupComponent,
        ],
        exports: [
            radiogroup_component_1.RadioGroupComponent,
        ],
        imports: [
            forms_1.FormsModule,
            common_1.CommonModule,
            material_1.MatInputModule,
            material_1.MatRadioModule,
        ],
    })
], RadioGroupModule);
exports.RadioGroupModule = RadioGroupModule;
