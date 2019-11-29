import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, FetcherService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { ObjectData } from '../../../../../material/core/services/object';
import { Currency, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, PostalAddress, CustomerShipment, Facility, Locale } from '../../../../../domain';
import { Equals, PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { SaveService, FiltersService } from '../../../../../material';

@Component({
  templateUrl: './customershipment-create.component.html',
  providers: [ContextService]
})
export class CustomerShipmentCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title = 'Add Customer Shipment';

  customerShipment: CustomerShipment;
  currencies: Currency[];
  shipToAddresses: PostalAddress[] = [];
  shipToContacts: Person[] = [];
  shipFromAddresses: PostalAddress[] = [];
  shipFromContacts: Person[] = [];
  internalOrganisation: Organisation;

  addShipToAddress = false;
  addShipToContactPerson = false;

  private subscription: Subscription;
  facilities: Facility[];
  locales: Locale[];

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<CustomerShipmentCreateComponent>,
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

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([, internalOrganisationId]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.locales,
            pull.Facility({
              predicate: new Equals({ propertyType: m.Facility.Owner, object: internalOrganisationId }),
              sort: new Sort(m.Facility.Name)
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            })
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.facilities = loaded.collections.Facilities as Facility[];

        this.customerShipment = this.allors.context.create('CustomerShipment') as CustomerShipment;
        this.customerShipment.ShipFromParty = this.internalOrganisation;

        if (this.facilities.length > 0) {
          this.customerShipment.ShipFromFacility = this.facilities[0];
        }

        if (this.customerShipment.ShipToParty) {
          this.updateShipToParty(this.customerShipment.ShipToParty);
        }

        if (this.customerShipment.ShipFromParty) {
          this.updateShipFromParty(this.customerShipment.ShipFromParty);
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
          id: this.customerShipment.id,
          objectType: this.customerShipment.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }

  public shipToContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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

  public customerSelected(customer: Party) {
    this.updateShipToParty(customer);
  }

  private updateShipToParty(customer: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: customer,
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
        object: customer,
        fetch: {
          CurrentContacts: x,
        }
      }),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism) as PostalAddress[];
        this.shipToContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  private updateShipFromParty(organisation: Party): void {

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
        this.shipFromAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism) as PostalAddress[];
        this.shipToContacts = loaded.collections.CurrentContacts as Person[];
      });
  }
}
