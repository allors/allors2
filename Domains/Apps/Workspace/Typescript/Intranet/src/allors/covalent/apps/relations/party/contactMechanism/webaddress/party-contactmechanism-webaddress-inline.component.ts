import { Component, EventEmitter, Input, OnDestroy , OnInit, Output } from "@angular/core";

import { ErrorService, Loaded, Scope, WorkspaceService } from "../../../../../../angular";
import { ContactMechanismPurpose, PartyContactMechanism, WebAddress } from "../../../../../../domain";
import { PullRequest, Query } from "../../../../../../framework";
import { MetaDomain } from "../../../../../../meta";

@Component({
  selector: "party-contactmechanism-webAddress",
  templateUrl: "./party-contactmechanism-webaddress-inline.component.html",
})
export class PartyContactMechanismInlineWebAddressComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public webAddress: WebAddress;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanismPurposes: ContactMechanismPurpose[];

  public m: MetaDomain;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
  ) {
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    const query: Query[] = [
      new Query(
        {
          name: "contactMechanismPurposes",
          objectType: this.m.ContactMechanismPurpose,
        }),
    ];

    this.scope.load("Pull", new PullRequest({ query })).subscribe(
      (loaded: Loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];
        this.partyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
        this.webAddress = this.scope.session.create("WebAddress") as WebAddress;
        this.partyContactMechanism.ContactMechanism = this.webAddress;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public ngOnDestroy(): void {
    // if(!this.isSaved){
    //   this.partyContactMechanism.delete();
    //   this.webAddress.delete();
    // }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
      this.saved.emit(this.partyContactMechanism);
  }
}
