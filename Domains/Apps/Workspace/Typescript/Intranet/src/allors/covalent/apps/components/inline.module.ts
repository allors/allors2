import { NgModule } from "@angular/core";

import { PersonInlineModule } from "./relations/person/person-inline.module";

import { PartyContactMechanismEmailAddressInlineModule } from "./relations/party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-inline.module";
import { PartyContactMechanismInlineModule } from "./relations/party/contactmechanism/party-contactmechanism-inline.module";
import { PartyContactMechanismPostalAddressInlineModule } from "./relations/party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-inline.module";
import { PartyContactMechanismTelecommunicationsNumberInlineModule } from "./relations/party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-inline.module";
import { PartyContactMechanismInlineWebAddressModule } from "./relations/party/contactmechanism/webaddress/party-contactmechanism-webaddress-inline.module";

const APPS_INLINE_MODULES: any[] = [
  PersonInlineModule,
];

@NgModule({
  exports: [
    PersonInlineModule,
    PartyContactMechanismInlineModule, PartyContactMechanismEmailAddressInlineModule, PartyContactMechanismPostalAddressInlineModule, PartyContactMechanismTelecommunicationsNumberInlineModule, PartyContactMechanismInlineWebAddressModule,
  ],
  imports: [
    PersonInlineModule,
  ],
})
export class InlineModule { }
