import { Component, EventEmitter, Input, OnDestroy , OnInit, Output } from "@angular/core";

import { ErrorService, Loaded, Scope, WorkspaceService } from "../../../../../../../angular";
import { ContactMechanismPurpose, ContactMechanismType, Enumeration, PartyContactMechanism, TelecommunicationsNumber } from "../../../../../../../domain";
import { PullRequest, Query } from "../../../../../../../framework";
import { MetaDomain } from "../../../../../../../meta";

@Component({
  selector: "party-contactmechanism-telecommunicationsnumber",
  templateUrl: "./party-contactmechanism-telecommunicationsnumber-inline.component.html",
})
export class PartyContactMechanismTelecommunicationsNumberInlineComponent implements OnInit, OnDestroy {

  @Output()
  public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public contactMechanismPurposes: Enumeration[];
  public contactMechanismTypes: ContactMechanismType[];

  public partyContactMechanism: PartyContactMechanism;
  public telecommunicationsNumber: TelecommunicationsNumber;

  public m: MetaDomain;

  constructor(private workspaceService: WorkspaceService, private errorService: ErrorService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    const queries: Query[] = [
      new Query(
        {
          name: "contactMechanismPurposes",
          objectType: this.m.ContactMechanismPurpose,
        }),
      new Query(
        {
          name: "contactMechanismTypes",
          objectType: this.m.ContactMechanismType,
        }),
      ];

    this.scope
      .load("Pull", new PullRequest({ queries }))
      .subscribe((loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];
        this.contactMechanismTypes = loaded.collections.contactMechanismTypes as ContactMechanismType[];

        this.partyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
        this.telecommunicationsNumber = this.scope.session.create("TelecommunicationsNumber") as TelecommunicationsNumber;
        this.partyContactMechanism.ContactMechanism = this.telecommunicationsNumber;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public ngOnDestroy(): void {
    if (!!this.partyContactMechanism) {
      this.scope.session.delete(this.partyContactMechanism);
      this.scope.session.delete(this.telecommunicationsNumber);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.saved.emit(this.partyContactMechanism);

    this.partyContactMechanism = undefined;
    this.telecommunicationsNumber = undefined;
  }
}
