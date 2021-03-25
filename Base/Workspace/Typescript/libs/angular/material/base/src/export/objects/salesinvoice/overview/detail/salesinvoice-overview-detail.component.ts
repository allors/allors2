import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, PartyContactMechanism, Party, Currency, PostalAddress, Person, OrganisationContactRelationship, IrpfRegime, ContactMechanism, VatRegime, VatClause, Good, SalesInvoice, CustomerRelationship } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesinvoice-overview-detail',
  templateUrl: './salesinvoice-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class SalesInvoiceOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  invoice: SalesInvoice;
  goods: Good[] = [];
  internalOrganisation: Organisation;
  currencies: Currency[];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];
  vatClauses: VatClause[];
  billToContactMechanisms: ContactMechanism[] = [];
  billToContacts: Person[] = [];
  billToEndCustomerContactMechanisms: ContactMechanism[] = [];
  billToEndCustomerContacts: Person[] = [];
  shipToAddresses: ContactMechanism[] = [];
  shipToContacts: Person[] = [];
  shipToEndCustomerAddresses: ContactMechanism[] = [];
  shipToEndCustomerContacts: Person[] = [];

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

  customersFilter: SearchFactory;
  showIrpf: boolean;

  get billToCustomerIsPerson(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer.objectType.name === this.m.Person.name;
  }

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
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
    ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Sales Invoice Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const salesInvoicePullName = `${panel.name}_${this.m.SalesInvoice.name}`;
    const goodPullName = `${panel.name}_${this.m.Good.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { m, pull, x } = this.metaService;

        pulls.push(

          pull.SalesInvoice({
            name: salesInvoicePullName,
            object: this.panel.manager.id,
            include: {
              SalesInvoiceItems: {
                Product: x,
                InvoiceItemType: x,
              },
              SalesTerms: {
                TermType: x,
              },
              DerivedVatClause: x,
              DerivedCurrency: x,
              BillToCustomer: x,
              BillToContactPerson: x,
              ShipToCustomer: x,
              ShipToContactPerson: x,
              ShipToEndCustomer: x,
              ShipToEndCustomerContactPerson: x,
              SalesInvoiceState: x,
              CreatedBy: x,
              LastModifiedBy: x,
              DerivedBillToContactMechanism: {
                PostalAddress_Country: x
              },
              DerivedShipToAddress: {
                Country: x
              },
              DerivedBillToEndCustomerContactMechanism: {
                PostalAddress_Country: x
              },
              DerivedShipToEndCustomerAddress: {
                Country: x
              }
            }
          }),
          pull.Good({
            name: goodPullName,
            sort: new Sort(m.Good.Name),
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.goods = loaded.collections.Goods as Good[];
        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
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
            pull.SalesInvoice({
              object: id,
              include: {
                BillToCustomer: x,
                DerivedBillToContactMechanism: x,
                BillToContactPerson: x,
                ShipToCustomer: x,
                DerivedShipToAddress: x,
                ShipToContactPerson: x,
                BillToEndCustomer: x,
                DerivedBillToEndCustomerContactMechanism: x,
                BillToEndCustomerContactPerson: x,
                ShipToEndCustomer: x,
                DerivedShipToEndCustomerAddress: x,
                ShipToEndCustomerContactPerson: x,
                SalesInvoiceState: x,
                DerivedCurrency: x,
                DerivedVatClause: x
              },
            }),
            pull.IrpfRegime({ sort: new Sort(m.IrpfRegime.Name) }),
            pull.VatClause({ sort: new Sort(m.VatClause.Name) }),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.IsoCode)
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: [
                new Sort(m.Organisation.PartyName),
              ],
            })
          ];

          this.customersFilter = Filters.customersFilter(m, this.internalOrganisationId.value);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.vatClauses = loaded.collections.VatClauses as VatClause[];
        this.currencies = loaded.collections.Currencies as Currency[];

        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;

        if (this.invoice.BillToCustomer) {
          this.previousBillToCustomer = this.invoice.BillToCustomer;
          this.updateBillToCustomer(this.invoice.BillToCustomer);
        }

        if (this.invoice.BillToEndCustomer) {
          this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;
          this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
        }

        if (this.invoice.ShipToCustomer) {
          this.previousShipToCustomer = this.invoice.ShipToCustomer;
          this.updateShipToCustomer(this.invoice.ShipToCustomer);
        }

        if (this.invoice.ShipToEndCustomer) {
          this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
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

    this.invoice.ShipToCustomer = party;
  }

  public billToCustomerAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.BillToCustomer = party;
  }

  public shipToEndCustomerAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.ShipToEndCustomer = party;
  }

  public billToEndCustomerAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.BillToEndCustomer = party;
  }

  public billToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToContacts.push(person);
    this.invoice.BillToContactPerson = person;
  }

  public billToEndCustomerContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToEndCustomerContacts.push(person);
    this.invoice.BillToEndCustomerContactPerson = person;
  }

  public shipToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToContacts.push(person);
    this.invoice.ShipToContactPerson = person;
  }

  public shipToEndCustomerContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToEndCustomerContacts.push(person);
    this.invoice.ShipToEndCustomerContactPerson = person;
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedBillToContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public billToEndCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToEndCustomerContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedBillToEndCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public shipToEndCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToEndCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.AssignedShipToEndCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public billToCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateBillToCustomer(party as Party);
    }
  }

  public billToEndCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateBillToEndCustomer(party as Party);
    }
  }

  public shipToCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateShipToCustomer(party as Party);
    }
  }

  public shipToEndCustomerSelected(party: ISessionObject) {
    if (party) {
      this.updateShipToEndCustomer(party as Party);
    }
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

        if (this.invoice.BillToCustomer !== this.previousBillToCustomer) {
          this.invoice.AssignedBillToContactMechanism = null;
          this.invoice.BillToContactPerson = null;
          this.previousBillToCustomer = this.invoice.BillToCustomer;
        }

        if (this.invoice.BillToCustomer !== null && this.invoice.ShipToCustomer === null) {
          this.invoice.ShipToCustomer = this.invoice.BillToCustomer;
          this.updateShipToCustomer(this.invoice.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.currentContacts as Person[];
      });
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

        if (this.invoice.ShipToCustomer !== this.previousShipToCustomer) {
          this.invoice.AssignedShipToAddress = null;
          this.invoice.ShipToContactPerson = null;
          this.previousShipToCustomer = this.invoice.ShipToCustomer;
        }

        if (this.invoice.ShipToCustomer !== null && this.invoice.BillToCustomer === null) {
          this.invoice.BillToCustomer = this.invoice.ShipToCustomer;
          this.updateBillToCustomer(this.invoice.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToContacts = loaded.collections.currentContacts as Person[];
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
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

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
        this.billToEndCustomerContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToEndCustomerContacts = loaded.collections.currentContacts as Person[];
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
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

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
        this.shipToEndCustomerAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToEndCustomerContacts = loaded.collections.currentContacts as Person[];
      });
  }
}
