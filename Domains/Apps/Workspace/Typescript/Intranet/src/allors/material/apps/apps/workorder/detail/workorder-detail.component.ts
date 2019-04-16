import * as moment from 'moment';
import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { PullRequest, And, Equals, ISessionObject, Or, Contains, ContainedIn, Filter } from '../../../../../framework';
import { AllorsFilterService, ContextService, NavigationService, RefreshService, MetaService, NavigationActivatedRoute, SearchFactory, Action, AllorsBarcodeService, UserId } from '../../../../../angular';
import { SaveService, Table, EditService, CreateData } from '../../../..';

import { WorkEffort, TimeEntry, WorkEffortInventoryAssignment, TimeSheet, RateType, InventoryItem, UnifiedGood, NonUnifiedPart } from '../../../../../domain';

export interface Row {
  object: WorkEffortInventoryAssignment;
  name: string;
  location: string;
  quantity: number;
}

@Component({
  styleUrls: ['./workorder-detail.component.scss'],
  templateUrl: './workorder-detail.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class WorkerOrderDetailComponent implements OnInit, OnDestroy {

  title = 'Work Orders - Detail';

  edit: Action;
  table: Table<Row>;

  private subscription: Subscription;
  private barcodeSubscription: Subscription;

  rateTypes: RateType[];
  timeSheets: TimeSheet[];
  timeEntries: TimeEntry[];
  workEffortInventoryAssignments: WorkEffortInventoryAssignment[];

  workEffortInventoryAssignment: WorkEffortInventoryAssignment;
  newSelected: InventoryItem;
  filter: (search: string) => Observable<ISessionObject[]>;

  workEffort: WorkEffort;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    private route: ActivatedRoute,
    public refreshService: RefreshService,
    public editService: EditService,
    public barcodeService: AllorsBarcodeService,
    public navigation: NavigationService,
    private saveService: SaveService,
    private userId: UserId,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.edit = editService.edit();
    this.edit.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'location', sort: true },
        { name: 'quantity', sort: true },
      ],
      actions: [
        this.edit,
      ],
      defaultAction: this.edit,
    });

    const { m, pull, x } = this.metaService;

    this.barcodeSubscription = barcodeService.scan$
      .pipe(
        switchMap((barcode: string) => {

          const unifiedProduct = new ContainedIn({
            propertyType: m.UnifiedGood.ProductIdentifications,
            extent: new Filter({
              objectType: m.ProductIdentification,
              predicate: new Equals({ propertyType: m.ProductIdentification.Identification, value: barcode })
            })
          });

          const inventoryItem = new ContainedIn({
            propertyType: m.InventoryItem.Part,
            extent: new Filter({
              objectType: m.UnifiedProduct,
              predicate: unifiedProduct
            })
          });

          const pulls = [
            pull.InventoryItem({
              predicate: inventoryItem,
              include: {
                Part: x,
                Facility: x,
              }
            }),
            pull.WorkEffortInventoryAssignment({
              predicate: new And([
                new Equals({ propertyType: m.WorkEffortInventoryAssignment.Assignment, object: this.workEffort }),
                new ContainedIn({
                  propertyType: m.WorkEffortInventoryAssignment.InventoryItem,
                  extent: new Filter({
                    objectType: m.InventoryItem,
                    predicate: inventoryItem
                  })
                })
              ]
              ),
              include: {
                Assignment: x,
                InventoryItem: {
                  Part: x,
                  Facility: x,
                },
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        const inventoryItems = loaded.collections.InventoryItems as InventoryItem[];
        const workEffortInventoryAssignments = loaded.collections.WorkEffortInventoryAssignments as WorkEffortInventoryAssignment[];

        if (workEffortInventoryAssignments && workEffortInventoryAssignments.length > 0) {
          const workEffortInventoryAssignment = workEffortInventoryAssignments[0];
          this.edit.execute(workEffortInventoryAssignment);
        } else if (inventoryItems && inventoryItems.length > 0) {
          const inventoryItem = inventoryItems[0];
          this.create(inventoryItem);
        }
      });

  }

  get createData(): CreateData {
    if (this.workEffort) {
      return {
        associationId: this.workEffort.id,
      };
    }
  }

  get runningTimeEntry(): TimeEntry {
    if (this.timeEntries) {
      return this.timeEntries.find((v => v.Worker.id === this.userId.value && !v.ThroughDate));
    }
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const object = navRoute.id();

          const pulls = [
            pull.RateType({}),
            pull.WorkEffort({
              object,
              include: {
                Customer: x,
                ContactPerson: x,
                WorkEffortState: x
              }
            }),
            pull.TimeSheet({
              predicate: new Equals({ propertyType: m.TimeSheet.Worker, object: this.userId.value }),
              include: {
                Worker: x,
                TimeEntries: x,
              }
            }),
            pull.TimeEntry({
              predicate: new Equals({ propertyType: m.TimeEntry.WorkEffort, object }),
              include: {
                WorkEffort: x,
                Worker: x,
              }
            }),
            pull.WorkEffortInventoryAssignment({
              predicate: new Equals({ propertyType: m.WorkEffortInventoryAssignment.Assignment, object }),
              include: {
                Assignment: x,
                InventoryItem: {
                  Part: x,
                  Facility: x,
                },
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.rateTypes = loaded.collections.RateTypes as RateType[];
        this.workEffort = loaded.objects.WorkEffort as WorkEffort;
        this.timeSheets = loaded.collections.TimeSheets as TimeSheet[];
        this.timeEntries = loaded.collections.TimeEntries as TimeEntry[];
        this.workEffortInventoryAssignments = loaded.collections.WorkEffortInventoryAssignments as WorkEffortInventoryAssignment[];

        // Parts
        const partModels: Row[] = this.workEffortInventoryAssignments.map((v) => {

          const part = v.InventoryItem.Part as UnifiedGood & NonUnifiedPart;

          return {
            object: v,
            name: part.Name,
            location: v.InventoryItem.Facility.Name,
            quantity: v.Quantity,
          };
        });

        this.table.data = partModels;

        this.filter = new SearchFactory({
          objectType: m.InventoryItem,
          roleTypes: [m.InventoryItem.Name],
        }).create(this.allors);

      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    if (this.barcodeSubscription) {
      this.barcodeSubscription.unsubscribe();
    }
  }

  onSelected(inventoryItem: InventoryItem) {
    if (!inventoryItem) {
      this.workEffortInventoryAssignment = null;
    } else {
      this.create(inventoryItem);
    }
  }

  start() {
    const { m } = this.metaService;

    const timeEntry = this.allors.context.create(m.TimeEntry) as TimeEntry;
    timeEntry.WorkEffort = this.workEffort;
    timeEntry.FromDate = moment.utc().toISOString();
    timeEntry.RateType = this.rateTypes[0];

    const timeSheet = this.timeSheets[0];
    timeSheet.AddTimeEntry(timeEntry);

    this.save();
  }

  stop() {
    this.runningTimeEntry.ThroughDate = moment.utc().toISOString();

    this.save();
  }

  private create(inventoryItem: InventoryItem) {
    const inventoryAssignment = this.allors.context.create('WorkEffortInventoryAssignment') as WorkEffortInventoryAssignment;
    inventoryAssignment.Assignment = this.workEffort;
    inventoryAssignment.InventoryItem = inventoryItem;
    inventoryAssignment.BillableQuantity = 1;
    inventoryAssignment.Quantity = 1;
    this.workEffortInventoryAssignment = inventoryAssignment;

    this.save();
  }

  private save() {

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
      },
        this.saveService.errorHandler);
  }
}
