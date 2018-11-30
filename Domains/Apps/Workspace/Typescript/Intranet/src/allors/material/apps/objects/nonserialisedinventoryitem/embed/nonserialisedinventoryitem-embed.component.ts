import { Component, OnDestroy, Input, Output, EventEmitter, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, SessionService, NavigationActivatedRoute, NavigationService, Action, ActionTarget } from '../../../../../angular';
import { Part, InventoryItem, InventoryItemKind, NonSerialisedInventoryItem, SerialisedInventoryItem } from '../../../../../domain';
import { PullRequest, ObjectType } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { TableRow, Table } from 'src/allors/material/base/components/table';
import { NavigateService } from 'src/allors/material/base/actions';

interface Row extends TableRow {
  object: InventoryItem;
  name: string;
  qoh: string;
  atp: string;
}

@Component({
  selector: 'nonserialisedinventoryitem-embed',
  templateUrl: './nonserialisedinventoryitem-embed.component.html',
  providers: [SessionService]
})
export class NonSerialisedInventoryComponent implements OnInit, OnDestroy {

  @Input() part: Part;

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<InventoryItem> = new EventEmitter<InventoryItem>();

  title = 'Inventory';

  table: Table<Row>;
  receiveInventory: Action;

  m: MetaDomain;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  inventoryItems: InventoryItem[];

  constructor(
    @Self() private allors: SessionService,
    public navigateService: NavigateService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService,
  ) {
    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.table = new Table({
      selection: false,
      columns: [
        { name: 'name', sort: true },
        'uom',
        'qoh',
        'atp',
      ],
      actions: [
        {
          name: () => 'Add transaction',
          description: () => '',
          disabled: () => false,
          execute: (target: ActionTarget) => {
            if (!Array.isArray(target)) {
              this.navigateService.navigationService.add(this.m.InventoryItemTransaction, target, this.part);
            }
          },
          result: null
        }
      ],
    });
  }

  public ngOnInit(): void {

    const { pull, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();

          const pulls = [
            pull.Part({
              object: id,
              fetch: {
                InventoryItemsWherePart: {
                  include: {
                    Facility: x,
                    UnitOfMeasure: x,
                  }
                }
              },
            })
          ];

          return this.allors
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.session.reset();

        const inventoryItems = loaded.collections.InventoryItems as NonSerialisedInventoryItem[];

        this.table.data = inventoryItems.map((v) => {
          return {
            object: v,
            name: v.Facility.Name,
            uom: v.UnitOfMeasure.Abbreviation,
            qoh: v.QuantityOnHand.toString(),
            atp: v.AvailableToPromise.toString(),
          } as Row;
        });
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors
      .save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}