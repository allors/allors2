import { Component, OnInit, Self, HostBinding, AfterViewInit, OnDestroy, Injector, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';
import { formatDistance, format, isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, Action, NavigationService, PanelService, PanelManagerService, ContextService, NavigationActivatedRoute, ActionTarget } from '@allors/angular/core';
import { CommunicationEvent, ContactMechanism, CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, InventoryItem, NonSerialisedInventoryItem, Part, NonUnifiedPart, Organisation, SupplierOffering, PartyContactMechanism, PartyRate, ProductIdentification, PurchaseInvoiceItem, PurchaseInvoice, PurchaseOrderItem, PurchaseOrder, QuoteItem, ProductQuote, RepeatingSalesInvoice, RequestForQuote, Quote, SalesInvoiceItem, SalesOrder, SalesOrderItem, SalesTerm, SerialisedItem, Party, Shipment, TimeEntry, WorkEffort } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService, ObjectData, ObjectService, OverviewService, MethodService, Sorter } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { RoleType } from '@allors/meta/system';
import { Pull, Fetch, Step, Sort, Equals } from '@allors/data/system';

interface Row extends TableRow {
  object: TimeEntry;
  person: string;
  from: string;
  through: string;
  time: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'timeentry-overview-panel',
  templateUrl: './timeentry-overview-panel.component.html',
  providers: [PanelService, ContextService]
})
export class TimeEntryOverviewPanelComponent extends TestScope implements OnInit {
  workEffort: WorkEffort;
  private subscription: Subscription;
@HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: TimeEntry[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public deleteService: DeleteService,
    public editService: EditService
  ) {
    super();

    this.m = this.metaService.m;

    this.panel.name = 'timeentry';
    this.panel.title = 'Time Entries';
    this.panel.icon = 'timer';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'person' },
        { name: 'from', sort: true },
        { name: 'through', sort: true },
        { name: 'time', sort: true },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    this.panel.onPull = (pulls) => {

      if (this.panel.isCollapsed) {
        const { pull, x, tree } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.WorkEffort({
            object: id,
            fetch: {
              ServiceEntriesWhereWorkEffort: {
                include: {
                  TimeEntry_Worker: x
                }
              }
            }
          }),
          pull.WorkEffort({
            object: id,
          }),
        );
      }
    };

    this.panel.onPulled = (loaded) => {
      this.workEffort = loaded.objects.WorkEffort as WorkEffort;
      this.objects = loaded.collections.ServiceEntries as TimeEntry[];
    };
  }

  ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest([this.panel.manager.on$, this.table.sort$])
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(([, sort]) => {
          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const sorter = new Sorter(
            {
              from: m.TimeEntry.FromDate,
              through: m.TimeEntry.ThroughDate,
              time: m.TimeEntry.AmountOfTime,
            }
          );

          const pulls = [
            pull.TimeEntry({
              predicate:
                new Equals({
                  propertyType: m.TimeEntry.WorkEffort,
                  object: id,
                }),
              sort: sorter.create(sort),
              include: {
                Worker: x
              }
            }),
            pull.WorkEffort({
              object: id,
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.objects = loaded.collections.TimeEntries as TimeEntry[];

        if (this.objects) {
          this.table.total = this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              person: v.Worker.displayName,
              from: format(new Date(v.FromDate), 'dd-MM-yyyy'),
              through: v.ThroughDate !== null ? format(new Date(v.ThroughDate), 'dd-MM-yyyy') : '',
              time: v.AmountOfTime,
            } as Row;
          });
        }
      });
  }
}