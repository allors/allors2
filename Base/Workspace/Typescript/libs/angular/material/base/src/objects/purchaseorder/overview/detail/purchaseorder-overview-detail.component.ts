import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, NavigationService, PanelService, ContextService, SingletonId } from '@allors/angular/core';
import { CustomerShipment, Organisation, PartyContactMechanism, Party, Currency, PostalAddress, Person, Facility, ShipmentMethod, Carrier, OrganisationContactRelationship, InternalOrganisation, Enumeration, IrpfRegime, WorkTask, WorkEffortState, Priority, WorkEffortPurpose, ContactMechanism, WorkEffort, SalesOrder, ProductQuote, VatRegime, VatClause, Store, SalesOrderItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, CustomerRelationship, UnifiedGood, InventoryItemKind, ProductType, ProductCategory, VatRate, SupplierOffering, Brand, Model, ProductIdentificationType, ProductNumber, UnitOfMeasure, PriceComponent, Settings, SupplierRelationship, CustomOrganisationClassification, IndustryClassification, LegalForm, PurchaseOrder } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseorder-overview-detail',
  templateUrl: './purchaseorder-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class PurchaseOrderOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  order: PurchaseOrder;
  currencies: Currency[];
  takenViaContactMechanisms: ContactMechanism[] = [];
  takenViaContacts: Person[] = [];
  billToContactMechanisms: ContactMechanism[] = [];
  billToContacts: Person[] = [];
  shipToAddresses: ContactMechanism[] = [];
  shipToContacts: Person[] = [];
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  internalOrganisation: Organisation;
  facilities: Facility[];
  selectedFacility: Facility;
  addFacility = false;

  addSupplier = false;
  addTakenViaContactMechanism = false;
  addTakenViaContactPerson = false;

  addBillToContactMechanism = false;
  addBillToContactPerson = false;

  addShipToAddress = false;
  addShipToContactPerson = false;

  private previousSupplier: Party;

  private takenVia: Party;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public filtersService: FiltersService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Purchase Order Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const purchaseOrderPullName = `${panel.name}_${this.m.PurchaseOrder.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { m, pull, x } = this.metaService;

        pulls.push(

          this.fetcher.internalOrganisation,
          pull.PurchaseOrder({
            name: purchaseOrderPullName,
            object: this.panel.manager.id,
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.order = loaded.objects[purchaseOrderPullName] as PurchaseOrder;
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

          this.order = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            pull.PurchaseOrder({
              object: id,
              include: {
                OrderedBy: x,
                StoredInFacility: x,
                TakenViaSupplier: x,
                TakenViaContactMechanism: x,
                TakenViaContactPerson: x,
                BillToContactPerson: x,
                PurchaseOrderState: x,
                PurchaseOrderShipmentState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                ShipToAddress: {
                  Country: x,
                },
                BillToContactMechanism: {
                  PostalAddress_Country: x
                },
              }
            }),
            pull.VatRate(),
            pull.VatRegime(),
            pull.Facility({ sort: new Sort(m.Facility.Name) }),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.IsoCode)
            }),
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        this.selectedFacility = this.order.StoredInFacility;

        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.facilities  = loaded.collections.Facilities as Facility[];
        this.currencies = loaded.collections.Currencies as Currency[];

        if (this.order.TakenViaSupplier) {
          this.takenVia = this.order.TakenViaSupplier;
          this.updateSupplier(this.takenVia);
        }

        if (this.order.OrderedBy) {
          this.updateOrderedBy(this.order.OrderedBy);
        }

        this.previousSupplier = this.order.TakenViaSupplier;

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.order.StoredInFacility = this.selectedFacility;

    this.allors.context
      .save()
      .subscribe(() => {
        this.refreshService.refresh();
        this.panel.toggle();
      },
        this.saveService.errorHandler
      );
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
  }

  public supplierAdded(organisation: Organisation): void {

    const supplierRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
    supplierRelationship.Supplier = organisation;
    supplierRelationship.InternalOrganisation = this.internalOrganisation;

    this.order.TakenViaSupplier = organisation;
    this.takenVia = organisation;
  }

  public takenViaContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.takenVia as Organisation;
    organisationContactRelationship.Contact = person;

    this.takenViaContacts.push(person);
    this.order.TakenViaContactPerson = person;
  }

  public takenViaContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.takenViaContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.takenVia.AddPartyContactMechanism(partyContactMechanism);
    this.order.TakenViaContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public billToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.OrderedBy as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToContacts.push(person);
    this.order.BillToContactPerson = person;
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.order.OrderedBy.AddPartyContactMechanism(partyContactMechanism);
    this.order.BillToContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.OrderedBy as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToContacts.push(person);
    this.order.ShipToContactPerson = person;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.order.OrderedBy.AddPartyContactMechanism(partyContactMechanism);
    this.order.ShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public supplierSelected(supplier: ISessionObject) {
    this.updateSupplier(supplier as Party);
  }

  private updateSupplier(supplier: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: supplier,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: {
                ContactMechanism: {
                  PostalAddress_Country: x
                }
              }
            }
          }
        }
      ),
      pull.Party({
        object: supplier,
        fetch: {
          CurrentContacts: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.TakenViaSupplier !== this.previousSupplier) {
          this.order.TakenViaContactMechanism = null;
          this.order.TakenViaContactPerson = null;
          this.previousSupplier = this.order.TakenViaSupplier;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.takenViaContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.takenViaContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  private updateOrderedBy(organisation: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: organisation,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: {
                ContactMechanism: {
                  PostalAddress_Country: x
                }
              }
            }
          }
        }
      ),
      pull.Party({
        object: organisation,
        fetch: {
          CurrentContacts: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.CurrentContacts as Person[];
        this.shipToContacts = this.billToContacts;
      });
  }
}
