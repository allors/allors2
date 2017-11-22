"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const common_1 = require("@angular/common");
const core_1 = require("@angular/core");
const forms_1 = require("@angular/forms");
const material_1 = require("@angular/material");
const core_2 = require("@covalent/core");
const chips_component_1 = require("./chips.component");
var chips_component_2 = require("./chips.component");
exports.ChipsComponent = chips_component_2.ChipsComponent;
let ChipsModule = class ChipsModule {
};
ChipsModule = __decorate([
    core_1.NgModule({
        declarations: [
            chips_component_1.ChipsComponent,
        ],
        exports: [
            chips_component_1.ChipsComponent,
        ],
        imports: [
            common_1.CommonModule,
            forms_1.FormsModule,
            material_1.MatInputModule,
            core_2.CovalentChipsModule,
        ],
    })
], ChipsModule);
exports.ChipsModule = ChipsModule;
