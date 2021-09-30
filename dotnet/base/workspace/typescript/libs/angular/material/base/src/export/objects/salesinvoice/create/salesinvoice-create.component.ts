import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  CustomerRelationship,
  VatRegime,
  IrpfRegime,
  Currency,
  Person,
  OrganisationContactRelationship,
  PostalAddress,
  Party,
  PartyContactMechanism,
  ContactMechanism,
  SalesInvoice,
  SalesInvoiceType,
} from '@allors/domain/generated';
import { Equals, Sort } from '@allors/data/system';
import { FetcherService, InternalOrganisationId, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  templateUrl: './salesinvoice-create.component.html',
  providers: [ContextService],
})
export class SalesInvoiceCreateComponent extends TestScope implements OnInit, OnDestroy {
  readonly m: Meta;
  public title = 'Add Sales Invoice';

  invoice: SalesInvoice;
  billToContactMechanisms: ContactMechanism[] = [];
  billToContacts: Person[] = [];
  billToEndCustomerContactMechanisms: ContactMechanism[] = [];
  billToEndCustomerContacts: Person[] = [];
  shipToAddresses: ContactMechanism[] = [];
  shipToContacts: Person[] = [];
  shipToEndCustomerAddresses: ContactMechanism[] = [];
  shipToEndCustomerContacts: Person[] = [];
  internalOrganisation: Organisation;
  currencies: Currency[];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];

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
  salesInvoiceTypes: SalesInvoiceType[];

  customersFilter: SearchFactory;
  currencyInitialRole: Currency;
  billToContactMechanismInitialRole: ContactMechanism;
  billToEndCustomerContactMechanismInitialRole: ContactMechanism;
  shipToEndCustomerAddressInitialRole: ContactMechanism;
  shipToAddressInitialRole: PostalAddress;
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
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<SalesInvoiceCreateComponent>,
    public metaService: MetaService,
    private saveService: SaveService,
    public refreshService: RefreshService,
    public internalOrganisationId: InternalOrganisationId,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { m, pull } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(([, internalOrganisationId]) => {
          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.IsoCode),
            }),
            pull.IrpfRegime({ sort: new Sort(m.IrpfRegime.Name) }),
            pull.SalesInvoiceType({
              predicate: new Equals({ propertyType: m.SalesInvoiceType.IsActive, value: true }),
              sort: new Sort(m.SalesInvoiceType.Name),
            }),
          ];

          this.customersFilter = Filters.customersFilter(m, internalOrganisationId);

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.salesInvoiceTypes = loaded.collections.SalesInvoiceTypes as SalesInvoiceType[];
        this.currencies = loaded.collections.Currencies as Currency[];
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];

        this.invoice = this.allors.context.create('SalesInvoice') as SalesInvoice;
        this.invoice.BilledFrom = this.internalOrganisation;
        this.invoice.AdvancePayment = '0';

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
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.invoice.id,
        objectType: this.invoice.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
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
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToContacts.push(person);
    this.invoice.BillToContactPerson = person;
  }

  public billToEndCustomerContactPersonAdded(person: Person): void {
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToEndCustomerContacts.push(person);
    this.invoice.BillToEndCustomerContactPerson = person;
  }

  public shipToContactPersonAdded(person: Person): void {
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToContacts.push(person);
    this.invoice.ShipToContactPerson = person;
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

  private updateShipToCustomer(party: Party): void {
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
          OrderAddress: x,
          BillingAddress: x,
          ShippingAddress: x,
          GeneralCorrespondence: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.previousShipToCustomer && this.invoice.ShipToCustomer !== this.previousShipToCustomer) {
        this.invoice.ShipToContactPerson = null;
      }

      this.previousShipToCustomer = this.invoice.ShipToCustomer;

      if (this.invoice.ShipToCustomer !== null && this.invoice.BillToCustomer === null) {
        this.invoice.BillToCustomer = this.invoice.ShipToCustomer;
        this.updateBillToCustomer(this.invoice.ShipToCustomer);
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.shipToAddresses = partyContactMechanisms
        .filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress')
        .map((v: PartyContactMechanism) => v.ContactMechanism);
      this.shipToContacts = loaded.collections.CurrentContacts as Person[];
      
      this.setDerivedInitialRoles();
    });
  }

  private updateBillToCustomer(party: Party) {
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
          OrderAddress: x,
          BillingAddress: x,
          ShippingAddress: x,
          GeneralCorrespondence: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.previousBillToCustomer && this.invoice.BillToCustomer !== this.previousBillToCustomer) {
        this.invoice.BillToContactPerson = null;
      }

      this.previousBillToCustomer = this.invoice.BillToCustomer;

      if (this.invoice.BillToCustomer !== null && this.invoice.ShipToCustomer === null) {
        this.invoice.ShipToCustomer = this.invoice.BillToCustomer;
        this.updateShipToCustomer(this.invoice.ShipToCustomer);
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
      this.billToContacts = loaded.collections.CurrentContacts as Person[];
      
      this.setDerivedInitialRoles();
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
      pull.Party({
        object: party,
        include: {
          PreferredCurrency: x,
          OrderAddress: x,
          BillingAddress: x,
          ShippingAddress: x,
          GeneralCorrespondence: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.previousBillToEndCustomer && this.invoice.BillToEndCustomer !== this.previousBillToEndCustomer) {
        this.invoice.BillToEndCustomerContactPerson = null;
      }
      this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;

      if (this.invoice.BillToEndCustomer !== null && this.invoice.ShipToEndCustomer === null) {
        this.invoice.ShipToEndCustomer = this.invoice.BillToEndCustomer;
        this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.billToEndCustomerContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
      this.billToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
      
      this.setDerivedInitialRoles();
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
      pull.Party({
        object: party,
        include: {
          PreferredCurrency: x,
          OrderAddress: x,
          BillingAddress: x,
          ShippingAddress: x,
          GeneralCorrespondence: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.previousShipToEndCustomer && this.invoice.ShipToEndCustomer !== this.previousShipToEndCustomer) {
        this.invoice.ShipToEndCustomerContactPerson = null;
      }

      this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;

      if (this.invoice.ShipToEndCustomer !== null && this.invoice.BillToEndCustomer === null) {
        this.invoice.BillToEndCustomer = this.invoice.ShipToEndCustomer;
        this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.shipToEndCustomerAddresses = partyContactMechanisms
        .filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress')
        .map((v: PartyContactMechanism) => v.ContactMechanism);
      this.shipToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
      
      this.setDerivedInitialRoles();
    });
  }

  private setDerivedInitialRoles() {
    this.currencyInitialRole = this.invoice.BillToCustomer?.PreferredCurrency ?? this.invoice.BillToCustomer?.Locale?.Country?.Currency ?? this.invoice.BilledFrom?.PreferredCurrency;
    this.billToContactMechanismInitialRole = this.invoice.BillToCustomer?.BillingAddress ?? this.invoice.BillToCustomer?.ShippingAddress ?? this.invoice.BillToCustomer?.GeneralCorrespondence;
    this.billToEndCustomerContactMechanismInitialRole = this.invoice.BillToEndCustomer?.BillingAddress ?? this.invoice.BillToEndCustomer?.ShippingAddress ?? this.invoice.BillToEndCustomer?.GeneralCorrespondence;
    this.shipToEndCustomerAddressInitialRole = this.invoice.ShipToEndCustomer?.ShippingAddress ?? this.invoice.ShipToEndCustomer?.GeneralCorrespondence;
    this.shipToAddressInitialRole = this.invoice.ShipToCustomer?.ShippingAddress;
  }
}
