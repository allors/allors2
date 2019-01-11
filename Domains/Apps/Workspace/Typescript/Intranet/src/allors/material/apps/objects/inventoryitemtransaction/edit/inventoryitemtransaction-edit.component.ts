import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService, Saved } from '../../../../../angular';
import { InternalOrganisation, InventoryItem, InventoryItemTransaction, InventoryTransactionReason, Part, Facility, Lot } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

import { CreateData, EditData, ObjectData } from '../../../../../material/base/services/object';
import { Fetcher } from '../../Fetcher';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './inventoryitemtransaction-edit.component.html',
  providers: [ContextService]
})
export class InventoryItemTransactionEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  inventoryItem: InventoryItem;
  internalOrganisation: InternalOrganisation;
  inventoryItemTransaction: InventoryItemTransaction;
  part: Part;
  inventoryTransactionReasons: InventoryTransactionReason[];
  selectedFacility: Facility;
  addFacility = false;
  facilities: Facility[];
  lots: Lot[];
  serialised: boolean;

  private subscription: Subscription;
  private readonly refresh$: BehaviorSubject<Date>;
  private readonly fetcher: Fetcher;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<InventoryItemTransactionEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public stateService: StateService,
    private errorService: ErrorService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const create = (this.data as EditData).id === undefined;

          let pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.facilities,
            pull.InventoryTransactionReason(),
            pull.Lot({ sort: new Sort(m.Lot.LotNumber) })
          ];

          // InventoryItemTransactions are always added, never edited
          if (create && this.data.associationId) {
            pulls = [
              ...pulls,
              pull.InventoryItem({
                object: this.data.associationId,
                include: {
                  Facility: x,
                  UnitOfMeasure: x,
                  Lot: x,
                  Part: {
                    InventoryItemKind: x
                  }
                }
              })
            ];
          }

          return this.allors.context.load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create }))
            );
        })
      )
      .subscribe(({ loaded, create }) => {

        this.allors.context.reset();

        this.inventoryTransactionReasons = loaded.collections.InventoryTransactionReasons as InventoryTransactionReason[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.lots = loaded.collections.Lots as Lot[];

        this.inventoryItem = loaded.objects.InventoryItem as InventoryItem;
        this.serialised = this.inventoryItem.Part.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase();

        if (create) {

          this.inventoryItemTransaction = this.allors.context.create('InventoryItemTransaction') as InventoryItemTransaction;

          if (this.inventoryItem) {
            this.inventoryItemTransaction.Facility = this.inventoryItem.Facility;
            this.inventoryItemTransaction.UnitOfMeasure = this.inventoryItem.UnitOfMeasure;
            this.inventoryItemTransaction.Lot = this.inventoryItem.Lot;
            this.inventoryItemTransaction.TransactionDate = new Date();
          }

          if (this.part) {
            this.inventoryItemTransaction.Part = this.part;
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe((saved: Saved) => {
        const data: ObjectData = {
          id: this.inventoryItem.id,
          objectType: this.inventoryItem.objectType,
        };

        this.dialogRef.close(data);
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
