import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { Meta } from '@allors/meta/generated';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, PanelManagerService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  Party,
  Person,
  CustomerShipment,
  Currency,
  PostalAddress,
  Facility,
  ShipmentMethod,
  Carrier,
  ShipmentPackage,
  OrganisationContactRelationship,
  PartyContactMechanism,
} from '@allors/domain/generated';
import { Equals, Sort } from '@allors/data/system';
import { InternalOrganisationId, FetcherService, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';


@Component({
  templateUrl: './customershipment-create.component.html',
  providers: [PanelManagerService, ContextService],
})
export class CustomerShipmentCreateComponent extends TestScope implements OnInit, OnDestroy {
  readonly m: Meta;
  public title: string;
  public subTitle: string;

  customerShipment: CustomerShipment;
  currencies: Currency[];
  shipToAddresses: PostalAddress[] = [];
  shipToContacts: Person[] = [];
  shipFromAddresses: PostalAddress[] = [];
  shipFromContacts: Person[] = [];
  internalOrganisation: Organisation;

  addShipFromAddress = false;

  addShipToAddress = false;
  addShipToContactPerson = false;

  private previousShipToparty: Party;

  private subscription: Subscription;
  facilities: Facility[];
  locales: Locale[];
  shipmentMethods: ShipmentMethod[];
  carriers: Carrier[];

  customersFilter: SearchFactory;
  
  get shipToCustomerIsPerson(): boolean {
    return !this.customerShipment.ShipToParty || this.customerShipment.ShipToParty.objectType.name === this.m.Person.name;
  }

  constructor(
    @Self() public panelManager: PanelManagerService,
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<CustomerShipmentCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { m, pull } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(([, internalOrganisationId]) => {
          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.locales,
            this.fetcher.ownWarehouses,
            pull.ShipmentMethod({ sort: new Sort(m.ShipmentMethod.Name) }),
            pull.Carrier({ sort: new Sort(m.Carrier.Name) }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            }),
          ];

          this.customersFilter = Filters.customersFilter(m, internalOrganisationId);

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => ({ loaded, isCreate })));
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.shipmentMethods = loaded.collections.ShipmentMethods as ShipmentMethod[];
        this.carriers = loaded.collections.Carriers as Carrier[];

        if (isCreate) {
          this.title = 'Add Customer Shipment';
          this.customerShipment = this.allors.context.create('CustomerShipment') as CustomerShipment;
          this.customerShipment.ShipFromParty = this.internalOrganisation;

          const shipmentPackage = this.allors.context.create('ShipmentPackage') as ShipmentPackage;
          this.customerShipment.AddShipmentPackage(shipmentPackage);

          if (this.facilities.length === 1) {
            this.customerShipment.ShipFromFacility = this.facilities[0];
          }
        } else {
          this.customerShipment = loaded.objects.CustomerShipment as CustomerShipment;

          if (this.customerShipment.CanWriteComment) {
            this.title = 'Edit Customer Shipment';
          } else {
            this.title = 'View Customer Shipment';
          }
        }

        if (this.customerShipment.ShipToParty) {
          this.updateShipToParty(this.customerShipment.ShipToParty);
        }

        if (this.customerShipment.ShipFromParty) {
          this.updateShipFromParty(this.customerShipment.ShipFromParty);
        }

        this.previousShipToparty = this.customerShipment.ShipToParty;
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
        id: this.customerShipment.id,
        objectType: this.customerShipment.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }

  public shipToContactPersonAdded(person: Person): void {
    const organisationContactRelationship = this.allors.context.create(
      'OrganisationContactRelationship'
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.customerShipment.ShipToParty as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipToContacts.push(person);
    this.customerShipment.ShipToContactPerson = person;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.customerShipment.ShipToParty.AddPartyContactMechanism(partyContactMechanism);

    const postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
    this.shipToAddresses.push(postalAddress);
    this.customerShipment.ShipToAddress = postalAddress;
  }

  public shipFromAddressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.shipFromAddresses.push(partyContactMechanism.ContactMechanism as PostalAddress);
    this.customerShipment.ShipFromParty.AddPartyContactMechanism(partyContactMechanism);
    this.customerShipment.ShipFromAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public customerSelected(customer: ISessionObject) {
    this.updateShipToParty(customer as Party);
  }

  private updateShipToParty(customer: Party): void {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: customer,
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
        object: customer,
        fetch: {
          CurrentContacts: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      if (this.customerShipment.ShipToParty !== this.previousShipToparty) {
        this.customerShipment.ShipToAddress = null;
        this.customerShipment.ShipToContactPerson = null;
        this.previousShipToparty = this.customerShipment.ShipToParty;
      }

      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.shipToAddresses = partyContactMechanisms
        .filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress')
        .map((v: PartyContactMechanism) => v.ContactMechanism) as PostalAddress[];
      this.shipToContacts = loaded.collections.CurrentContacts as Person[];
    });
  }

  private updateShipFromParty(organisation: Party): void {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: organisation,
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
        object: organisation,
        fetch: {
          CurrentContacts: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.shipFromAddresses = partyContactMechanisms
        .filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress')
        .map((v: PartyContactMechanism) => v.ContactMechanism) as PostalAddress[];
      this.shipToContacts = loaded.collections.CurrentContacts as Person[];
    });
  }
}
