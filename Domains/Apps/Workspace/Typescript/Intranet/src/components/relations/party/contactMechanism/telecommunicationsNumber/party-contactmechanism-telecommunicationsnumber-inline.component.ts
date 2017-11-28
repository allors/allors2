import { Component, EventEmitter, Input, OnInit , Output } from "@angular/core";

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, ContactMechanismPurpose } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort } from "@allors/framework";

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
export class PartyContactMechanismTelecommunicationsNumberInlineComponent implements OnInit {

  @Output()
  public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public contactMechanismPurposes: Enumeration[];
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanismTypes: ContactMechanismType[];
  public telecommunicationsNumber: TelecommunicationsNumber;

  public m: MetaDomain;

  constructor(private workspaceService: WorkspaceService, private errorService: ErrorService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
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
    this.saved.emit(this.partyContactMechanism);
  }
}
