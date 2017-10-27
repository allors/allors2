import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, OnDestroy, OnInit , Output } from "@angular/core";
import { Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "@allors";
import { ContactMechanismPurpose, ContactMechanismType, Enumeration, PartyContactMechanism, TelecommunicationsNumber } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  selector: "party-contactmechanism-telecommunicationsnumber",
  template:
  `
  <a-mat-select [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.ContactPurposes" [options]="contactMechanismPurposes" display="Name"></a-mat-select>
  <a-mat-select [object]="telecommunicationsNumber" [roleType]="m.ContactMechanism.ContactMechanismType" [options]="contactMechanismTypes" display="Name"></a-mat-select>
  <a-mat-input  [object]="telecommunicationsNumber" [roleType]="m.TelecommunicationsNumber.CountryCode"></a-mat-input>
  <a-mat-input  [object]="telecommunicationsNumber" [roleType]="m.TelecommunicationsNumber.AreaCode"></a-mat-input>
  <a-mat-input  [object]="telecommunicationsNumber" [roleType]="m.TelecommunicationsNumber.ContactNumber"></a-mat-input>
  <a-mat-slide-toggle [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.UseAsDefault" label="Use as default"></a-mat-slide-toggle>
  <a-mat-slide-toggle [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.NonSolicitationIndicator" label="Non Solicitation"></a-mat-slide-toggle>

  <button mat-button color="primary" type="button" (click)="save()">Save</button>
  <button mat-button color="secondary" type="button"(click)="cancel()">Cancel</button>
`,
})
export class PartyContactMechanismInlineTelecommunicationsNumberComponent implements OnInit {

  @Output()
  public saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public contactMechanismPurposes: Enumeration[];
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanismTypes: ContactMechanismType[];
  public telecommunicationsNumber: TelecommunicationsNumber;

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
      new Query(
        {
          name: "contactMechanismTypes",
          objectType: this.m.ContactMechanismType,
        }),
      ];

    this.scope
      .load("Pull", new PullRequest({ query }))
      .subscribe((loaded: Loaded) => {
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
