import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  SupplierRelationship,
  CustomerRelationship,
  VatRegime,
  IrpfRegime,
  Currency,
  Person,
  OrganisationContactRelationship,
  PostalAddress,
  Party,
  PartyContactMechanism,
  PurchaseInvoice,
  PurchaseInvoiceType,
  ContactMechanism,
} from '@allors/domain/generated';
import { Equals, Sort } from '@allors/data/system';
import { FetcherService, InternalOrganisationId, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  templateUrl: './purchaseinvoice-create.component.html',
  providers: [ContextService]
})
export class PurchaseInvoiceCreateComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;

  title = 'Add Purchase Invoice';

  invoice: PurchaseInvoice;
  currencies: Currency[];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];
  purchaseInvoiceTypes: PurchaseInvoiceType[];
  internalOrganisation: Organisation;

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

  customersFilter: SearchFactory;
  employeeFilter: SearchFactory;
  suppliersFilter: SearchFactory;
  billedFromContactMechanismInitialRole: ContactMechanism;
  shipToCustomerAddressInitialRole: ContactMechanism;
  billToEndCustomerContactMechanismInitialRole: ContactMechanism;
  shipToEndCustomerAddressInitialRole: ContactMechanism;
  currencyInitialRole: Currency;
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
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseInvoiceCreateComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(() => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.IrpfRegime({ sort: new Sort(m.IrpfRegime.Name) }),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.IsoCode)
            }),
            pull.PurchaseInvoiceType({
              predicate: new Equals({ propertyType: m.PurchaseInvoiceType.IsActive, value: true }),
              sort: new Sort(m.PurchaseInvoiceType.Name),
            })
          ];

          this.customersFilter = Filters.customersFilter(m, this.internalOrganisationId.value);
          this.employeeFilter = Filters.employeeFilter(m, this.internalOrganisationId.value);
          this.suppliersFilter = Filters.suppliersFilter(m, this.internalOrganisationId.value);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.currencies = loaded.collections.Currencies as Currency[];
        this.purchaseInvoiceTypes = loaded.collections.PurchaseInvoiceTypes as PurchaseInvoiceType[];

        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;

        this.invoice = this.allors.context.create('PurchaseInvoice') as PurchaseInvoice;
        this.invoice.BilledTo = this.internalOrganisation;

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

        this.currencyInitialRole = this.internalOrganisation.PreferredCurrency;
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
        const data: IObject = {
          id: this.invoice.id,
          objectType: this.invoice.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public billedFromAdded(organisation: Organisation): void {

    const supplierRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
    supplierRelationship.Supplier = organisation;
    supplierRelationship.InternalOrganisation = this.internalOrganisation;

    this.invoice.BilledFrom = organisation;
    this.billedFromSelected(organisation);
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

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BilledFrom as Organisation;
    organisationContactRelationship.Contact = person;

    this.billedFromContacts.push(person);
    this.invoice.BilledFromContactPerson = person;
  }

  public shipToCustomerContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BilledFrom as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToCustomerContacts.push(person);
    this.invoice.ShipToCustomerContactPerson = person;
  }

  public billToEndCustomerContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = person;

    this.billToEndCustomerContacts.push(person);
    this.invoice.BillToEndCustomerContactPerson = person;
  }

  public shipToEndCustomerContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Organisation(
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
      pull.Organisation({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      }),
      pull.Organisation({
        object: party,
        name: 'selectedSupplier',
        include: {
          OrderAddress: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BilledFrom !== this.previousBilledFrom) {
          this.invoice.AssignedBilledFromContactMechanism = null;
          this.invoice.BilledFromContactPerson = null;
          this.previousBilledFrom = this.invoice.BilledFrom;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.billedFromContactMechanisms = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billedFromContacts = loaded.collections.CurrentContacts as Person[];

        const selectedSupplier = loaded.objects.selectedSupplier as Organisation;
        this.billedFromContactMechanismInitialRole = selectedSupplier.OrderAddress;
      });
  }

  private updateShipToCustomer(party: Party) {

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
        name: 'selectedParty',
        include: {
          PreferredCurrency: x,
          BillingAddress: x,
          GeneralCorrespondence: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.ShipToCustomer !== this.previousShipToCustomer) {
          this.invoice.AssignedShipToEndCustomerAddress = null;
          this.invoice.ShipToCustomerContactPerson = null;
          this.previousShipToCustomer = this.invoice.ShipToCustomer;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipToCustomerAddresses = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToCustomerContacts = loaded.collections.CurrentContacts as Person[];

        const selectedparty = loaded.objects.selectedParty as Party;
        this.shipToCustomerAddressInitialRole = selectedparty.BillingAddress ?? selectedparty.GeneralCorrespondence;
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
                  PostalAddress_Country: x
              }
            }
          }
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      }),
      pull.Party({
        object: party,
        name: 'selectedParty',
        include: {
          PreferredCurrency: x,
          BillingAddress: x,
          GeneralCorrespondence: x,
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
        this.billToEndCustomerContactMechanisms = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
        
        const selectedparty = loaded.objects.selectedParty as Party;
        this.billToEndCustomerContactMechanismInitialRole = selectedparty.BillingAddress ?? selectedparty.GeneralCorrespondence;
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
                PostalAddress_Country: x
              }
            }
          }
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      }),
      pull.Party({
        object: party,
        name: 'selectedParty',
        include: {
          PreferredCurrency: x,
          BillingAddress: x,
          GeneralCorrespondence: x,
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
        this.shipToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
        
        const selectedparty = loaded.objects.selectedParty as Party;
        this.shipToEndCustomerAddressInitialRole = selectedparty.BillingAddress ?? selectedparty.GeneralCorrespondence;
      });
  }
}
