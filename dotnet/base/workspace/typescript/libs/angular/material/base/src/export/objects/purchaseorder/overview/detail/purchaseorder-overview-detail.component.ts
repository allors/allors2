import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, PartyContactMechanism, Party, Currency, PostalAddress, Person, Facility, OrganisationContactRelationship, ContactMechanism, VatRegime, VatRate, SupplierRelationship, PurchaseOrder, IrpfRegime } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

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

  suppliersFilter: SearchFactory;
  irpfRegimes: IrpfRegime[];
  showIrpf: boolean;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
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
        const { pull } = this.metaService;

        pulls.push(

          pull.PurchaseOrder({
            name: purchaseOrderPullName,
            object: this.panel.manager.id,
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
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
            this.fetcher.internalOrganisation,
            pull.PurchaseOrder({
              object: id,
              include: {
                OrderedBy: x,
                StoredInFacility: x,
                TakenViaSupplier: x,
                DerivedTakenViaContactMechanism: x,
                TakenViaContactPerson: x,
                BillToContactPerson: x,
                PurchaseOrderState: x,
                PurchaseOrderShipmentState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                DerivedShipToAddress: {
                  Country: x,
                },
                DerivedBillToContactMechanism: {
                  PostalAddress_Country: x
                },
                DerivedVatRegime: {
                  VatRates: x,
                }
              }
            }),
            pull.IrpfRegime(),
            pull.Facility({ sort: new Sort(m.Facility.Name) }),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.IsoCode)
            }),
          ];

          this.suppliersFilter = Filters.suppliersFilter(m, this.internalOrganisationId.value);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
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
    this.order.StoredInFacility = facility;

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
    this.order.AssignedTakenViaContactMechanism = partyContactMechanism.ContactMechanism;
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
    this.order.AssignedBillToContactMechanism = partyContactMechanism.ContactMechanism;
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
    this.order.AssignedShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
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
          this.order.AssignedTakenViaContactMechanism = null;
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
