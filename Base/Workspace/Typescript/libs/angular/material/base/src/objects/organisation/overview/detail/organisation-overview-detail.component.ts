import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, NavigationService, PanelService, ContextService, SingletonId } from '@allors/angular/core';
import { CustomerShipment, Organisation, PartyContactMechanism, Party, Currency, PostalAddress, Person, Facility, ShipmentMethod, Carrier, OrganisationContactRelationship, InternalOrganisation, Enumeration, IrpfRegime, WorkTask, WorkEffortState, Priority, WorkEffortPurpose, ContactMechanism, WorkEffort, SalesOrder, ProductQuote, VatRegime, VatClause, Store, SalesOrderItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, CustomerRelationship, UnifiedGood, InventoryItemKind, ProductType, ProductCategory, VatRate, SupplierOffering, Brand, Model, ProductIdentificationType, ProductNumber, UnitOfMeasure, PriceComponent, Settings, SupplierRelationship, CustomOrganisationClassification, IndustryClassification, LegalForm } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'organisation-overview-detail',
  templateUrl: './organisation-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class OrganisationOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  organisation: Organisation;
  locales: Locale[];
  classifications: CustomOrganisationClassification[];
  industries: IndustryClassification[];
  internalOrganisation: InternalOrganisation;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  legalForms: LegalForm[];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];
  currencies: Currency[];

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public saveService: SaveService,
    public location: Location,
    public refreshService: RefreshService,
    private route: ActivatedRoute,
    private singletonId: SingletonId,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    panel.name = 'detail';
    panel.title = 'Organisation Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const pullName = `${this.panel.name}_${this.m.Organisation.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.Organisation({
            name: pullName,
            object: id,
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.organisation = loaded.objects[pullName] as Organisation;
      }
    };
  }

  public ngOnInit(): void {

    // Expanded
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {
          this.organisation = undefined;

          const { m, x, pull } = this.metaService;
          const id = this.panel.manager.id;

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
            pull.VatRegime({
              sort: new Sort(m.VatRegime.Name)
            }),
            pull.IrpfRegime({
              sort: new Sort(m.IrpfRegime.Name)
            })
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.organisation = loaded.objects.Organisation as Organisation;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.currencies = loaded.collections.Currencies as Currency[];
        this.locales = loaded.collections.Locales as Locale[] || [];
        this.classifications = loaded.collections.CustomOrganisationClassifications as CustomOrganisationClassification[];
        this.industries = loaded.collections.IndustryClassifications as IndustryClassification[];
        this.legalForms = loaded.collections.LegalForms as LegalForm[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        this.refreshService.refresh();
        window.history.back();
      },
        this.saveService.errorHandler
      );
  }
}
