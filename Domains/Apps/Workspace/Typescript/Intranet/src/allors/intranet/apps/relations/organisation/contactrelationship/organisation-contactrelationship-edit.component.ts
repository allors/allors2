import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Loaded, Saved, Scope } from "@allors";
import { Fetch, PullRequest, Query, TreeNode } from "@allors";
import { Enumeration, OrganisationContactRelationship, PersonRole } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./organisation-contactrelationship.component.html",
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
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;

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

        this.scope.session.reset();

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
