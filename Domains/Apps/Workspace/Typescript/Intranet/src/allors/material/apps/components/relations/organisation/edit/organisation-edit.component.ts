import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, Scope, WorkspaceService, Allors } from '../../../../../../angular';
import { CustomerRelationship, CustomOrganisationClassification, IndustryClassification, InternalOrganisation, Locale, Organisation, OrganisationRole, SupplierRelationship } from '../../../../../../domain';
import { And, Equals, Exists, Not, PullRequest, Sort } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { Title } from '@angular/platform-browser';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './organisation-edit.component.html',
  providers: [Allors]
})
export class OrganisationEditComponent implements OnInit, OnDestroy {

  public title = 'Organisation';
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

  private fetcher: Fetcher;

  constructor(
    @Self() private allors: Allors,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService,
    titleService: Title) {

    titleService.setTitle(this.title);
    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Organisation({ object: id }),
            pull.OrganisationRole(),
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
            pull.CustomOrganisationClassification({
              sort: new Sort(m.CustomOrganisationClassification.Name)
            }),
            pull.IndustryClassification({
              sort: new Sort(m.IndustryClassification.Name)
            })
          ];

          if (id != null) {

            pulls.push(
              pull.CustomerRelationship(
                {
                  predicate: new And({
                    operands: [
                      new Equals({ propertyType: m.CustomerRelationship.Customer, object: id }),
                      new Equals({ propertyType: m.CustomerRelationship.InternalOrganisation, object: internalOrganisationId }),
                      new Not({
                        operand: new Exists({ propertyType: m.CustomerRelationship.ThroughDate }),
                      }),
                    ]
                  }),
                }),
            );

            pulls.push(
              pull.SupplierRelationship(
                {
                  predicate: new And({
                    operands: [
                      new Equals({ propertyType: m.SupplierRelationship.Supplier, object: id }),
                      new Equals({ propertyType: m.SupplierRelationship.InternalOrganisation, object: internalOrganisationId }),
                      new Not({
                        operand: new Exists({ propertyType: m.SupplierRelationship.ThroughDate }),
                      })
                    ]
                  }),
                }),
            );
          }

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.subTitle = 'edit organisation';
        this.organisation = loaded.objects.Organisation as Organisation;
        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;

        if (this.organisation) {
          this.customerRelationship = loaded.collections.CustomerRelationships[0] as CustomerRelationship;
          this.supplierRelationship = loaded.collections.SupplierRelationships[0] as SupplierRelationship;
        } else {
          this.subTitle = 'add a new organisation';
          this.organisation = scope.session.create('Organisation') as Organisation;
          this.organisation.IsManufacturer = false;
        }

        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.classifications = loaded.collections.CustomOrganisationClassifications as CustomOrganisationClassification[];
        this.industries = loaded.collections.IndustryClassifications as IndustryClassification[];
        this.roles = loaded.collections.OrganisationRoles as OrganisationRole[];
        this.customerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === '8B5E0CEE-4C98-42F1-8F18-3638FBA943A0');
        this.supplierRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === '8C6D629B-1E27-4520-AA8C-E8ADF93A5095');
        this.manufacturerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === '32E74BEF-2D79-4427-8902-B093AFA81661');
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
      },this.errorService.handler );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    const { scope } = this.allors;

    if (this.activeRoles.indexOf(this.customerRole) > -1 && !this.isActiveCustomer) {
      const customerRelationship = scope.session.create('CustomerRelationship') as CustomerRelationship;
      customerRelationship.Customer = this.organisation;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.customerRole) > -1 && this.customerRelationship) {
      this.customerRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.customerRole) === -1 && this.isActiveCustomer) {
      this.customerRelationship.ThroughDate = new Date();
    }

    if (this.activeRoles.indexOf(this.supplierRole) > -1 && !this.isActiveSupplier) {
      const supplierRelationship = scope.session.create('SupplierRelationship') as SupplierRelationship;
      supplierRelationship.Supplier = this.organisation;
      supplierRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.supplierRole) > -1 && this.supplierRelationship) {
      this.supplierRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.supplierRole) === -1 && this.isActiveSupplier) {
      this.supplierRelationship.ThroughDate = new Date();
    }

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
