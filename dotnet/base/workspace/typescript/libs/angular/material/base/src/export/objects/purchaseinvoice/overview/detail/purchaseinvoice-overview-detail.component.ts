import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, PanelService, ContextService } from '@allors/angular/services/core';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';
import {
  PurchaseInvoice,
  Currency,
  VatRegime,
  IrpfRegime,
  PurchaseInvoiceType,
  SupplierRelationship,
  CustomerRelationship,
  OrganisationContactRelationship,
  PartyContactMechanism,
  Person,
  ContactMechanism,
  Party,
  Organisation,
  PostalAddress,
} from '@allors/domain/generated';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseinvoice-overview-detail',
  templateUrl: './purchaseinvoice-overview-detail.component.html',
  providers: [ContextService, PanelService],
})
export class PurchaseInvoiceOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {
  readonly m: Meta;

  invoice: PurchaseInvoice;

  currencies: Currency[];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];
  purchaseInvoiceTypes: PurchaseInvoiceType[];

  billedFromContacts: Person[] = [];
  billedFromContactMechanisms: ContactMechanism[] = [];
  billToContacts: Person[] = [];
  shipToCustomerAddresses: ContactMechanism[] = [];
  shipToCustomerContacts: Person[] = [];
  billToEndCustomerContactMechanisms: ContactMechanism[] = [];
  billToEndCustomerContacts: Person[] = [];
  shipToEndCustomerAddresses: ContactMechanism[] = [];
  shipToEndCustomerContacts: Person[] = [];

  addBilledFrom = false;
  addBilledFromContactMechanism = false;
  addBilledFromContactPerson = false;
  addBillToContactMechanism = false;
  addBillToContactPerson = false;
  addShipToCustomer = false;
  addShipToCustomerAddress = false;
  addShipToCustomerContactPerson = false;
  addBillToEndCustomer = false;
  addBillToEndCustomerContactMechanism = false;
  addBillToEndCustomerContactPerson = false;
  addShipToEndCustomer = false;
  addShipToEndCustomerAddress = false;
  addShipToEndCustomerContactPerson = false;

  private previousBilledFrom: Party;
  private previousShipToCustomer: Party;
  private previousBillToEndCustomer: Party;
  private previousShipToEndCustomer: Party;

  private subscription: Subscription;
  internalOrganisation: Organisation;

  customersFilter: SearchFactory;
  employeeFilter: SearchFactory;
  suppliersFilter: SearchFactory;
  showIrpf: boolean;

  get shipToCustomerIsPerson(): boolean {
    return !this.invoice.ShipToCustomer || this.invoice.ShipToCustomer.objectType.name === this.m.Person.name;
  }

  get billToEndCustomerIsPerson(): boolean {
    return !this.invoice.BillToEndCustomer || this.invoice.BillToEndCustomer.objectType.name === this.m.Person.name;
  }

  get shipToEndCustomerIsPerson(): boolean {
    return !this.invoice.ShipToEndCustomer || this.invoice.ShipToEndCustomer.objectType.name === this.m.Person.name;
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Purchase Invoice Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Normal
    const purchaseInvoicePullName = `${panel.name}_${this.m.PurchaseInvoice.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const { id } = this.panel.manager;

      pulls.push(
        pull.PurchaseInvoice({
          name: purchaseInvoicePullName,
          object: id,
          include: {
            PurchaseInvoiceItems: {
              InvoiceItemType: x,
            },
            BilledFrom: x,
            DerivedBilledFromContactMechanism: {
              PostalAddress_Country: {},
            },
            BilledFromContactPerson: x,
            BillToEndCustomer: x,
            BillToEndCustomerContactPerson: x,
            ShipToEndCustomer: x,
            ShipToEndCustomerContactPerson: x,
            ElectronicDocuments: x,
            PurchaseInvoiceState: x,
            CreatedBy: x,
            LastModifiedBy: x,
            DerivedVatRegime: {
              VatRates: x,
            },
            DerivedBillToEndCustomerContactMechanism: {
              PostalAddress_Country: {},
            },
            DerivedShipToEndCustomerAddress: {
              Country: x,
            },
          },
        })
      );
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.invoice = loaded.objects[purchaseInvoicePullName] as PurchaseInvoice;
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
          this.invoice = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.PurchaseInvoice({
              object: id,
              include: {
                BilledFrom: x,
                BilledFromContactPerson: x,
                DerivedBilledFromContactMechanism: x,
                ShipToCustomer: x,
                BillToEndCustomer: x,
                DerivedBillToEndCustomerContactMechanism: x,
                BillToEndCustomerContactPerson: x,
                ShipToEndCustomer: x,
                DerivedShipToEndCustomerAddress: x,
                ShipToEndCustomerContactPerson: x,
                PurchaseInvoiceState: x,
                AssignedVatRegime: x,
                DerivedVatRegime: x,
              },
            }),
            pull.IrpfRegime({ sort: new Sort(m.IrpfRegime.Name) }),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.IsoCode),
            }),
            pull.PurchaseInvoiceType({
              predicate: new Equals({ propertyType: m.PurchaseInvoiceType.IsActive, value: true }),
              sort: new Sort(m.PurchaseInvoiceType.Name),
            }),
          ];

          this.customersFilter = Filters.customersFilter(m, this.internalOrganisationId.value);
          this.employeeFilter = Filters.employeeFilter(m, this.internalOrganisationId.value);
          this.suppliersFilter = Filters.suppliersFilter(m, this.internalOrganisationId.value);

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.currencies = loaded.collections.Currencies as Currency[];
        this.purchaseInvoiceTypes = loaded.collections.PurchaseInvoiceTypes as PurchaseInvoiceType[];

        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;

        if (this.invoice.BilledFrom) {
          this.updateBilledFrom(this.invoice.BilledFrom);
        }

        if (this.invoice.ShipToCustomer) {
          this.updateShipToCustomer(this.invoice.ShipToCustomer);
        }

        if (this.invoice.BillToEndCustomer) {
          this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
        }

        if (this.invoice.ShipToEndCustomer) {
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
        }

        this.previousBilledFrom = this.invoice.BilledFrom;
        this.previousShipToCustomer = this.invoice.ShipToCustomer;
        this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;
        this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.allors.context.save().subscribe(() => {
      this.refreshService.refresh();
      this.panel.toggle();
    }, this.saveService.errorHandler);
  }

  public billedFromAdded(organisation: Organisation): void {
    const supplierRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
    supplierRelationship.Supplier = organisation;
    supplierRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.BilledFrom = organisation;
  }

  public shipToCustomerAdded(party: Party): void {
    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.ShipToCustomer = party;
  }

  public billToEndCustomerAdded(party: Party): void {
    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.BillToEndCustomer = party;
  }

  public shipToEndCustomerAdded(party: Party): void {
    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.ShipToEndCustomer = party;
  }

  public billedFromContactPersonAdded(person: Person): void {
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BilledFrom as Organisation;
    organisationContactRelationship.Contact = person;

    this.billedFromContacts.push(person);
    this.invoice.BilledFromContactPerson = person;
  }

  public shipToCustomerContactPersonAdded(person: Person): void {
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BilledFrom as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToCustomerContacts.push(person);
    this.invoice.ShipToCustomerContactPerson = person;
  }

  public billToEndCustomerContactPersonAdded(person: Person): void {
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToEndCustomerContacts.push(person);
    this.invoice.BillToEndCustomerContactPerson = person;
  }

  public shipToEndCustomerContactPersonAdded(person: Person): void {
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToEndCustomerContacts.push(person);
    this.invoice.ShipToEndCustomerContactPerson = person;
  }

  public billedFromContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.billedFromContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BilledFrom.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedBilledFromContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.shipToCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedShipToCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public billToEndCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.billToEndCustomerContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedBillToEndCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToEndCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.shipToEndCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedShipToEndCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public billedFromSelected(organisation: ISessionObject) {
    if (organisation) {
      this.updateBilledFrom(organisation as Organisation);
    }
  }

  public shipToCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateShipToCustomer(party as Party);
    }
  }

  public billToEndCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateBillToEndCustomer(party as Party);
    }
  }

  public shipToEndCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateShipToEndCustomer(party as Party);
    }
  }

  private updateBilledFrom(party: Party): void {
    const { pull, x, m } = this.metaService;

    const pulls = [
      pull.Organisation({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_Country: x,
              },
            },
          },
        },
      }),
      pull.Organisation({
        object: party,
        fetch: {
          CurrentContacts: x,
        },
      }),
      pull.PurchaseOrder({
        predicate: new Equals({ propertyType: m.PurchaseOrder.TakenViaSupplier, object: party }),
        sort: new Sort(m.PurchaseOrder.OrderNumber),
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.invoice.BilledFrom !== this.previousBilledFrom) {
        this.invoice.AssignedBilledFromContactMechanism = null;
        this.invoice.BilledFromContactPerson = null;
        this.previousBilledFrom = this.invoice.BilledFrom;
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.billedFromContactMechanisms = partyContactMechanisms
        .filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress')
        .map((v: PartyContactMechanism) => v.ContactMechanism);
      this.billedFromContacts = loaded.collections.CurrentContacts as Person[];
    });
  }

  private updateShipToCustomer(party: Party) {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_Country: x,
              },
            },
          },
        },
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        },
      }),
      pull.Party({
        object: party,
        include: {
          PreferredCurrency: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.invoice.ShipToCustomer !== this.previousShipToCustomer) {
        this.invoice.AssignedShipToEndCustomerAddress = null;
        this.invoice.ShipToCustomerContactPerson = null;
        this.previousShipToCustomer = this.invoice.ShipToCustomer;
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.shipToCustomerAddresses = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
      this.shipToCustomerContacts = loaded.collections.CurrentContacts as Person[];
    });
  }

  private updateBillToEndCustomer(party: Party) {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_Country: x,
              },
            },
          },
        },
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.invoice.BillToEndCustomer !== this.previousBillToEndCustomer) {
        this.invoice.AssignedBillToEndCustomerContactMechanism = null;
        this.invoice.BillToEndCustomerContactPerson = null;
        this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;
      }

      if (this.invoice.BillToEndCustomer !== null && this.invoice.ShipToEndCustomer === null) {
        this.invoice.ShipToEndCustomer = this.invoice.BillToEndCustomer;
        this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.billToEndCustomerContactMechanisms = partyContactMechanisms
        .filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress')
        .map((v: PartyContactMechanism) => v.ContactMechanism);
      this.billToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
    });
  }

  private updateShipToEndCustomer(party: Party) {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_Country: x,
              },
            },
          },
        },
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.invoice.ShipToEndCustomer !== this.previousShipToEndCustomer) {
        this.invoice.AssignedShipToEndCustomerAddress = null;
        this.invoice.ShipToEndCustomerContactPerson = null;
        this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
      }

      if (this.invoice.ShipToEndCustomer !== null && this.invoice.BillToEndCustomer === null) {
        this.invoice.BillToEndCustomer = this.invoice.ShipToEndCustomer;
        this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.shipToEndCustomerAddresses = partyContactMechanisms
        .filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress')
        .map((v: PartyContactMechanism) => v.ContactMechanism);
      this.shipToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
    });
  }
}
