import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ContextService, NavigationService, PanelService, RefreshService, MetaService, FetcherService, TestScope, InternalOrganisationId } from '../../../../../../angular';
import { Locale, Organisation, PurchaseShipment, Currency, PostalAddress, Person, Party, Facility, ShipmentMethod, Carrier, OrganisationContactRelationship, PartyContactMechanism } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { SaveService, FiltersService } from '../../../../../../../allors/material';

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
  shipFromAddresses: PostalAddress[] = [];
  shipToContacts: Person[] = [];
  shipFromContacts: Person[] = [];
  internalOrganisation: Organisation;
  facilities: Facility[];
  locales: Locale[];
  shipmentMethods: ShipmentMethod[];
  carriers: Carrier[];
  addShipFromAddress = false;
  addShipToAddress = false;
  addShipFromContactPerson = false;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  previousShipFromParty: Party;

  get shipFromCustomerIsPerson(): boolean {
    return !this.purchaseShipment.ShipFromParty || this.purchaseShipment.ShipFromParty.objectType.name === this.m.Person.name;
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public filtersService: FiltersService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private fetcher: FetcherService
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
    this.subscription = combineLatest(this.refresh$, this.panel.manager.on$)
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
            this.fetcher.internalOrganisation,
            pull.ShipmentMethod({ sort: new Sort(m.ShipmentMethod.Name) }),
            pull.Carrier({ sort: new Sort(m.Carrier.Name) }),
            pull.Facility({
              predicate: new Equals({ propertyType: m.Facility.Owner, object: this.internalOrganisation }),
              sort: new Sort(m.Facility.Name)
            }),
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

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.purchaseShipment = loaded.objects.PurchaseShipment as PurchaseShipment;
        this.facilities = loaded.collections.Facilities as Facility[];
        this.shipmentMethods = loaded.collections.ShipmentMethods as ShipmentMethod[];
        this.carriers = loaded.collections.Carriers as Carrier[];

        if (this.purchaseShipment.ShipToParty) {
          this.updateShipToParty(this.purchaseShipment.ShipToParty);
        }

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

    this.allors.context.save()
      .subscribe(() => {
        this.panel.toggle();
      },
        this.saveService.errorHandler
      );
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

  public shipFromAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipFromAddresses.push(partyContactMechanism.ContactMechanism as PostalAddress);
    this.purchaseShipment.ShipFromParty.AddPartyContactMechanism(partyContactMechanism);
    this.purchaseShipment.ShipFromAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public supplierSelected(customer: Party) {
    this.updateShipFromParty(customer);
  }

  private updateShipToParty(internalOrganisation: Party): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: internalOrganisation,
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
        object: internalOrganisation,
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
        this.shipFromContacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  public setDirty(): void {
    this.allors.context.session.hasChanges = true;
  }
}
