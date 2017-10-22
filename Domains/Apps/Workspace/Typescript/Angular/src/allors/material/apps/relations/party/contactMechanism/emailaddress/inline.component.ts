import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, OnDestroy, OnInit , Output } from "@angular/core";
import { Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "../../../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "../../../../../../domain";
import { ContactMechanismPurpose, EmailAddress, PartyContactMechanism } from "../../../../../../domain";
import { MetaDomain } from "../../../../../../meta/index";

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
export class PartyContactMechanismInlineEmailAddressComponent implements OnInit {

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
