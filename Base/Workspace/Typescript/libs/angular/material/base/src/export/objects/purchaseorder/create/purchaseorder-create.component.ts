import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  Facility,
  SupplierRelationship,
  VatRegime,
  Currency,
  Person,
  OrganisationContactRelationship,
  PostalAddress,
  Party,
  PartyContactMechanism,
  ContactMechanism,
  PurchaseOrder,
  IrpfRegime,
} from '@allors/domain/generated';
import { Equals, Sort } from '@allors/data/system';
import { FetcherService, InternalOrganisationId, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';
import { TestScope, SearchFactory } from '@allors/angular/core';


@Component({
  templateUrl: './purchaseorder-create.component.html',
  providers: [ContextService]
})
export class PurchaseOrderCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title = 'Add Purchase Order';

  order: PurchaseOrder;
  currencies: Currency[];
  takenViaContactMechanisms: ContactMechanism[] = [];
  takenViaContacts: Person[] = [];
  billToContactMechanisms: ContactMechanism[] = [];
  billToContacts: Person[] = [];
  shipToAddresses: ContactMechanism[] = [];
  shipToContacts: Person[] = [];
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

  private takenVia: Party;

  private subscription: Subscription;

  suppliersFilter: SearchFactory;
  irpfRegimes: IrpfRegime[];
  currencyInitialRole: Currency;
  shipToAddressInitialRole: PostalAddress;
  billToContactMechanismInitialRole: ContactMechanism;
  takenViaContactMechanismInitialRole: ContactMechanism;
  showIrpf: boolean;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseOrderCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId) {
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
            pull.IrpfRegime(),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.IsoCode)
            }),
            pull.Facility({ sort: new Sort(m.Facility.Name) }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            })
          ];

          this.suppliersFilter = Filters.suppliersFilter(m, internalOrganisationId);

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
        this.facilities  = loaded.collections.Facilities as Facility[];

        this.order = this.allors.context.create('PurchaseOrder') as PurchaseOrder;
        this.order.OrderedBy = this.internalOrganisation;

        if (this.order.TakenViaSupplier) {
          this.takenVia = this.order.TakenViaSupplier;
          this.updateSupplier(this.takenVia);
        }

        if (this.order.OrderedBy) {
          this.updateOrderedBy(this.order.OrderedBy);
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
        const data: IObject = {
          id: this.order.id,
          objectType: this.order.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public supplierAdded(supplier: Organisation): void {

    const supplierRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
    supplierRelationship.Supplier = supplier;
    supplierRelationship.InternalOrganisation = this.internalOrganisation;

    this.order.TakenViaSupplier = supplier;
    this.takenVia = supplier;
    this.supplierSelected(supplier);
  }

  public takenViaContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.TakenViaSupplier as Organisation;
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

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
  }

  private updateSupplier(supplier: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Organisation(
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
      pull.Organisation({
        object: supplier,
        fetch: {
          CurrentContacts: x,
        }
      }),
      pull.Organisation({
        object: supplier,
        name: 'selectedSupplier',
        include: {
          OrderAddress: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.takenViaContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.takenViaContacts = loaded.collections.CurrentContacts as Person[];

        const selectedSupplier = loaded.objects.selectedSupplier as Organisation;
        this.takenViaContactMechanismInitialRole = selectedSupplier.OrderAddress;
      });
  }

  private updateOrderedBy(organisation: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Organisation(
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
      pull.Organisation({
        object: organisation,
        fetch: {
          CurrentContacts: x,
        }
      }),
      pull.Organisation({
        object: organisation,
        name: 'selectedOrganisation',
        include: {
          PreferredCurrency: x,
          ShippingAddress: x,
          BillingAddress: x,
          GeneralCorrespondence: x,
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

        const selectedOrganisation = loaded.objects.selectedOrganisation as Organisation;
        this.currencyInitialRole = selectedOrganisation.PreferredCurrency;
        this.shipToAddressInitialRole = selectedOrganisation.ShippingAddress;
        this.billToContactMechanismInitialRole = selectedOrganisation.BillingAddress ?? selectedOrganisation.GeneralCorrespondence;
      });
  }
}
