import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { CustomerRelationship, CustomOrganisationClassification, IndustryClassification, InternalOrganisation, Locale, Organisation, OrganisationRole, SupplierRelationship } from "../../../../../domain";
import { And, Equals, Exists, Fetch, Not, Path, Predicate, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

@Component({
  templateUrl: "./organisation.component.html",
})
export class OrganisationComponent implements OnInit, OnDestroy {

  public title: string = "Organisation";
  public subTitle: string;

  public m: MetaDomain;

  public organisation: Organisation;

  public locales: Locale[];
  public classifications: CustomOrganisationClassification[];
  public industries: IndustryClassification[];

  public customerRelationship: CustomerRelationship;
  public supplierRelationship: SupplierRelationship;
  public internalOrganisation: InternalOrganisation;
  public roles: OrganisationRole[];
  public selectableRoles: OrganisationRole[] = [];
  public activeRoles: OrganisationRole[] = [];
  private customerRole: OrganisationRole;
  private supplierRole: OrganisationRole;
  private manufacturerRole: OrganisationRole;
  private isActiveCustomer: boolean;
  private isActiveSupplier: boolean;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;
        const id: string = this.route.snapshot.paramMap.get("id");

        const fetch: Fetch[] = [
          this.fetcher.internalOrganisation,
          new Fetch({
            name: "organisation",
            id,
          }),
        ];

        const query: Query[] = [
          new Query(this.m.Locale),
          new Query(this.m.OrganisationRole),
          new Query(this.m.CustomOrganisationClassification),
          new Query(this.m.IndustryClassification),
          ];

        if (id != null) {
          const customerRelationshipPredicate: And = new And();
          const customerRelationshipPredicates: Predicate[] = customerRelationshipPredicate.predicates;

          customerRelationshipPredicates.push(new Equals({ roleType: m.CustomerRelationship.Customer, value: id }));
          customerRelationshipPredicates.push(new Equals({ roleType: m.CustomerRelationship.InternalOrganisation, value: internalOrganisationId }));
          const not1 = new Not();
          customerRelationshipPredicates.push(not1);

          not1.predicate = new Exists({ roleType: m.CustomerRelationship.ThroughDate });
          const supplierRelationshipPredicate: And = new And();
          const supplierRelationshipPredicates: Predicate[] = supplierRelationshipPredicate.predicates;

          supplierRelationshipPredicates.push(new Equals({ roleType: m.SupplierRelationship.Supplier, value: id }));
          supplierRelationshipPredicates.push(new Equals({ roleType: m.SupplierRelationship.InternalOrganisation, value: internalOrganisationId }));
          const not2 = new Not();
          customerRelationshipPredicates.push(not2);
          not2.predicate = new Exists({ roleType: m.SupplierRelationship.ThroughDate });

          query.push(new Query(
            {
              name: "customerRelationships",
              objectType: m.CustomerRelationship,
              predicate: customerRelationshipPredicate,
            }),
          );

          query.push(new Query(
            {
              name: "supplierRelationships",
              objectType: m.SupplierRelationship,
              predicate: supplierRelationshipPredicate,
          }),
          );
        }

        return this.scope
        .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded) => {

        this.subTitle = "edit organisation";
        this.organisation = loaded.objects.organisation as Organisation;
        this.internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (this.organisation) {
          this.customerRelationship = loaded.collections.customerRelationships[0] as CustomerRelationship;
          this.supplierRelationship = loaded.collections.supplierRelationships[0] as SupplierRelationship;
        } else {
          this.subTitle = "add a new organisation";
          this.organisation = this.scope.session.create("Organisation") as Organisation;
        }

        this.locales = loaded.collections.LocaleQuery as Locale[];
        this.classifications = loaded.collections.CustomOrganisationClassificationQuery as CustomOrganisationClassification[];
        this.industries = loaded.collections.IndustryClassificationQuery as IndustryClassification[];
        this.roles = loaded.collections.OrganisationRoleQuery as OrganisationRole[];
        this.customerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === "8B5E0CEE-4C98-42F1-8F18-3638FBA943A0");
        this.supplierRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === "8C6D629B-1E27-4520-AA8C-E8ADF93A5095");
        this.manufacturerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === "32E74BEF-2D79-4427-8902-B093AFA81661");
        this.selectableRoles.push(this.customerRole);
        this.selectableRoles.push(this.supplierRole);

        if (this.internalOrganisation.ActiveCustomers.includes(this.organisation)) {
          this.isActiveCustomer = true;
          this.activeRoles.push(this.customerRole);
        }

        if (this.internalOrganisation.ActiveSuppliers.includes(this.organisation)) {
          this.isActiveSupplier = true;
          this.activeRoles.push(this.supplierRole);
        }

        if (this.organisation.IsManufacturer) {
          this.activeRoles.push(this.manufacturerRole);
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
      customerRelationship.Customer = this.organisation;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.customerRole) > -1  && this.customerRelationship) {
      this.customerRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.customerRole) === -1 && this.isActiveCustomer) {
      this.customerRelationship.ThroughDate = new Date();
    }

    if (this.activeRoles.indexOf(this.supplierRole) > -1 && !this.isActiveSupplier) {
      const supplierRelationship = this.scope.session.create("SupplierRelationship") as SupplierRelationship;
      supplierRelationship.Supplier = this.organisation;
      supplierRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.supplierRole) > -1 && this.supplierRelationship) {
      this.supplierRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.supplierRole) === -1 && this.isActiveSupplier) {
      this.supplierRelationship.ThroughDate = new Date();
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
