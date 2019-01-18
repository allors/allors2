import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { Currency, ContactMechanism, Person, PartyContactMechanism, Good, InternalOrganisation, Party, VatRate, VatRegime, PurchaseOrder, PurchaseInvoice, PurchaseInvoiceType, OrganisationContactRelationship, Organisation, PostalAddress, SalesOrder, SalesInvoice, RepeatingSalesInvoice } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesinvoice-overview-detail',
  templateUrl: './salesinvoice-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class SalesInvoiceOverviewDetailComponent implements OnInit, OnDestroy {

  public m: Meta;

  public order: SalesOrder;
  public invoice: SalesInvoice;
  public repeatingInvoices: RepeatingSalesInvoice[];
  public repeatingInvoice: RepeatingSalesInvoice;
  public goods: Good[] = [];

  public internalOrganisations: InternalOrganisation[];
  public currencies: Currency[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];

  public billToContactMechanisms: ContactMechanism[];
  public billToContacts: Person[];
  public billToEndCustomerContactMechanisms: ContactMechanism[];
  public billToEndCustomerContacts: Person[];
  public shipToAddresses: ContactMechanism[];
  public shipToContacts: Person[];
  public shipToEndCustomerAddresses: ContactMechanism[];
  public shipToEndCustomerContacts: Person[];

  public addBillToContactMechanism = false;
  public addBillToContactPerson = false;
  public addBillToEndCustomerContactMechanism = false;
  public addBillToEndCustomerContactPerson = false;
  public addShipToAddress = false;
  public addShipToContactPerson = false;
  public addShipToEndCustomerAddress = false;
  public addShipToEndCustomerContactPerson = false;

  private previousShipToCustomer: Party;
  private previousShipToEndCustomer: Party;
  private previousBillToCustomer: Party;
  private previousBillToEndCustomer: Party;


  private fetcher: Fetcher;
  private subscription: Subscription;

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
    panel.title = 'Purchase Invoice Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Normal
    const salesInvoicePullName = `${panel.name}_${this.m.PurchaseInvoice.name}`;
    const salesOrderPullName = `${panel.name}_${this.m.PurchaseOrder.name}`;
    const goodPullName = `${panel.name}_${this.m.Good.name}`;
    const repeatingSalesInvoicePullName = `${panel.name}_${this.m.Good.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, x } = this.metaService;

      const { id } = this.panel.manager;

      pulls.push(
        pull.SalesInvoice({
          name: salesInvoicePullName,
          object: id,
          include: {
            SalesInvoiceItems: {
              Product: x,
              InvoiceItemType: x,
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
            SalesInvoiceState: x,
            CreatedBy: x,
            LastModifiedBy: x,
            SalesOrder: x,
            BillToContactMechanism: {
              PostalAddress_PostalBoundary: {
                Country: x
              }
            },
            ShipToAddress: {
              PostalBoundary: {
                Country: x
              }
            },
            BillToEndCustomerContactMechanism: {
              PostalAddress_PostalBoundary: {
                Country: x
              }
            },
            ShipToEndCustomerAddress: {
              PostalBoundary: {
                Country: x
              }
            }
          }
        }),
        pull.SalesInvoice({
          name: salesOrderPullName,
          object: id,
          fetch: {
            SalesOrder: x
          }
        }),
        pull.Good({
          name: goodPullName,
          sort: new Sort(m.Good.Name),
        }),
        pull.RepeatingSalesInvoice({
          name: repeatingSalesInvoicePullName,
          predicate: new Equals({ propertyType: m.RepeatingSalesInvoice.Source, object: id }),
          include: {
            Frequency: x,
            DayOfWeek: x
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.goods = loaded.collections.Goods as Good[];
      this.order = loaded.objects.SalesOrder as SalesOrder;
      this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
      this.repeatingInvoices = loaded.collections.RepeatingSalesInvoices as RepeatingSalesInvoice[];
      if (this.repeatingInvoices.length > 0) {
        this.repeatingInvoice = this.repeatingInvoices[0];
      } else {
        this.repeatingInvoice = undefined;
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
            pull.SalesInvoice({
              object: id,
              include: {
                BillToCustomer: x,
                BillToContactMechanism: x,
                BillToContactPerson: x,
                ShipToCustomer: x,
                ShipToAddress: x,
                ShipToContactPerson: x,
                BillToEndCustomer: x,
                BillToEndCustomerContactMechanism: x,
                BillToEndCustomerContactPerson: x,
                ShipToEndCustomer: x,
                ShipToEndCustomerAddress: x,
                ShipToEndCustomerContactPerson: x,
                SalesInvoiceState: x,
                SalesOrder: x,
              },
            }),
            pull.SalesInvoice({
              object: id,
              fetch: {
                SalesOrder: x
              }
            }),
            pull.VatRate(),
            pull.VatRegime(),
            pull.Currency({
              sort: new Sort(m.Currency.Name),
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: [
                new Sort(m.Organisation.PartyName),
              ],
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.currencies = loaded.collections.Currencies as Currency[];
        this.internalOrganisations = loaded.collections.InternalOrganisations as InternalOrganisation[];

        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
        this.order = loaded.objects.SalesOrder as SalesOrder;

        if (this.invoice.BillToCustomer) {
          this.updateBillToCustomer(this.invoice.BillToCustomer);
        }

        if (this.invoice.BillToEndCustomer) {
          this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
        }

        if (this.invoice.ShipToCustomer) {
          this.updateShipToCustomer(this.invoice.ShipToCustomer);
        }

        if (this.invoice.ShipToEndCustomer) {
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
        }

        this.previousShipToCustomer = this.invoice.ShipToCustomer;
        this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
        this.previousBillToCustomer = this.invoice.BillToCustomer;
        this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public billToContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToContacts.push(contact);
    this.invoice.BillToContactPerson = contact;
  }

  public billToEndCustomerContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToEndCustomerContacts.push(contact);
    this.invoice.BillToEndCustomerContactPerson = contact;
  }

  public shipToContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToContacts.push(contact);
    this.invoice.ShipToContactPerson = contact;
  }

  public shipToEndCustomerContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToEndCustomerContacts.push(contact);
    this.invoice.ShipToEndCustomerContactPerson = contact;
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.BillToContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public billToEndCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.billToEndCustomerContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.BillToEndCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.ShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public shipToEndCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToEndCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.ShipToEndCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
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

  private updateBillToCustomer(party: Party) {

    const { pull, tree, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          CurrentPartyContactMechanisms: x
        },
        include: tree.PartyContactMechanism(
          {
            ContactMechanism: {
              PostalAddress_PostalBoundary: {
                Country: x
              }
            }
          }
        )
      }),
      pull.Party({
        object: party.id,
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BillToCustomer !== this.previousBillToCustomer) {
          this.invoice.BillToContactMechanism = null;
          this.invoice.BillToContactPerson = null;
          this.previousBillToCustomer = this.invoice.BillToCustomer;
        }

        if (this.invoice.BillToCustomer !== null && this.invoice.ShipToCustomer === null) {
          this.invoice.ShipToCustomer = this.invoice.BillToCustomer;
          this.updateShipToCustomer(this.invoice.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateShipToCustomer(party: Party): void {
    const { pull, tree, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          PartyContactMechanisms: x
        },
        include: tree.PartyContactMechanism({
          ContactMechanism: {
            PostalAddress_PostalBoundary: {
              Country: x
            }
          }
        })
      }),
      pull.Party({
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.ShipToCustomer !== this.previousShipToCustomer) {
          this.invoice.ShipToAddress = null;
          this.invoice.ShipToContactPerson = null;
          this.previousShipToCustomer = this.invoice.ShipToCustomer;
        }

        if (this.invoice.ShipToCustomer !== null && this.invoice.BillToCustomer === null) {
          this.invoice.BillToCustomer = this.invoice.ShipToCustomer;
          this.updateBillToCustomer(this.invoice.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateBillToEndCustomer(party: Party) {

    const { pull, tree, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          CurrentPartyContactMechanisms: x,
        },
        include: tree.PartyContactMechanism(
          {
            ContactMechanism: {
              PostalAddress_PostalBoundary: {
                Country: x
              }
            }
          }
        )
      }),
      pull.Party({
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BillToEndCustomer !== this.previousBillToEndCustomer) {
          this.invoice.BillToEndCustomerContactMechanism = null;
          this.invoice.BillToEndCustomerContactPerson = null;
          this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;
        }

        if (this.invoice.BillToEndCustomer !== null && this.invoice.ShipToEndCustomer === null) {
          this.invoice.ShipToEndCustomer = this.invoice.BillToEndCustomer;
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.billToEndCustomerContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToEndCustomerContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateShipToEndCustomer(party: Party) {

    const { pull, tree, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: x
        },
        include: tree.PartyContactMechanism({
          ContactMechanism: {
            PostalAddress_PostalBoundary: {
              Country: x
            }
          }
        })
      }),
      pull.Party({
        object: party,
        fetch: { CurrentContacts: x }
      }),
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.ShipToEndCustomer !== this.previousShipToEndCustomer) {
          this.invoice.ShipToEndCustomerAddress = null;
          this.invoice.ShipToEndCustomerContactPerson = null;
          this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
        }

        if (this.invoice.ShipToEndCustomer !== null && this.invoice.BillToEndCustomer === null) {
          this.invoice.BillToEndCustomer = this.invoice.ShipToEndCustomer;
          this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.shipToEndCustomerAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToEndCustomerContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }

}
