"use strict";
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(require("./overview.module"));
__export(require("./dashboard/dashboard.module"));
__export(require("./person/person-overview.module"));
__export(require("./person/person.module"));
__export(require("./person/people-export.module"));
__export(require("./person/people-overview.module"));
__export(require("./organisation/organisations-overview.module"));
__export(require("./organisation/organisation-overview.module"));
__export(require("./organisation/organisation.module"));
__export(require("./organisation/contactrelationship/organisation-contactrelationship-add.module"));
__export(require("./organisation/contactrelationship/organisation-contactrelationship-edit.module"));
__export(require("./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-add.module"));
__export(require("./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-edit.module"));
__export(require("./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-add.module"));
__export(require("./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-edit.module"));
__export(require("./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-add.module"));
__export(require("./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-edit.module"));
__export(require("./party/contactmechanism/webaddress/party-contactmechanism-webaddress-add.module"));
__export(require("./party/contactmechanism/webaddress/party-contactmechanism-webaddress-edit.module"));
__export(require("./party/communicationevent/emailcommunication/party-communicationevent-emailcommunication.module"));
__export(require("./party/communicationevent/facetofacecommunication/party-communicationevent-facetofacecommunication.module"));
__export(require("./party/communicationevent/lettercorrespondence/party-communicationevent-lettercorrespondence.module"));
__export(require("./party/communicationevent/phonecommunication/party-communicationevent-phonecommunication.module"));
__export(require("./party/communicationevent/worktask/party-communicationevent-worktask.module"));
__export(require("./communicationevent/communicationevent-overview.module"));
__export(require("./communicationevent/communicationevents-overview.module"));
const overview_module_1 = require("./overview.module");
const dashboard_module_1 = require("./dashboard/dashboard.module");
const people_export_module_1 = require("./person/people-export.module");
const people_overview_module_1 = require("./person/people-overview.module");
const person_overview_module_1 = require("./person/person-overview.module");
const person_module_1 = require("./person/person.module");
const organisation_contactrelationship_add_module_1 = require("./organisation/contactrelationship/organisation-contactrelationship-add.module");
const organisation_contactrelationship_edit_module_1 = require("./organisation/contactrelationship/organisation-contactrelationship-edit.module");
const organisation_overview_module_1 = require("./organisation/organisation-overview.module");
const organisation_module_1 = require("./organisation/organisation.module");
const organisations_overview_module_1 = require("./organisation/organisations-overview.module");
const party_contactmechanism_emailaddress_add_module_1 = require("./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-add.module");
const party_contactmechanism_emailaddress_edit_module_1 = require("./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-edit.module");
const party_contactmechanism_postaladdress_add_module_1 = require("./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-add.module");
const party_contactmechanism_postaladdress_edit_module_1 = require("./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-edit.module");
const party_contactmechanism_telecommunicationsnumber_add_module_1 = require("./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-add.module");
const party_contactmechanism_telecommunicationsnumber_edit_module_1 = require("./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-edit.module");
const party_contactmechanism_webaddress_add_module_1 = require("./party/contactmechanism/webaddress/party-contactmechanism-webaddress-add.module");
const party_contactmechanism_webaddress_edit_module_1 = require("./party/contactmechanism/webaddress/party-contactmechanism-webaddress-edit.module");
const party_communicationevent_emailcommunication_module_1 = require("./party/communicationevent/emailcommunication/party-communicationevent-emailcommunication.module");
const party_communicationevent_facetofacecommunication_module_1 = require("./party/communicationevent/facetofacecommunication/party-communicationevent-facetofacecommunication.module");
const party_communicationevent_lettercorrespondence_module_1 = require("./party/communicationevent/lettercorrespondence/party-communicationevent-lettercorrespondence.module");
const party_communicationevent_phonecommunication_module_1 = require("./party/communicationevent/phonecommunication/party-communicationevent-phonecommunication.module");
const party_communicationevent_worktask_module_1 = require("./party/communicationevent/worktask/party-communicationevent-worktask.module");
const communicationevent_overview_module_1 = require("./communicationevent/communicationevent-overview.module");
const communicationevents_overview_module_1 = require("./communicationevent/communicationevents-overview.module");
exports.Modules = [
    overview_module_1.OverviewModule,
    dashboard_module_1.DashboardModule,
    people_export_module_1.PeopleExportModule, people_overview_module_1.PeopleOverviewModule, person_overview_module_1.PersonOverviewModule, person_module_1.PersonModule,
    organisation_contactrelationship_add_module_1.OrganisationContactrelationshipAddModule, organisation_contactrelationship_edit_module_1.OrganisationContactrelationshipEditModule, organisation_overview_module_1.OrganisationOverviewModule, organisation_module_1.OrganisationModule, organisations_overview_module_1.OrganisationsOverviewModule,
    party_contactmechanism_emailaddress_add_module_1.PartyContactMechanismEmailAddressAddModule, party_contactmechanism_emailaddress_edit_module_1.PartyContactMechanismEmailAddressEditModule, party_contactmechanism_postaladdress_add_module_1.PartyContactMechanismAddPostalAddressModule, party_contactmechanism_postaladdress_edit_module_1.PartyContactMechanismPostalAddressEditModule, party_contactmechanism_telecommunicationsnumber_add_module_1.PartyContactMechanismTelecommunicationsNumberAddModule, party_contactmechanism_telecommunicationsnumber_edit_module_1.PartyContactMechanismTelecommunicationsNumberEditModule, party_contactmechanism_webaddress_add_module_1.PartyContactMechanismAddWebAddressModule, party_contactmechanism_webaddress_edit_module_1.PartyContactMechanismEditWebAddressModule,
    party_communicationevent_emailcommunication_module_1.PartyCommunicationEventEmailCommunicationModule, party_communicationevent_facetofacecommunication_module_1.PartyCommunicationEventFaceToFaceCommunicationModule, party_communicationevent_lettercorrespondence_module_1.PartyCommunicationEventLetterCorrespondenceModule, party_communicationevent_phonecommunication_module_1.PartyCommunicationEventPhoneCommunicationModule, party_communicationevent_worktask_module_1.PartyCommunicationEventWorkTaskModule,
    communicationevents_overview_module_1.CommunicationEventsOverviewModule,
    communicationevent_overview_module_1.CommunicationEventOverviewModule,
];
