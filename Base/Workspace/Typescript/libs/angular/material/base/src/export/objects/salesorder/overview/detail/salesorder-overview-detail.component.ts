import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, PartyContactMechanism, Party, Currency, PostalAddress, Person, Facility, OrganisationContactRelationship, InternalOrganisation, IrpfRegime, ContactMechanism, SalesOrder, ProductQuote, VatRegime, VatClause, Store, SalesOrderItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, CustomerRelationship } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesorder-overview-detail',
  templateUrl: './salesorder-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class SalesOrderOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  order: SalesOrder;
  quote: ProductQuote;
  internalOrganisations: InternalOrganisation[];
  currencies: Currency[];
  billToContactMechanisms: ContactMechanism[] = [];
  billToEndCustomerContactMechanisms: ContactMechanism[] = [];
  shipFromAddresses: ContactMechanism[] = [];
  shipToAddresses: ContactMechanism[] = [];
  shipToEndCustomerAddresses: ContactMechanism[] = [];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];
  vatClauses: VatClause[];
  billToContacts: Person[] = [];
  billToEndCustomerContacts: Person[] = [];
  shipToContacts: Person[] = [];
  shipToEndCustomerContacts: Person[] = [];
  stores: Store[];
  orderItems: SalesOrderItem[] = [];
  salesInvoice: SalesInvoice;
  billingProcesses: BillingProcess[];
  billingForOrderItems: BillingProcess;
  selectedSerialisedInventoryState: string;
  inventoryItemStates: SerialisedInventoryItemState[];
  internalOrganisation: Organisation;

  addShipFromAddress = false;

  addShipToCustomer = false;
  addShipToAddress = false;
  addShipToContactPerson = false;

  addBillToCustomer = false;
  addBillToContactMechanism = false;
  addBillToContactPerson = false;

  addShipToEndCustomer = false;
  addShipToEndCustomerAddress = false;
  addShipToEndCustomerContactPerson = false;

  addBillToEndCustomer = false;
  addBillToEndCustomerContactMechanism = false;
  addBillToEndCustomerContactPerson = false;

  private previousShipToCustomer: Party;
  private previousShipToEndCustomer: Party;
  private previousBillToCustomer: Party;
  private previousBillToEndCustomer: Party;

  private subscription: Subscription;
  facilities: Facility[];

  customersFilter: SearchFactory;
  showIrpf: boolean;

  get billToCustomerIsPerson(): boolean {
    return !this.order.BillToCustomer || this.order.BillToCustomer.objectType.name === this.m.Person.name;
  }

  get shipToCustomerIsPerson(): boolean {
    return !this.order.ShipToCustomer || this.order.ShipToCustomer.objectType.name === this.m.Person.name;
  }

  get billToEndCustomerIsPerson(): boolean {
    return !this.order.BillToEndCustomer || this.order.BillToEndCustomer.objectType.name === this.m.Person.name;
  }

  get shipToEndCustomerIsPerson(): boolean {
    return !this.order.ShipToEndCustomer || this.order.ShipToEndCustomer.objectType.name === this.m.Person.name;
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private snackBar: MatSnackBar,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Sales Order Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const salesOrderPullName = `${panel.name}_${this.m.SalesOrder.name}`;
    const salesInvoicePullName = `${panel.name}_${this.m.SalesInvoice.name}`;
    const goodPullName = `${panel.name}_${this.m.Good.name}`;
    const billingProcessPullName = `${panel.name}_${this.m.BillingProcess.name}`;
    const serialisedInventoryItemStatePullName = `${panel.name}_${this.m.SerialisedInventoryItemState.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { m, pull, x } = this.metaService;

        pulls.push(

          pull.SalesOrder({
            name: salesOrderPullName,
            object: this.panel.manager.id,
            include: {
              SalesOrderItems: {
                Product: x,
                InvoiceItemType: x,
                SalesOrderItemState: x,
                SalesOrderItemShipmentState: x,
                SalesOrderItemPaymentState: x,
                SalesOrderItemInvoiceState: x,
              },
              SalesTerms: {
                TermType: x,
              },
              DerivedCurrency: x,
              BillToCustomer: x,
              BillToContactPerson: x,
              ShipToCustomer: x,
              ShipToContactPerson: x,
              ShipToEndCustomer: x,
              ShipToEndCustomerContactPerson: x,
              BillToEndCustomer: x,
              BillToEndCustomerContactPerson: x,
              SalesOrderState: x,
              SalesOrderShipmentState: x,
              SalesOrderInvoiceState: x,
              SalesOrderPaymentState: x,
              CreatedBy: x,
              LastModifiedBy: x,
              Quote: x,
              DerivedShipFromAddress: {
                Country: x,
              },
              DerivedShipToAddress: {
                Country: x,
              },
              DerivedBillToEndCustomerContactMechanism: {
                PostalAddress_Country: x
              },
              DerivedShipToEndCustomerAddress: {
                Country: x,
              },
              DerivedBillToContactMechanism: {
                PostalAddress_Country: x
              }
            }
          }),
          pull.SalesOrder({
            name: salesInvoicePullName,
            object: this.panel.manager.id,
            fetch: { SalesInvoicesWhereSalesOrder: x }
          }),
          pull.BillingProcess({
            name: billingProcessPullName,
            sort: new Sort(m.BillingProcess.Name),
          }),
          pull.Currency({
            predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
            sort: new Sort(m.Currency.IsoCode),
          }),
          pull.SerialisedInventoryItemState({
            name: serialisedInventoryItemStatePullName,
            predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
            sort: new Sort(m.SerialisedInventoryItemState.Name)
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.order = loaded.objects[salesOrderPullName] as SalesOrder;
        this.orderItems = loaded.collections[salesOrderPullName] as SalesOrderItem[];
        this.salesInvoice = loaded.objects[salesInvoicePullName] as SalesInvoice;
        this.currencies = loaded.collections.Currencies as Currency[];
        this.billingProcesses = loaded.collections[billingProcessPullName] as BillingProcess[];
        this.billingForOrderItems = this.billingProcesses.find((v: BillingProcess) => v.UniqueId === 'ab01ccc2-6480-4fc0-b20e-265afd41fae2');
        this.inventoryItemStates = loaded.collections[serialisedInventoryItemStatePullName] as SerialisedInventoryItemState[];
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
            this.fetcher.warehouses,
            pull.SalesOrder({
              object: id,
              include: {
                DerivedCurrency: x,
                Store: x,
                OriginFacility: x,
                ShipToCustomer: x,
                DerivedShipToAddress: x,
                ShipToContactPerson: x,
                SalesOrderState: x,
                DerivedBillToContactMechanism: x,
                BillToContactPerson: x,
                DerivedBillToEndCustomerContactMechanism: x,
                BillToEndCustomerContactPerson: x,
                ShipToEndCustomer: x,
                DerivedShipToEndCustomerAddress: x,
                ShipToEndCustomerContactPerson: x,
                DerivedVatClause: x,
                DerivedVatRegime: x,
                DerivedIrpfRegime: x,
              }
            }),
            pull.VatClause({ sort: new Sort(m.VatClause.Name) }),
            pull.Currency({ sort: new Sort(m.Currency.Name) }),
            pull.IrpfRegime({ sort: new Sort(m.IrpfRegime.Name) }),
            pull.Store({
              predicate: new Equals({ propertyType: m.Store.InternalOrganisation, object: this.internalOrganisation }),
              include: { BillingProcess: x },
              sort: new Sort(m.Store.Name)
            }),
            pull.Party(
              {
                object: this.internalOrganisationId.value,
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
          ];

          this.customersFilter = Filters.customersFilter(m, this.internalOrganisationId.value);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.order = loaded.objects.SalesOrder as SalesOrder;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.facilities = loaded.collections.Facilities as Facility[];
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.vatClauses = loaded.collections.VatClauses as VatClause[];
        this.stores = loaded.collections.Stores as Store[];
        this.currencies = loaded.collections.Currencies as Currency[];

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipFromAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);

        if (this.order.ShipToCustomer) {
          this.previousShipToCustomer = this.order.ShipToCustomer;
          this.updateShipToCustomer(this.order.ShipToCustomer);
        }

        if (this.order.BillToCustomer) {
          this.previousBillToCustomer = this.order.BillToCustomer;
          this.updateBillToCustomer(this.order.BillToCustomer);
        }

        if (this.order.BillToEndCustomer) {
          this.previousBillToEndCustomer = this.order.BillToEndCustomer;
          this.updateBillToEndCustomer(this.order.BillToEndCustomer);
        }

        if (this.order.ShipToEndCustomer) {
          this.previousShipToEndCustomer = this.order.ShipToEndCustomer;
          this.updateShipToEndCustomer(this.order.ShipToEndCustomer);
        }

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

  public shipToCustomerAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.order.ShipToCustomer = party;
  }

  public billToCustomerAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.order.BillToCustomer = party;
  }

  public shipToEndCustomerAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.order.ShipToEndCustomer = party;
  }

  public billToEndCustomerAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.order.BillToEndCustomer = party;
  }

  public billToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToContacts.push(person);
    this.order.BillToContactPerson = person;
  }

  public billToEndCustomerContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.BillToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToEndCustomerContacts.push(person);
    this.order.BillToEndCustomerContactPerson = person;
  }

  public shipToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToContacts.push(person);
    this.order.ShipToContactPerson = person;
  }

  public shipToEndCustomerContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToEndCustomerContacts.push(person);
    this.order.ShipToEndCustomerContactPerson = person;
  }

  public billToCustomerSelected(party: ISessionObject) {
    this.updateBillToCustomer(party as Party);
  }

  public billToEndCustomerSelected(party: ISessionObject) {
    this.updateBillToEndCustomer(party as Party);
  }

  public shipToEndCustomerSelected(party: ISessionObject) {
    this.updateShipToEndCustomer(party as Party);
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.order.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.AssignedBillToContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public billToEndCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToEndCustomerContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.order.BillToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.AssignedBillToEndCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.order.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.AssignedShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public shipToEndCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToEndCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.order.ShipToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.AssignedShipToEndCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public shipFromAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipFromAddresses.push(partyContactMechanism.ContactMechanism);
    this.order.TakenBy.AddPartyContactMechanism(partyContactMechanism);
    this.order.AssignedShipFromAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  private updateShipToCustomer(party: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
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
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      }),
      pull.Party({
        object: party,
        include: {
          PreferredCurrency: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.ShipToCustomer !== this.previousShipToCustomer) {
          this.order.AssignedShipToAddress = null;
          this.order.ShipToContactPerson = null;
          this.previousShipToCustomer = this.order.ShipToCustomer;
        }

        if (this.order.ShipToCustomer !== null && this.order.BillToCustomer === null) {
          this.order.BillToCustomer = this.order.ShipToCustomer;
          this.updateBillToCustomer(this.order.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  private updateBillToCustomer(party: Party) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
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
      pull.Party(
        {
          object: party,
          fetch: {
            CurrentContacts: x,
          }
        }
      ),
      pull.Party({
        object: party,
        include: {
          PreferredCurrency: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.BillToCustomer !== this.previousBillToCustomer) {
          this.order.AssignedBillToContactMechanism = null;
          this.order.BillToContactPerson = null;
          this.previousBillToCustomer = this.order.BillToCustomer;
        }

        if (this.order.BillToCustomer !== null && this.order.ShipToCustomer === null) {
          this.order.ShipToCustomer = this.order.BillToCustomer;
          this.updateShipToCustomer(this.order.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  private updateBillToEndCustomer(party: Party) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
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
      pull.Party(
        {
          object: party,
          fetch: {
            CurrentContacts: x
          }
        }
      )
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.BillToEndCustomer !== this.previousBillToEndCustomer) {
          this.order.AssignedBillToEndCustomerContactMechanism = null;
          this.order.BillToEndCustomerContactPerson = null;
          this.previousBillToEndCustomer = this.order.BillToEndCustomer;
        }

        if (this.order.BillToEndCustomer !== null && this.order.ShipToEndCustomer === null) {
          this.order.ShipToEndCustomer = this.order.BillToEndCustomer;
          this.updateShipToEndCustomer(this.order.ShipToEndCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.billToEndCustomerContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  private updateShipToEndCustomer(party: Party) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
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
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.ShipToEndCustomer !== this.previousShipToEndCustomer) {
          this.order.AssignedShipToEndCustomerAddress = null;
          this.order.ShipToEndCustomerContactPerson = null;
          this.previousShipToEndCustomer = this.order.ShipToEndCustomer;
        }

        if (this.order.ShipToEndCustomer !== null && this.order.BillToEndCustomer === null) {
          this.order.BillToEndCustomer = this.order.ShipToEndCustomer;
          this.updateBillToEndCustomer(this.order.BillToEndCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipToEndCustomerAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  public shipToCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateShipToCustomer(party as Party);
    }
  }

  public update(): void {
    const { context } = this.allors;

    context
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
