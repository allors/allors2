import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, SessionService, NavigationService, NavigationActivatedRoute, MetaService } from '../../../../../angular';
import { InternalOrganisation, InventoryItemTransaction, InventoryItem, Part, InventoryTransactionReason, Facility, Lot, NonSerialisedInventoryItemState, SerialisedInventoryItemState } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './inventoryitemtransaction-edit.component.html',
  providers: [SessionService]
})
export class InventoryItemTransactionEditComponent implements OnInit, OnDestroy {

  readonly m: MetaDomain;

  readonly title = 'InventoryItem Transaction';

  internalOrganisation: InternalOrganisation;
  inventoryItem: InventoryItem;
  inventoryItemTransaction: InventoryItemTransaction;
  part: Part;
  inventoryTransactionReasons: InventoryTransactionReason[];
  selectedFacility: Facility;
  addFacility = false;
  facilities: Facility[];
  lots: Lot[];
  nonSerialisedInventoryItemStates: NonSerialisedInventoryItemState[];
  serialised: boolean;

  add: boolean;
  edit: boolean;

  private subscription: Subscription;
  private readonly refresh$: BehaviorSubject<Date>;
  private readonly fetcher: Fetcher;
  serialisedInventoryItemStates: SerialisedInventoryItemState[];

  constructor(
    @Self() public allors: SessionService,
    public metaService: MetaService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private titleService: Title,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
    this.titleService.setTitle(this.title);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, refresh, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const inventoryItemId = navRoute.queryParam(m.InventoryItem);
          const partId = navRoute.queryParam(m.Part);

          let pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.facilities,
            pull.InventoryTransactionReason(),
            pull.NonSerialisedInventoryItemState({ sort: new Sort(m.NonSerialisedInventoryItemState.Name) }),
            pull.SerialisedInventoryItemState({ sort: new Sort(m.SerialisedInventoryItemState.Name) }),
            pull.Lot({ sort: new Sort(m.Lot.LotNumber) })
          ];

          // InventoryItemTransactions are always added, never edited
          if (inventoryItemId) {
            pulls = [
              ...pulls,
              pull.InventoryItem({
                object: inventoryItemId,
                include: {
                  Facility: x,
                  UnitOfMeasure: x,
                  Lot: x
                }
              })
            ];
          }

          if (partId) {
            pulls = [
              ...pulls,
              pull.Part({
                object: partId,
              })
            ];
          }

          const add = !id;

          return this.allors
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {
        this.allors.session.reset();

        this.inventoryTransactionReasons = loaded.collections.InventoryTransactionReasons as InventoryTransactionReason[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.nonSerialisedInventoryItemStates  = loaded.collections.NonSerialisedInventoryItemStates as NonSerialisedInventoryItemState[];
        this.serialisedInventoryItemStates  = loaded.collections.SerialisedInventoryItemStates as SerialisedInventoryItemState[];
        this.lots  = loaded.collections.Lots as Lot[];

        this.inventoryItem = loaded.objects.InventoryItem as InventoryItem;
        this.part = loaded.objects.Part as Part;
        this.serialised = this.part.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase();

        if (add) {
          this.add = !(this.edit = false);

          this.inventoryItemTransaction = this.allors.session.create('InventoryItemTransaction') as InventoryItemTransaction;

          if (this.inventoryItem) {
            this.inventoryItemTransaction.Facility = this.inventoryItem.Facility;
            this.inventoryItemTransaction.UnitOfMeasure = this.inventoryItem.UnitOfMeasure;
            this.inventoryItemTransaction.Lot = this.inventoryItem.Lot;
            this.inventoryItemTransaction.TransactionDate =  new Date();
          }

          if (this.part) {
            this.inventoryItemTransaction.Part = this.part;
          }

        } else {
          this.edit = !(this.add = false);
        }

      }, this.errorService.handler);
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
        this.location.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    facility.Owner = this.internalOrganisation;
  }
}
