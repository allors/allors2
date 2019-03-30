import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { CreateData } from '../../../../../material/base/services/object';
import { ContactMechanism, Currency, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, PostalAddress, PurchaseOrder, VatRate, VatRegime, SupplierRelationship, Facility } from '../../../../../domain';
import { Equals, PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './purchaseorder-create.component.html',
  providers: [ContextService]
})
export class PurchaseOrderCreateComponent implements OnInit, OnDestroy {

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
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  internalOrganisation: Organisation;

  addSupplier = false;

  addTakenViaContactMechanism = false;
  addTakenViaContactPerson = false;

  addBillToContactMechanism = false;
  addBillToContactPerson = false;

  addShipToAddress = false;
  addShipToContactPerson = false;

  private subscription: Subscription;
  private fetcher: Fetcher;
  facilities: Facility[];

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<PurchaseOrderCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private errorService: ErrorService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, internalOrganisationId]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Facility({
              predicate: new Equals({ propertyType: m.Facility.Owner, object: internalOrganisationId }),
              sort: new Sort(m.Facility.Name)
            }),
            pull.VatRate(),
            pull.VatRegime(),
            pull.Currency({ sort: new Sort(m.Currency.Name) }),
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

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.order = this.allors.context.create('PurchaseOrder') as PurchaseOrder;
        this.order.OrderedBy = this.internalOrganisation;
        this.facilities = loaded.collections.Facilities as Facility[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.currencies = loaded.collections.Currencies as Currency[];

        if (this.order.TakenViaSupplier) {
          this.updateSupplier(this.order.TakenViaSupplier);
        }

        if (this.order.OrderedBy) {
          this.updateOrderedBy(this.order.OrderedBy);
        }

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
      .subscribe(() => {
        const data: IObject = {
          id: this.order.id,
          objectType: this.order.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public supplierAdded(organisation: Organisation): void {

    const supplierRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
    supplierRelationship.Supplier = organisation;
    supplierRelationship.InternalOrganisation = this.internalOrganisation;

    this.order.TakenViaSupplier = organisation;
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
    this.order.TakenViaSupplier.AddPartyContactMechanism(partyContactMechanism);
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

  public supplierSelected(supplier: Party) {
    this.updateSupplier(supplier);
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
        object: supplier,
        fetch: {
          CurrentContacts: x,
        }
      }),
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.takenViaContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.takenViaContacts = loaded.collections.CurrentContacts as Person[];
      }, this.errorService.handler);
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
        object: organisation,
        fetch: {
          CurrentContacts: x,
        }
      }),
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.CurrentContacts as Person[];
        this.shipToContacts = this.billToContacts;
      }, this.errorService.handler);
  }
}
