import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Subscription } from "rxjs/Subscription";

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

import { template } from "./organisation-contactrelationship.component";

@Component({
  template: template,
})
export class OrganisationContactrelationshipEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string = "Contact Relationship";
  public subTitle: string = "add a new contact relationship";

  public m: MetaDomain;

  public peopleFilter: Filter;
  public organisationContactRelationship: OrganisationContactRelationship;
  public organisationContactKinds: Enumeration[];
  public roles: PersonRole[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.peopleFilter = new Filter({scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName]});
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const roleId: string = this.route.snapshot.paramMap.get("roleId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: "organisationContactRelationship",
            id: roleId,
            include: [
              new TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "organisationContactKinds",
              objectType: this.m.OrganisationContactKind,
            }),
          new Query(
            {
              name: "roles",
              objectType: this.m.PersonRole,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.organisationContactRelationship = loaded.objects.organisationContactRelationship as OrganisationContactRelationship;

        this.organisationContactKinds = loaded.collections.organisationContactKinds as Enumeration[];
        this.roles = loaded.collections.roles as PersonRole[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public goBack(): void {
    window.history.back();
  }
}
