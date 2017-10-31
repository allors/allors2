import { NgModule } from "@angular/core";

import { PersonInlineModule } from "./apps/relations/people/person/person-inline.module";

import { PartyContactMechanismEmailAddressInlineModule } from "./apps/relations/party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-inline.module";
import { PartyContactMechanismPostalAddressInlineModule } from "./apps/relations/party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-inline.module";
import { PartyContactMechanismTelecommunicationsNumberInlineModule } from "./apps/relations/party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-inline.module";
import { PartyContactMechanismInlineWebAddressModule } from "./apps/relations/party/contactmechanism/webaddress/party-contactmechanism-webaddress-inline.module";

const APPS_INLINE_MODULES: any[] = [
  PersonInlineModule,
];

@NgModule({
  exports: [
    PersonInlineModule,

    PartyContactMechanismEmailAddressInlineModule, PartyContactMechanismPostalAddressInlineModule, PartyContactMechanismTelecommunicationsNumberInlineModule, PartyContactMechanismInlineWebAddressModule,
  ],
  imports: [
    PersonInlineModule,
  ],
})
export class InlineModule { }
