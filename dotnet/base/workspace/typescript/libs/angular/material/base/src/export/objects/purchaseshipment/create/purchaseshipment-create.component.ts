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
  Person,
  OrganisationContactRelationship,
  PostalAddress,
  Party,
  PartyContactMechanism,
  ContactMechanism,
  PurchaseShipment,
} from '@allors/domain/generated';
import { Equals, Sort } from '@allors/data/system';
import { FetcherService, InternalOrganisationId, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';
import { TestScope, SearchFactory } from '@allors/angular/core';


@Component({
  templateUrl: './purchaseshipment-create.component.html',
  providers: [ContextService]
})
export class PurchaseShipmentCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title = 'Add Purchase Shipment';

  shipment: PurchaseShipment;
  shipFromContacts: Person[] = [];
  shipToAddresses: ContactMechanism[] = [];
  shipToContacts: Person[] = [];
  internalOrganisation: Organisation;

  facilities: Facility[];
  selectedFacility: Facility;
  addFacility = false;

  addSupplier = false;
  addShipFromContactPerson = false;

  addShipToAddress = false;
  addShipToContactPerson = false;

  private subscription: Subscription;

  suppliersFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseShipmentCreateComponent>,
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
        switchMap(() => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Facility({ sort: new Sort(m.Facility.Name) }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            })
          ];

          this.suppliersFilter = Filters.suppliersFilter(m, this.internalOrganisationId.value);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facilities = loaded.collections.Facilities as Facility[];

        this.shipment = this.allors.context.create('PurchaseShipment') as PurchaseShipment;
        this.shipment.ShipToParty = this.internalOrganisation;

        if (this.shipment.ShipFromParty) {
          this.updateSupplier(this.shipment.ShipFromParty);
        }

        if (this.shipment.ShipToParty) {
          this.updateOrderedBy(this.shipment.ShipToParty);
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.shipment.ShipToFacility = this.selectedFacility;

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.shipment.id,
          objectType: this.shipment.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
  }

  public supplierAdded(organisation: Organisation): void {

    const supplierRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
    supplierRelationship.Supplier = organisation;
    supplierRelationship.InternalOrganisation = this.internalOrganisation;

    this.shipment.ShipFromParty = organisation;
  }

  public shipFromContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.shipment.ShipFromParty as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipFromContacts.push(person);
    this.shipment.ShipFromContactPerson = person;
  }

  public shipToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.shipment.ShipToParty as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToContacts.push(person);
    this.shipment.ShipToContactPerson = person;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.shipment.ShipToParty.AddPartyContactMechanism(partyContactMechanism);
    this.shipment.ShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
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

        this.shipFromContacts = loaded.collections.CurrentContacts as Person[];
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
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToContacts = loaded.collections.CurrentContacts as Person[];
      });
  }
}
