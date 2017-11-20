import { Component, EventEmitter, Input, OnInit , Output } from "@angular/core";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { PullRequest, Query } from "@allors";
import { ContactMechanismPurpose, PartyContactMechanism, WebAddress } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  selector: "party-contactmechanism-webaddress",
  template: `

  <a-mat-select [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.ContactPurposes" [options]="contactMechanismPurposes" display="Name"></a-mat-select>
  <a-mat-input [object]="webAddress" [roleType]="m.EmailAddress.ElectronicAddressString" label="Web address"></a-mat-input>
  <a-mat-slide-toggle [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.UseAsDefault" label="Use as default"></a-mat-slide-toggle>
  <a-mat-slide-toggle [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.NonSolicitationIndicator" label="Non Solicitation"></a-mat-slide-toggle>

  <button mat-button color="primary" type="button" (click)="save()">Save</button>
  <button mat-button color="secondary" type="button"(click)="cancel()">Cancel</button>
`,
})
export class PartyContactMechanismInlineWebAddressComponent implements OnInit {
  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public webAddress: WebAddress;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanismPurposes: ContactMechanismPurpose[];

  public m: MetaDomain;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
  ) {
    this.m = this.allors.meta;
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

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
      this.saved.emit(this.partyContactMechanism);
  }
}
