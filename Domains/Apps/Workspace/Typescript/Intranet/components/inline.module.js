"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const person_inline_module_1 = require("./relations/person/person-inline.module");
const party_contactmechanism_emailaddress_inline_module_1 = require("./relations/party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-inline.module");
const party_contactmechanism_postaladdress_inline_module_1 = require("./relations/party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-inline.module");
const party_contactmechanism_telecommunicationsnumber_inline_module_1 = require("./relations/party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-inline.module");
const party_contactmechanism_webaddress_inline_module_1 = require("./relations/party/contactmechanism/webaddress/party-contactmechanism-webaddress-inline.module");
const APPS_INLINE_MODULES = [
    person_inline_module_1.PersonInlineModule,
];
let InlineModule = class InlineModule {
};
InlineModule = __decorate([
    core_1.NgModule({
        exports: [
            person_inline_module_1.PersonInlineModule,
            party_contactmechanism_emailaddress_inline_module_1.PartyContactMechanismEmailAddressInlineModule, party_contactmechanism_postaladdress_inline_module_1.PartyContactMechanismPostalAddressInlineModule, party_contactmechanism_telecommunicationsnumber_inline_module_1.PartyContactMechanismTelecommunicationsNumberInlineModule, party_contactmechanism_webaddress_inline_module_1.PartyContactMechanismInlineWebAddressModule,
        ],
        imports: [
            person_inline_module_1.PersonInlineModule,
        ],
    })
], InlineModule);
exports.InlineModule = InlineModule;
