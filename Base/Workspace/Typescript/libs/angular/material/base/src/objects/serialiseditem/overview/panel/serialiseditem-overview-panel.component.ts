import { Component, Self, OnInit, OnDestroy, Inject, HostBinding } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter, format, formatDistance } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, Payment, Invoice, PurchaseInvoice, WorkEffort, SerialisedItem, SalesOrder, ProductQuote, PurchaseOrder, PriceComponent } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService, TableRow, Table, ObjectService, MethodService, DeleteService, EditService, OverviewService, Sorter } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


interface Row extends TableRow {
  object: SerialisedItem;
  number: string;
  name: string;
  availability: string;
  onWebsite: string;
  ownership: string;
  ownedBy: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-overview-panel',
  templateUrl: './serialiseditem-overview-panel.component.html',
  providers: [PanelService]
})
export class SerialisedItemOverviewPanelComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: SerialisedItem[] = [];
  table: Table<Row>;

  delete: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'serialiseditem';
    this.panel.title = 'Serialised Assets';
    this.panel.icon = 'link';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number' },
        { name: 'name' },
        { name: 'availability' },
        { name: 'onWebsite' },
        { name: 'ownership' },
        { name: 'ownedBy' },
      ],
      actions: [
        {
          name: 'changeinventory',
          displayName: () => 'Change Inventory',
          description: () => '',
          disabled: () => false,
          execute: (target: ActionTarget) => {
            if (!Array.isArray(target)) {
              this.factoryService.create(this.m.InventoryItemTransaction, {
                associationId: target.id,
                associationObjectType: target.objectType,
              });
            }
          },
          result: null
        },
        this.overviewService.overview(),
        this.delete,
      ],
      defaultAction: this.overviewService.overview(),
      autoSort: true,
      autoFilter: true,
    });

    const partSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}`;
    const ownedSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}_OwnedSerialisedItemsName`;
    const rentedSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}_RentedSerialisedItems`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: partSerialisedItemsName,
          object: id,
          fetch: {
            SerialisedItems: {
              include: {
                OwnedBy: x,
                Ownership: x,
                SerialisedItemAvailability: x,
                SerialisedItemState: x,
              }
            }
          }
        }),
        pull.Party({
          object: id,
          name: ownedSerialisedItemsName,
          fetch: {
            SerialisedItemsWhereOwnedBy: {
              include: {
                OwnedBy: x,
                Ownership: x,
                SerialisedItemAvailability: x,
                SerialisedItemState: x,
              }
            }
          }
        }),
        pull.Party({
          object: id,
          name: rentedSerialisedItemsName,
          fetch: {
            SerialisedItemsWhereRentedBy: {
              include: {
                OwnedBy: x,
                Ownership: x,
                SerialisedItemAvailability: x,
                SerialisedItemState: x,
              }
            }
          }
        }),
      );

      this.panel.onPulled = (loaded) => {
        const partSerialisedItems = loaded.collections[partSerialisedItemsName] as SerialisedItem[];
        const ownedSerialisedItems = loaded.collections[ownedSerialisedItemsName] as SerialisedItem[];
        const rentedSerialisedItems = loaded.collections[rentedSerialisedItemsName] as SerialisedItem[];

        this.objects = [];

        if (ownedSerialisedItems !== undefined) {
          this.objects = this.objects.concat(ownedSerialisedItems);
        }

        if (rentedSerialisedItems !== undefined) {
          this.objects = this.objects.concat(rentedSerialisedItems);
        }

        if (partSerialisedItems !== undefined) {
          this.objects = this.objects.concat(partSerialisedItems);
        }

        if (this.objects) {
          this.table.total = this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              number: v.ItemNumber,
              name: v.displayName,
              availability: v.SerialisedItemAvailability ? v.SerialisedItemAvailability.Name : '',
              onWebsite: v.AvailableForSale ? 'Yes' : 'No',
              ownership: v.Ownership ? v.Ownership.Name : '',
              ownedBy: v.OwnedBy ? v.OwnedBy.displayName : '',
            } as Row;
          });
        }
      };
    };
  }
}
