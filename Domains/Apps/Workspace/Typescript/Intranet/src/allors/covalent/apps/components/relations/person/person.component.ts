import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { CustomerRelationship, Employment, Enumeration, InternalOrganisation, Locale, Person, PersonRole } from "../../../../../domain";
import { And, Equals, Exists, Fetch, Not, Path, Predicate, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

@Component({
  templateUrl: "./person.component.html",
})
export class PersonComponent implements OnInit, OnDestroy {

  public title: string = "Person";
  public subTitle: string;

  public m: MetaDomain;

  public internalOrganisation: InternalOrganisation;
  public person: Person;

  public locales: Locale[];
  public genders: Enumeration[];
  public salutations: Enumeration[];

  public roles: PersonRole[];
  public selectableRoles: PersonRole[] = [];
  public activeRoles: PersonRole[] = [];
  public customerRelationship: CustomerRelationship;
  public employment: Employment;

  private customerRole: PersonRole;
  private employeeRole: PersonRole;
  private isActiveCustomer: boolean;
  private isActiveEmployee: boolean;
  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService,
    private titleService: Title,
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.titleService.setTitle(this.title);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {
    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get("id");

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const fetch: Fetch[] = [
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Person.Picture }),
            ],
            name: "person",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "locales",
              objectType: this.m.Locale,
            }),
          new Query(
            {
              name: "genders",
              objectType: this.m.GenderType,
            }),
          new Query(
            {
              name: "salutations",
              objectType: this.m.Salutation,
            }),
          new Query(
            {
              name: "roles",
              objectType: this.m.PersonRole,
            }),
        ];

        if (id != null) {
          const customerRelationshipPredicate: And = new And();
          const customerRelationshipPredicates: Predicate[] = customerRelationshipPredicate.predicates;

          customerRelationshipPredicates.push(new Equals({ roleType: m.CustomerRelationship.Customer, value: id }));
          customerRelationshipPredicates.push(new Equals({ roleType: m.CustomerRelationship.InternalOrganisation, value: internalOrganisationId }));
          const not1 = new Not();
          customerRelationshipPredicates.push(not1);

          not1.predicate = new Exists({ roleType: m.CustomerRelationship.ThroughDate });

          const employmentPredicate: And = new And();
          const employmentPredicates: Predicate[] = employmentPredicate.predicates;

          employmentPredicates.push(new Equals({ roleType: m.Employment.Employee, value: id }));
          employmentPredicates.push(new Equals({ roleType: m.Employment.Employer, value: internalOrganisationId }));
          const not2 = new Not();
          employmentPredicates.push(not2);
          not2.predicate = new Exists({ roleType: m.Employment.ThroughDate });

          query.push(new Query(
            {
              name: "customerRelationships",
              objectType: m.CustomerRelationship,
              predicate: customerRelationshipPredicate,
            }),
          );

          query.push(new Query(
            {
              name: "employments",
              objectType: m.Employment,
              predicate: employmentPredicate,
          }),
          );
        }

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded) => {

        this.subTitle = "edit person";
        this.person = loaded.objects.person as Person;
        this.internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (this.person) {
          this.customerRelationship = loaded.collections.customerRelationships[0] as CustomerRelationship;
          this.employment = loaded.collections.employments[0] as Employment;
        } else {
          this.subTitle = "add a new person";
          this.person = this.scope.session.create("Person") as Person;
        }

        this.locales = loaded.collections.locales as Locale[];
        this.genders = loaded.collections.genders as Enumeration[];
        this.salutations = loaded.collections.salutations as Enumeration[];
        this.roles = loaded.collections.roles as PersonRole[];
        this.customerRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === "B29444EF-0950-4D6F-AB3E-9C6DC44C050F");
        this.employeeRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === "DB06A3E1-6146-4C18-A60D-DD10E19F7243");
        this.selectableRoles.push(this.customerRole);
        this.selectableRoles.push(this.employeeRole);

        if (this.internalOrganisation.ActiveCustomers.includes(this.person)) {
          this.isActiveCustomer = true;
          this.activeRoles.push(this.customerRole);
        }

        if (this.internalOrganisation.ActiveEmployees.includes(this.person)) {
          this.isActiveEmployee = true;
          this.activeRoles.push(this.employeeRole);
        }
      },
      (error: any) => {
         this.errorService.message(error);
         this.goBack();
      },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    if (this.activeRoles.indexOf(this.customerRole) > -1 && !this.isActiveCustomer) {
      const customerRelationship = this.scope.session.create("CustomerRelationship") as CustomerRelationship;
      customerRelationship.Customer = this.person;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.customerRole) > -1  && this.customerRelationship) {
      this.customerRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.customerRole) === -1 && this.isActiveCustomer) {
      this.customerRelationship.ThroughDate = new Date();
    }

    if (this.activeRoles.indexOf(this.employeeRole) > -1 && !this.isActiveEmployee) {
      const employment = this.scope.session.create("Employment") as Employment;
      employment.Employee = this.person;
      employment.Employer = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.employeeRole) > -1 && this.employment) {
      this.employment.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.employeeRole) === -1 && this.isActiveEmployee) {
      this.employment.ThroughDate = new Date();
    }

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
