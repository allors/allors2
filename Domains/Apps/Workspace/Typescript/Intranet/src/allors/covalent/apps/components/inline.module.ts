import { NgModule } from "@angular/core";

import { BrandInlineModule } from "./catalogues/brand/brand-inline.module";
import { ModelInlineModule } from "./catalogues/model/model-inline.module";
import { PartyContactMechanismEmailAddressInlineModule } from "./relations/party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-inline.module";
import { PartyContactMechanismInlineModule } from "./relations/party/contactmechanism/party-contactmechanism-inline.module";
import { PartyContactMechanismPostalAddressInlineModule } from "./relations/party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-inline.module";
import { PartyContactMechanismTelecommunicationsNumberInlineModule } from "./relations/party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-inline.module";
import { PartyContactMechanismInlineWebAddressModule } from "./relations/party/contactmechanism/webaddress/party-contactmechanism-webaddress-inline.module";
import { PersonInlineModule } from "./relations/person/person-inline.module";

const APPS_INLINE_MODULES: any[] = [
  BrandInlineModule,
  ModelInlineModule,
  PersonInlineModule,
];

@NgModule({
  exports: [
    BrandInlineModule, ModelInlineModule, PersonInlineModule,
    PartyContactMechanismInlineModule, PartyContactMechanismEmailAddressInlineModule, PartyContactMechanismPostalAddressInlineModule, PartyContactMechanismTelecommunicationsNumberInlineModule, PartyContactMechanismInlineWebAddressModule,
  ],
  imports: [
    BrandInlineModule, ModelInlineModule, PersonInlineModule,
  ],
})
export class InlineModule { }
