"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const inline_module_1 = require("../../inline.module");
const shared_module_1 = require("../../shared.module");
const productcharacteristic_component_1 = require("./productcharacteristic.component");
var productcharacteristic_component_2 = require("./productcharacteristic.component");
exports.ProductCharacteristicComponent = productcharacteristic_component_2.ProductCharacteristicComponent;
let ProductCharacteristicModule = class ProductCharacteristicModule {
};
ProductCharacteristicModule = __decorate([
    core_1.NgModule({
        declarations: [
            productcharacteristic_component_1.ProductCharacteristicComponent,
        ],
        exports: [
            productcharacteristic_component_1.ProductCharacteristicComponent,
            inline_module_1.InlineModule,
            shared_module_1.SharedModule,
        ],
        imports: [
            inline_module_1.InlineModule,
            shared_module_1.SharedModule,
        ],
    })
], ProductCharacteristicModule);
exports.ProductCharacteristicModule = ProductCharacteristicModule;
