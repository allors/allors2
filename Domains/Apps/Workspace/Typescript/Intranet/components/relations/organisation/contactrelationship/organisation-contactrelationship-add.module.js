"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const inline_module_1 = require("../../../inline.module");
const shared_module_1 = require("../../../shared.module");
const organisation_contactrelationship_add_component_1 = require("./organisation-contactrelationship-add.component");
var organisation_contactrelationship_add_component_2 = require("./organisation-contactrelationship-add.component");
exports.OrganisationContactrelationshipAddComponent = organisation_contactrelationship_add_component_2.OrganisationContactrelationshipAddComponent;
let OrganisationContactrelationshipAddModule = class OrganisationContactrelationshipAddModule {
};
OrganisationContactrelationshipAddModule = __decorate([
    core_1.NgModule({
        declarations: [
            organisation_contactrelationship_add_component_1.OrganisationContactrelationshipAddComponent,
        ],
        exports: [
            organisation_contactrelationship_add_component_1.OrganisationContactrelationshipAddComponent,
            inline_module_1.InlineModule,
            shared_module_1.SharedModule,
        ],
        imports: [
            inline_module_1.InlineModule,
            shared_module_1.SharedModule,
        ],
    })
], OrganisationContactrelationshipAddModule);
exports.OrganisationContactrelationshipAddModule = OrganisationContactrelationshipAddModule;
