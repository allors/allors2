import { Component, Self, OnInit, OnDestroy, Inject, HostBinding } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter, format, formatDistance } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/services/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, Payment, Invoice, PurchaseInvoice, WorkEffort, SerialisedItem } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService, TableRow, Table, ObjectService, MethodService, DeleteService, EditService, OverviewService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


interface Row extends TableRow {
  object: WorkEffort;
  number: string;
  name: string;
  state: string;
  customer: string;
  cost: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'worktask-overview-panel',
  templateUrl: './worktask-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkTaskOverviewPanelComponent extends TestScope implements OnInit {
  serialisedItem: SerialisedItem;

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffort[] = [];

  delete: Action;
  table: Table<TableRow>;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.delete = this.deleteService.delete(this.panel.manager.context);

    this.panel.name = 'workeffort';
    this.panel.title = 'Work Efforts';
    this.panel.icon = 'work';
    this.panel.expandable = true;

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort },
        { name: 'name', sort },
        { name: 'state', sort },
        { name: 'customer', sort },
        { name: 'cost', sort },
        { name: 'lastModifiedDate', sort },
      ],
      actions: [
        this.overviewService.overview(),
        this.delete,
      ],
      defaultAction: this.overviewService.overview(),
      autoSort: true,
      autoFilter: true,
    });

    const customerPullName = `${this.panel.name}_${this.m.WorkEffort.name}_customer`;
    const contactPullName = `${this.panel.name}_${this.m.WorkEffort.name}_contact`;
    const assetPullName = `${this.panel.name}_${this.m.WorkEffort.name}_fixedasset`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Party({
          name: customerPullName,
          object: id,
          fetch: {
            WorkEffortsWhereCustomer: {
              include: {
                WorkEffortState: x,
                Customer: x
              }
            }
          }
        }),
        pull.Person({
          name: contactPullName,
          object: id,
          fetch: {
            WorkEffortsWhereContactPerson: {
              include: {
                WorkEffortState: x,
                Customer: x
              }
            }
          }
        }),
        pull.SerialisedItem({
          name: assetPullName,
          object: id,
          fetch: {
            WorkEffortFixedAssetAssignmentsWhereFixedAsset: {
              Assignment: {
                include: {
                  WorkEffortState: x,
                  Customer: x
                }
              }
            }
          }
        }),
        pull.SerialisedItem({
          object: id,
        }),
      );
    };

    this.panel.onPulled = (loaded) => {

      this.serialisedItem = loaded.objects.SerialisedItem as SerialisedItem;
      const fromCustomer = loaded.collections[customerPullName] as WorkEffort[];
      const fromContact = loaded.collections[contactPullName] as WorkEffort[];
      const fromAsset = loaded.collections[assetPullName] as WorkEffort[];

      if (fromCustomer !== undefined && fromCustomer.length > 0) {
        this.objects = fromCustomer;
      }

      if (fromContact !== undefined && fromContact.length > 0) {
        this.objects = fromContact;
      }

      if (fromAsset !== undefined && fromAsset.length > 0) {
        this.objects = fromAsset;
      }

      if (this.objects) {
        this.table.total = this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            number: v.WorkEffortNumber,
            name: v.Name,
            customer: v.Customer.displayName,
            state: v.WorkEffortState ? v.WorkEffortState.Name : '',
            cost: v.TotalCost,
            lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
          } as Row;
        });
      }
    };
  }
}
