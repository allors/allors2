import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, NavigationService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, Facility, InternalOrganisation, Enumeration, Part, SerialisedItem, SerialisedInventoryItem, Locale } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-overview-detail',
  templateUrl: './serialiseditem-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class SerialisedItemOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  serialisedItem: SerialisedItem;

  internalOrganisation: InternalOrganisation;
  locales: Locale[];
  serialisedItemStates: Enumeration[];
  ownerships: Enumeration[];
  part: Part;
  currentSuppliers: Organisation[];
  currentFacility: Facility;

  private subscription: Subscription;
  serialisedItemAvailabilities: Enumeration[];
  internalOrganisationsFilter: SearchFactory;
  partiesFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private snackBar: MatSnackBar,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Serialised Asset data';
    panel.icon = 'business';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.SerialisedItem.name}`;

    panel.onPull = (pulls) => {

      this.serialisedItem = undefined;

      if (this.panel.isCollapsed) {
        const { pull } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.SerialisedItem({
            name: pullName,
            object: id,
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.serialisedItem = loaded.objects[pullName] as SerialisedItem;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.serialisedItem = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            pull.SerialisedItem({
              object: id,
              include: {
                SerialisedItemState: x,
                SerialisedItemCharacteristics: {
                  SerialisedItemCharacteristicType: {
                    UnitOfMeasure: x
                  }
                },
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x,
                },
                LocalisedComments: {
                  Locale: x,
                },
                LocalisedKeywords: {
                  Locale: x,
                },
                Ownership: x,
                Buyer: x,
                Seller: x,
                OwnedBy: x,
                RentedBy: x,
                PrimaryPhoto: x,
                SecondaryPhotos: x,
                AdditionalPhotos: x,
                PrivatePhotos: x,
                PublicElectronicDocuments: x,
                PrivateElectronicDocuments: x,
                PublicLocalisedElectronicDocuments: x,
                PrivateLocalisedElectronicDocuments: x,
                PurchaseInvoice: x,
                PurchaseOrder: x,
                SuppliedBy: x,
                AssignedSuppliedBy: x,
              }
            }),
            this.fetcher.internalOrganisation,
            this.fetcher.locales,
            pull.SerialisedItem({
              object: id,
              fetch: {
                PartWhereSerialisedItem: x
              }
            }),
            pull.SerialisedItem({
              object: id,
              fetch: {
                SerialisedInventoryItemsWhereSerialisedItem: {
                  include: {
                    Facility: x
                  }
                }
              }
            }),
            pull.InternalOrganisation({
              object: this.internalOrganisationId.value,
              fetch: {
                CurrentSuppliers: x
              }
            }),
            pull.SerialisedItemState({
              predicate: new Equals({ propertyType: m.SerialisedItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedItemState.Name),
            }),
            pull.SerialisedItemAvailability({
              predicate: new Equals({ propertyType: m.SerialisedItemAvailability.IsActive, value: true }),
              sort: new Sort(m.SerialisedItemAvailability.Name),
            }),
            pull.Ownership({
              predicate: new Equals({ propertyType: m.Ownership.IsActive, value: true }),
              sort: new Sort(m.Ownership.Name),
            }),
          ];

          this.internalOrganisationsFilter = Filters.internalOrganisationsFilter(m);
          this.partiesFilter = Filters.partiesFilter(m);

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.currentSuppliers = loaded.collections.CurrentSuppliers as Organisation[];

        this.serialisedItem = loaded.objects.SerialisedItem as SerialisedItem;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.serialisedItemStates = loaded.collections.SerialisedItemStates as Enumeration[];
        this.serialisedItemAvailabilities = loaded.collections.SerialisedItemAvailabilities as Enumeration[];
        this.ownerships = loaded.collections.Ownerships as Enumeration[];
        this.part = loaded.objects.Part as Part;

        const serialisedInventoryItems = loaded.collections.SerialisedInventoryItems as SerialisedInventoryItem[];
        const inventoryItem = serialisedInventoryItems.find(v => v.Quantity === 1);
        if (inventoryItem) {
          this.currentFacility = inventoryItem.Facility;
        }
      });

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public partSelected(part: Part): void {
    if (part) {
      this.part = part;
    }
  }

  public save(): void {

    // this.onSave();

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
        this.panel.toggle();
      },
        this.saveService.errorHandler
      );
  }

  public update(): void {
    const { context } = this.allors;

    this.onSave();

    context
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  private onSave() {
    this.part.AddSerialisedItem(this.serialisedItem);
  }
}
