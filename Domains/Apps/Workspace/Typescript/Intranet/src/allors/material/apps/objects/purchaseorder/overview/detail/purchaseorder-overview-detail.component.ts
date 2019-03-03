import { Component, OnDestroy, OnInit, Self } from '@angular/core';

import { Subscription } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { Organisation, Currency, ContactMechanism, Person, PartyContactMechanism, OrganisationContactRelationship, Party, VatRate, VatRegime, PostalAddress, CustomerRelationship, Facility, PurchaseOrder, SupplierRelationship } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { switchMap, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseorder-overview-detail',
  templateUrl: './purchaseorder-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class PurchaseOrderOverviewDetailComponent implements OnInit, OnDestroy {

  readonly m: Meta;

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

  private previousSupplier: Party;

  private subscription: Subscription;
  private fetcher: Fetcher;
  facilities: Facility[];

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);

    panel.name = 'detail';
    panel.title = 'Purchase Order Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const purchaseOrderPullName = `${panel.name}_${this.m.PurchaseOrder.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { m, pull, x } = this.metaService;

        pulls.push(

          this.fetcher.internalOrganisation,
          pull.PurchaseOrder({
            name: purchaseOrderPullName,
            object: this.panel.manager.id,
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.order = loaded.objects[purchaseOrderPullName] as PurchaseOrder;
      }
    };
  }

  public ngOnInit(): void {

    // Expanded
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.order = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            pull.PurchaseOrder({
              object: id,
              include: {
                OrderedBy: x,
                TakenViaSupplier: x,
                TakenViaContactMechanism: x,
                TakenViaContactPerson: x,
                BillToContactPerson: x,
                PurchaseOrderState: x,
                PurchaseOrderShipmentState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                ShipToAddress: {
                  PostalBoundary: {
                    Country: x,
                  }
                },
                BillToContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                },
              }
            }),
            pull.Facility({
              predicate: new Equals({ propertyType: m.Facility.Owner, object: this.internalOrganisation }),
              sort: new Sort(m.Facility.Name)
            }),
            pull.VatRate(),
            pull.VatRegime(),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;

        this.facilities = loaded.collections.Facilities as Facility[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];

        if (this.order.TakenViaSupplier) {
          this.updateSupplier(this.order.TakenViaSupplier);
        }

        if (this.order.OrderedBy) {
          this.updateOrderedBy(this.order.OrderedBy);
        }

        this.previousSupplier = this.order.TakenViaSupplier;

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
        this.panel.toggle();
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

        if (this.order.TakenViaSupplier  !== this.previousSupplier) {
          this.order.TakenViaContactMechanism = null;
          this.order.TakenViaContactPerson = null;
          this.previousSupplier = this.order.TakenViaSupplier;
        }

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
