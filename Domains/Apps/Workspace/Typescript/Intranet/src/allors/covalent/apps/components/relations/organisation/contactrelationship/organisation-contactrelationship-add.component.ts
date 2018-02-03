import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Loaded, Saved, Scope, WorkspaceService } from "../../../../../../angular";
import { Enumeration, Organisation, OrganisationContactRelationship, Person, PersonRole } from "../../../../../../domain";
import { Fetch, PullRequest, Query } from "../../../../../../framework";
import { MetaDomain } from "../../../../../../meta";

@Component({
  templateUrl: "./organisation-contactrelationship.html",
})
export class OrganisationContactrelationshipAddComponent implements OnInit, OnDestroy {

  public title: string = "Contact Relationship";
  public subTitle: string = "add a new contact relationship";

  public m: MetaDomain;

  public peopleFilter: Filter;
  public organisationContactRelationship: OrganisationContactRelationship;
  public person: Person;
  public organisationContactKinds: Enumeration[];
  public roles: PersonRole[];
  public addPerson: boolean = false;
  private refresh$: BehaviorSubject<Date>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.peopleFilter = new Filter({scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName], notExistRoletypes: [this.m.Person.CurrentOrganisationContactRelationships]});
  }

  public ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {
        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: "organisation",
            id,
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

        this.organisationContactKinds = loaded.collections.organisationContactKinds as Enumeration[];
        this.roles = loaded.collections.roles as PersonRole[];

        const organisation: Organisation = loaded.objects.organisation as Organisation;

        this.organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
        this.organisationContactRelationship.Organisation = organisation;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public personCancelled(): void {
    this.addPerson = false;
  }

  public personAdded(id: string): void {
    this.addPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;
    this.organisationContactRelationship.Contact = contact;
    this.refresh();
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
