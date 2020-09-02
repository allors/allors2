import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, NavigationService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, PartyContactMechanism, Party, Currency, PostalAddress, Person, Facility, ShipmentMethod, Carrier, OrganisationContactRelationship, PurchaseShipment } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseshipment-overview-detail',
  templateUrl: './purchaseshipment-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class PurchaseShipmentOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  purchaseShipment: PurchaseShipment;

  currencies: Currency[];
  shipToAddresses: PostalAddress[] = [];
  shipToContacts: Person[] = [];
  shipFromContacts: Person[] = [];
  internalOrganisation: Organisation;
  locales: Locale[];
  shipmentMethods: ShipmentMethod[];
  carriers: Carrier[];
  addShipToAddress = false;
  addShipFromContactPerson = false;

  facilities: Facility[];
  selectedFacility: Facility;
  addFacility = false;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  previousShipFromParty: Party;

  suppliersFilter: SearchFactory;

  get shipFromCustomerIsPerson(): boolean {
    return !this.purchaseShipment.ShipFromParty || this.purchaseShipment.ShipFromParty.objectType.name === this.m.Person.name;
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject(new Date());

    panel.name = 'detail';
    panel.title = 'Purchase Shipment Details';
    panel.icon = 'local_shipping';
    panel.expandable = true;

    // Collapsed
    const pullName = `${this.panel.name}_${this.m.PurchaseShipment.name}`;

    panel.onPull = (pulls) => {

      this.purchaseShipment = undefined;

      if (this.panel.isCollapsed) {
        const { pull } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          this.fetcher.internalOrganisation,
          pull.PurchaseShipment({
            name: pullName,
            object: id,
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.purchaseShipment = loaded.objects[pullName] as PurchaseShipment;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest([this.refresh$, this.panel.manager.on$])
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.purchaseShipment = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.locales,
            pull.Facility({ sort: new Sort(m.Facility.Name) }),
            pull.InternalOrganisation(
              {
                object: this.internalOrganisationId.value,
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
            pull.InternalOrganisation({
              object: this.internalOrganisationId.value,
              fetch: {
                CurrentContacts: x,
              }
            }),
            pull.ShipmentMethod({ sort: new Sort(m.ShipmentMethod.Name) }),
            pull.Carrier({ sort: new Sort(m.Carrier.Name) }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            }),
            pull.PurchaseShipment({
              object: id,
              include: {
                ShipFromParty: x,
                ShipFromAddress: x,
                ShipFromFacility: x,
                ShipToParty: x,
                ShipToContactPerson: x,
                ShipToAddress: x,
                ShipFromContactPerson: x,
                Carrier: x,
                ShipmentState: x,
                ElectronicDocuments: x
              }
            }),
          ];
          
          this.suppliersFilter = Filters.suppliersFilter(m, this.internalOrganisationId.value);

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism) as PostalAddress[];
        this.shipToContacts = loaded.collections.CurrentContacts as Person[];

        this.purchaseShipment = loaded.objects.PurchaseShipment as PurchaseShipment;
        this.selectedFacility = this.purchaseShipment.ShipToFacility;

        this.facilities = loaded.collections.Facilities as Facility[];
        this.shipmentMethods = loaded.collections.ShipmentMethods as ShipmentMethod[];
        this.carriers = loaded.collections.Carriers as Carrier[];

        if (this.purchaseShipment.ShipFromParty) {
          this.updateShipFromParty(this.purchaseShipment.ShipFromParty);
        }

        this.previousShipFromParty = this.purchaseShipment.ShipFromParty;
      });

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.purchaseShipment.ShipToFacility = this.selectedFacility;

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
        this.panel.toggle();
      },
        this.saveService.errorHandler
      );
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
  }

  public shipFromContactPersonAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.purchaseShipment.ShipFromParty as Organisation;
    organisationContactRelationship.Contact = person;

    this.shipFromContacts.push(person);
    this.purchaseShipment.ShipFromContactPerson = person;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.purchaseShipment.ShipToParty.AddPartyContactMechanism(partyContactMechanism);

    const postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
    this.shipToAddresses.push(postalAddress);
    this.purchaseShipment.ShipToAddress = postalAddress;
  }

  public supplierSelected(customer: ISessionObject) {
    this.updateShipFromParty(customer as Party);
  }

  private updateShipFromParty(organisation: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
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

        this.shipFromContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  public setDirty(): void {
    this.allors.context.session.hasChanges = true;
  }
}
