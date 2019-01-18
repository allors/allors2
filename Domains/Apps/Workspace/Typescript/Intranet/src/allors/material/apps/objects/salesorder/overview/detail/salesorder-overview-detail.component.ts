import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { Organisation, ProductQuote, Currency, ContactMechanism, Person, Quote, PartyContactMechanism, OrganisationContactRelationship, Good, SalesOrder, InternalOrganisation, Party, RequestForQuote, SalesOrderItem, SalesInvoice, BillingProcess, SerialisedInventoryItemState, VatRate, VatRegime, Store, PostalAddress } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesorder-overview-detail',
  templateUrl: './salesorder-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class SalesOrderOverviewDetailComponent implements OnInit, OnDestroy {

  public m: Meta;

  public order: SalesOrder;
  public quote: ProductQuote;
  public internalOrganisations: InternalOrganisation[];
  public currencies: Currency[];
  public billToContactMechanisms: ContactMechanism[];
  public billToEndCustomerContactMechanisms: ContactMechanism[];
  public shipToAddresses: ContactMechanism[];
  public shipToEndCustomerAddresses: ContactMechanism[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public billToContacts: Person[];
  public billToEndCustomerContacts: Person[];
  public shipToContacts: Person[];
  public shipToEndCustomerContacts: Person[];
  public stores: Store[];

  public addShipToAddress = false;
  public addBillToContactPerson = false;
  public addBillToEndCustomerContactPerson = false;
  public addShipToContactPerson = false;
  public addShipToEndCustomerContactPerson = false;

  public addShipToContactMechanism: boolean;
  public addBillToContactMechanism: boolean;
  public addBillToEndCustomerContactMechanism: boolean;
  public addShipToEndCustomerAddress: boolean;

  private previousShipToCustomer: Party;
  private previousShipToEndCustomer: Party;
  private previousBillToCustomer: Party;
  private previousBillToEndCustomer: Party;

  public orderItems: SalesOrderItem[] = [];
  public goods: Good[] = [];
  public salesInvoice: SalesInvoice;
  public billingProcesses: BillingProcess[];
  public billingForOrderItems: BillingProcess;
  public selectedSerialisedInventoryState: string;
  public inventoryItemStates: SerialisedInventoryItemState[];

  private fetcher: Fetcher;
  private subscription: Subscription;

  get showBillToOrganisations(): boolean {
    return !this.order.BillToCustomer || this.order.BillToCustomer.objectType.name === 'Organisation';
  }
  get showBillToPeople(): boolean {
    return !this.order.BillToCustomer || this.order.BillToCustomer.objectType.name === 'Person';
  }

  get showShipToOrganisations(): boolean {
    return !this.order.ShipToCustomer || this.order.ShipToCustomer.objectType.name === 'Organisation';
  }
  get showShipToPeople(): boolean {
    return !this.order.ShipToCustomer || this.order.ShipToCustomer.objectType.name === 'Person';
  }

  get showBillToEndCustomerOrganisations(): boolean {
    return !this.order.BillToEndCustomer || this.order.BillToEndCustomer.objectType.name === 'Organisation';
  }
  get showBillToEndCustomerPeople(): boolean {
    return !this.order.BillToEndCustomer || this.order.BillToEndCustomer.objectType.name === 'Person';
  }

  get showShipToEndCustomerOrganisations(): boolean {
    return !this.order.ShipToEndCustomer || this.order.ShipToEndCustomer.objectType.name === 'Organisation';
  }
  get showShipToEndCustomerPeople(): boolean {
    return !this.order.ShipToEndCustomer || this.order.ShipToEndCustomer.objectType.name === 'Person';
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);

    panel.name = 'detail';
    panel.title = 'Request For Quote Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Normal
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
              ShipToAddress: {
                PostalBoundary: {
                  Country: x,
                }
              },
              BillToEndCustomerContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x,
                }
              },
              ShipToEndCustomerAddress: {
                PostalBoundary: {
                  Country: x,
                }
              },
              BillToContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x,
                }
              }
            }
          }),
          pull.SalesOrder({
            name: salesInvoicePullName,
            object: this.panel.manager.id,
            fetch: { SalesInvoicesWhereSalesOrder: x }
          }),
          pull.Good({
            name: goodPullName,
            sort: new Sort(m.Good.Name),
          }),
          pull.BillingProcess({
            name: billingProcessPullName,
            sort: new Sort(m.BillingProcess.Name),
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
        this.goods = loaded.collections[goodPullName] as Good[];
        this.billingProcesses = loaded.collections[billingProcessPullName] as BillingProcess[];
        this.billingForOrderItems = this.billingProcesses.find((v: BillingProcess) => v.UniqueId.toUpperCase() === 'AB01CCC2-6480-4FC0-B20E-265AFD41FAE2');
        this.inventoryItemStates = loaded.collections[serialisedInventoryItemStatePullName] as SerialisedInventoryItemState[];
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        filter((v) => {
          return this.panel.isExpanded;
        }),
        switchMap(([urlSegments, date, , internalOrganisationId]) => {

          this.order = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.SalesOrder({
              object: id,
              include: {
                ShipToCustomer: x,
                ShipToAddress: x,
                ShipToContactPerson: x,
                SalesOrderState: x,
                BillToContactMechanism: x,
                BillToContactPerson: x,
                BillToEndCustomerContactMechanism: x,
                BillToEndCustomerContactPerson: x,
                ShipToEndCustomer: x,
                ShipToEndCustomerAddress: x,
                ShipToEndCustomerContactPerson: x,
                VatRegime: {
                  VatRate: x,
                }
              }
            }),
            pull.VatRate(),
            pull.VatRegime(),
            pull.Currency({ sort: new Sort(m.CommunicationEventPurpose.Name) }),
            pull.Store({
              predicate: new Equals({ propertyType: m.Store.InternalOrganisation, value: internalOrganisationId }),
              include: { BillingProcess: x },
              sort: new Sort(m.Store.Name)
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.order = loaded.objects.SalesOrder as SalesOrder;

        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.stores = loaded.collections.Stores as Store[];
        this.currencies = loaded.collections.Currencies as Currency[];
        this.internalOrganisations = loaded.collections.InternalOrganisations as InternalOrganisation[];

        if (this.order.ShipToCustomer) {
          this.updateShipToCustomer(this.order.ShipToCustomer);
        }

        if (this.order.BillToCustomer) {
          this.updateBillToCustomer(this.order.BillToCustomer);
        }

        if (this.order.BillToEndCustomer) {
          this.updateBillToEndCustomer(this.order.BillToEndCustomer);
        }

        if (this.order.ShipToEndCustomer) {
          this.updateShipToEndCustomer(this.order.ShipToEndCustomer);
        }

        this.previousShipToCustomer = this.order.ShipToCustomer;
        this.previousShipToEndCustomer = this.order.ShipToEndCustomer;
        this.previousBillToCustomer = this.order.BillToCustomer;
        this.previousBillToEndCustomer = this.order.BillToEndCustomer;

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        this.panel.toggle();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public billToContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToContacts.push(contact);
    this.order.BillToContactPerson = contact;
  }

  public billToEndCustomerContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.BillToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToEndCustomerContacts.push(contact);
    this.order.BillToEndCustomerContactPerson = contact;
  }

  public shipToContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToContacts.push(contact);
    this.order.ShipToContactPerson = contact;
  }

  public shipToEndCustomerContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToEndCustomerContacts.push(contact);
    this.order.ShipToEndCustomerContactPerson = contact;
  }

  public billToCustomerSelected(party: Party) {
    this.updateBillToCustomer(party);
  }

  public billToEndCustomerSelected(party: Party) {
    this.updateBillToEndCustomer(party);
  }

  public shipToEndCustomerSelected(party: Party) {
    this.updateShipToEndCustomer(party);
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.order.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.BillToContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public billToEndCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToEndCustomerContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.order.BillToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.BillToEndCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.order.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.ShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public shipToEndCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToEndCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.order.ShipToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.order.ShipToEndCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  private updateShipToCustomer(party: Party): void {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: {
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x
                  }
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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.ShipToCustomer !== this.previousShipToCustomer) {
          this.order.ShipToAddress = null;
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
      }, this.errorService.handler);
  }

  private updateBillToCustomer(party: Party) {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: {
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
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
      )
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.BillToCustomer !== this.previousBillToCustomer) {
          this.order.BillToContactMechanism = null;
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
      }, this.errorService.handler);
  }

  private updateBillToEndCustomer(party: Party) {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: {
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.BillToEndCustomer !== this.previousBillToEndCustomer) {
          this.order.BillToEndCustomerContactMechanism = null;
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
      }, this.errorService.handler);
  }

  private updateShipToEndCustomer(party: Party) {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: party,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: {
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.order.ShipToEndCustomer !== this.previousShipToEndCustomer) {
          this.order.ShipToEndCustomerAddress = null;
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
      }, this.errorService.handler);
  }

  public shipToCustomerSelected(party: Party) {
    if (party) {
      this.updateShipToCustomer(party);
    }
  }

}
