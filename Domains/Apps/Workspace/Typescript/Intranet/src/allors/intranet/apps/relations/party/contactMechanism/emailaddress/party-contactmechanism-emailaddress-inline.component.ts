import { Component, EventEmitter, OnInit, Output } from "@angular/core";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { PullRequest, Query } from "@allors";
import { ContactMechanismPurpose, EmailAddress, PartyContactMechanism } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  selector: "party-contactmechanism-emailAddress",
  template:
  `
  <a-mat-select [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.ContactPurposes" [options]="contactMechanismPurposes" display="Name"></a-mat-select>
  <a-mat-input [object]="emailAddress" [roleType]="m.EmailAddress.ElectronicAddressString" label="Email address"></a-mat-input>
  <a-mat-slide-toggle [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.UseAsDefault" label="Use as default"></a-mat-slide-toggle>
  <a-mat-slide-toggle [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.NonSolicitationIndicator" label="Non Solicitation"></a-mat-slide-toggle>

  <button mat-button color="primary" type="button" (click)="save()">Save</button>
  <button mat-button color="secondary" type="button"(click)="cancel()">Cancel</button>
`,
})
export class PartyContactMechanismEmailAddressInlineComponent implements OnInit {

  @Output()
  public saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public emailAddress: EmailAddress;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanismPurposes: ContactMechanismPurpose[];

  public m: MetaDomain;

  private scope: Scope;

  constructor(private allors: AllorsService, private errorService: ErrorService) {

    this.scope = new Scope(allors.database, allors.workspace);
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

    this.scope
      .load("Pull", new PullRequest({ query }))
      .subscribe((loaded: Loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];
        this.partyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
        this.emailAddress = this.scope.session.create("EmailAddress") as EmailAddress;
        this.partyContactMechanism.ContactMechanism = this.emailAddress;
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
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.partyContactMechanism.id);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }
}
