import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Meta } from '@allors/meta/generated'
import { ContextService, MetaService, RefreshService, SingletonId } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  SupplierRelationship,
  CustomOrganisationClassification,
  IndustryClassification,
  CustomerRelationship,
  InternalOrganisation,
  OrganisationRole,
  LegalForm,
  Locale,
  Currency,
} from '@allors/domain/generated';
import { Equals, Sort, And, Not, Exists } from '@allors/data/system';
import { FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './organisation-create.component.html',
  providers: [ContextService]
})
export class OrganisationCreateComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Add Organisation';

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

  legalForms: LegalForm[];
  currencies: Currency[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<OrganisationCreateComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private route: ActivatedRoute,
    private fetcher: FetcherService,
    private singletonId: SingletonId,
    private internalOrganisationId: InternalOrganisationId,
  ) {

    super();

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([,, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.locales,
            pull.Singleton({
              object: this.singletonId.value,
              fetch: {
                Locales: {
                  include: {
                    Language: x,
                    Country: x
                  }
                }
              }
            }),
            pull.Organisation({ object: id }),
            pull.OrganisationRole(),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.Name),
            }),
            pull.CustomOrganisationClassification({
              sort: new Sort(m.CustomOrganisationClassification.Name)
            }),
            pull.IndustryClassification({
              sort: new Sort(m.IndustryClassification.Name)
            }),
            pull.LegalForm({
              sort: new Sort(m.LegalForm.Description)
            }),
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

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.organisation = loaded.objects.Organisation as Organisation;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        if (this.organisation) {
          this.customerRelationship = loaded.collections.CustomerRelationships[0] as CustomerRelationship;
          this.supplierRelationship = loaded.collections.SupplierRelationships[0] as SupplierRelationship;
        } else {
          this.organisation = this.allors.context.create('Organisation') as Organisation;
          this.organisation.IsManufacturer = false;
          this.organisation.IsInternalOrganisation = false;
          this.organisation.CollectiveWorkEffortInvoice = false;
          this.organisation.PreferredCurrency = this.internalOrganisation.PreferredCurrency;
        }

        this.currencies = loaded.collections.Currencies as Currency[];
        this.locales = loaded.collections.Locales as Locale[] || [];
        this.classifications = loaded.collections.CustomOrganisationClassifications as CustomOrganisationClassification[];
        this.industries = loaded.collections.IndustryClassifications as IndustryClassification[];
        this.legalForms = loaded.collections.LegalForms as LegalForm[];
        this.roles = loaded.collections.OrganisationRoles as OrganisationRole[];
        this.customerRole = this.roles.find((v: OrganisationRole) => v.UniqueId === '8b5e0cee-4c98-42f1-8f18-3638fba943a0');
        this.supplierRole = this.roles.find((v: OrganisationRole) => v.UniqueId === '8c6d629b-1e27-4520-aa8c-e8adf93a5095');
        this.manufacturerRole = this.roles.find((v: OrganisationRole) => v.UniqueId === '32e74bef-2d79-4427-8902-b093afa81661');
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
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    if (this.activeRoles.indexOf(this.customerRole) > -1 && !this.isActiveCustomer) {
      const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
      customerRelationship.Customer = this.organisation;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.customerRole) > -1 && this.customerRelationship) {
      this.customerRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.customerRole) === -1 && this.isActiveCustomer) {
      this.customerRelationship.ThroughDate = new Date().toISOString();
    }

    if (this.activeRoles.indexOf(this.supplierRole) > -1 && !this.isActiveSupplier) {
      const supplierRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
      supplierRelationship.Supplier = this.organisation;
      supplierRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.supplierRole) > -1 && this.supplierRelationship) {
      this.supplierRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.supplierRole) === -1 && this.isActiveSupplier) {
      this.supplierRelationship.ThroughDate = new Date().toISOString();
    }

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.organisation.id,
          objectType: this.organisation.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
