import { Component, Self, OnInit, OnDestroy, Inject, HostBinding } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter, format, formatDistance } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, Payment, Invoice, PurchaseInvoice, WorkEffort, SerialisedItem, SalesOrder, ProductQuote, PurchaseOrder, PriceComponent, OrderAdjustment, RequestForQuote, PartyRelationship, InventoryItem, SerialisedInventoryItem } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService, TableRow, Table, ObjectService, MethodService, DeleteService, EditService, OverviewService, Sorter } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';
import { PrintService } from '../../../../services/actions';

interface Row extends TableRow {
  object: InventoryItem;
  facility: string;
  item: string;
  quantity: number;
  state: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialisedinventoryitem-overview-panel',
  templateUrl: './serialisedinventoryitem-overview-panel.component.html',
  providers: [PanelService]
})
export class SerialisedInventoryItemComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  table: Table<Row>;

  edit: Action;
  changeInventory: Action;

  objects: SerialisedInventoryItem[];

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public objectService: ObjectService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public editService: EditService
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    const { pull, x, m } = this.metaService;

    this.panel.name = 'serialised Inventory item';
    this.panel.title = 'Serialised Inventory items';
    this.panel.icon = 'link';
    this.panel.expandable = true;

    this.edit = this.editService.edit();
    this.changeInventory = {
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
    };

    this.table = new Table({
      selection: false,
      columns: [
        { name: 'facility', sort: true },
        { name: 'item', sort: true },
        { name: 'quantity', sort: true },
        { name: 'state', sort: true },
      ],
      defaultAction: this.changeInventory,
    });

    const inventoryPullName = `${this.panel.name}_${this.m.SerialisedInventoryItem.name}`;
    const serialiseditemPullName = `${this.panel.name}_${this.m.SerialisedItem.name}`;
 
    this.panel.onPull = (pulls) => {
      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: inventoryPullName,
          object: id,
          fetch: {
            InventoryItemsWherePart: {
              include: {
                SerialisedInventoryItem_SerialisedInventoryItemState: x,
                Facility: x,
                UnitOfMeasure: x
              }
            }
          },
        }),
        pull.SerialisedItem({
          name: inventoryPullName,
          object: id,
          fetch: {
            SerialisedInventoryItemsWhereSerialisedItem: {
              include: {
                SerialisedInventoryItemState: x,
                Facility: x,
                UnitOfMeasure: x
              }
            }
          },
        })
      );

      this.panel.onPulled = (loaded) => {

        const inventoryObjects = loaded.collections[inventoryPullName] as SerialisedInventoryItem[] ?? [];
        const serialisedItemobjects = loaded.collections[serialiseditemPullName] as SerialisedInventoryItem[] ?? [];

        this.objects = inventoryObjects.concat(serialisedItemobjects);

        if (this.objects) {
          this.table.total = loaded.values[`${this.objects.length}_total`] || this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              facility: v.Facility.Name,
              item: v.SerialisedItem?.displayName,
              quantity: v.Quantity,
              state: v.SerialisedInventoryItemState ? v.SerialisedInventoryItemState.Name : ''
            } as Row;
          });
        }
      };
    };
  }
}
