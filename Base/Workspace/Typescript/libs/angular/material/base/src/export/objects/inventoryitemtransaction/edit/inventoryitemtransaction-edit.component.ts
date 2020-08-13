import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved } from '@allors/angular/services/core';
import { InventoryItem, InternalOrganisation, InventoryItemTransaction, InventoryTransactionReason, Part, Facility, Lot, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItemState, SerialisedInventoryItemState, NonSerialisedInventoryItem } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { FetcherService } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './inventoryitemtransaction-edit.component.html',
  providers: [ContextService]
})
export class InventoryItemTransactionEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title = 'Add Inventory Item Transaction';

  inventoryItem: InventoryItem;
  internalOrganisation: InternalOrganisation;
  inventoryItemTransaction: InventoryItemTransaction;
  inventoryTransactionReasons: InventoryTransactionReason[];
  selectedPart: Part;
  parts: Part[];
  selectedFacility: Facility;
  addFacility = false;
  facilities: Facility[];
  lots: Lot[];
  serialised: boolean;
  serialisedInventoryItem: SerialisedInventoryItem;
  serialisedItem: SerialisedItem;
  part: Part;
  nonSerialisedInventoryItemState: NonSerialisedInventoryItemState[];
  serialisedInventoryItemState: SerialisedInventoryItemState[];

  private subscription: Subscription;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<InventoryItemTransactionEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Facility({ sort: new Sort(m.Facility.Name) }),
            pull.Part(),
            pull.InventoryTransactionReason(),
            pull.NonSerialisedInventoryItemState(),
            pull.SerialisedInventoryItemState(),
            pull.Lot({ sort: new Sort(m.Lot.LotNumber) }),
            pull.InventoryItem({
              object: this.data.associationId,
              include: {
                Facility: x,
                UnitOfMeasure: x,
                Lot: x,
                Part: {
                  InventoryItemKind: x,
                  PartWeightedAverage: x,
                }
              }
            }),
            pull.Part({
              object: this.data.associationId,
              include: {
                PartWeightedAverage: x,
              }
            }),
            pull.SerialisedItem({
              object: this.data.associationId,
            }),
            pull.SerialisedItem({
              object: this.data.associationId,
              fetch: {
                PartWhereSerialisedItem: {
                  include: {
                    PartWeightedAverage: x,
                  }
                }
              }
            })
          ];

          return this.allors.context.load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded }))
            );
        })
      )
      .subscribe(({ loaded }) => {

        this.allors.context.reset();

        this.inventoryTransactionReasons = loaded.collections.InventoryTransactionReasons as InventoryTransactionReason[];
        this.nonSerialisedInventoryItemState = loaded.collections.NonSerialisedInventoryItemStates as NonSerialisedInventoryItemState[];
        this.serialisedInventoryItemState = loaded.collections.SerialisedInventoryItemStates as SerialisedInventoryItemState[];
        this.part = loaded.objects.Part as Part;
        this.parts = loaded.collections.Parts as Part[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.lots = loaded.collections.Lots as Lot[];
        this.serialisedItem = loaded.objects.SerialisedItem as SerialisedItem;
        this.inventoryItem = loaded.objects.InventoryItem as InventoryItem;

        if (this.part) {
          this.selectedPart = this.part;
        }

        if (this.inventoryItem) {
          this.serialisedInventoryItem = loaded.objects.InventoryItem as SerialisedInventoryItem;
          this.nonSerialisedInventoryItem = loaded.objects.InventoryItem as NonSerialisedInventoryItem;
          this.part = this.inventoryItem.Part;
          this.selectedFacility = this.inventoryItem.Facility;
          this.serialised = this.inventoryItem.Part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';
        }

        this.inventoryItemTransaction = this.allors.context.create('InventoryItemTransaction') as InventoryItemTransaction;
        this.inventoryItemTransaction.TransactionDate = new Date().toISOString();
        this.inventoryItemTransaction.Part = this.part;
        this.inventoryItemTransaction.Cost = this.part.PartWeightedAverage?.AverageCost;

        if (this.inventoryItem) {
          this.inventoryItemTransaction.Facility = this.inventoryItem.Facility;
          this.inventoryItemTransaction.UnitOfMeasure = this.inventoryItem.UnitOfMeasure;
          this.inventoryItemTransaction.Lot = this.inventoryItem.Lot;

          if (this.serialised) {
            this.inventoryItemTransaction.SerialisedItem = this.serialisedInventoryItem.SerialisedItem;
            this.inventoryItemTransaction.SerialisedInventoryItemState = this.serialisedInventoryItem.SerialisedInventoryItemState;
          } else {
            this.inventoryItemTransaction.NonSerialisedInventoryItemState = this.nonSerialisedInventoryItem.NonSerialisedInventoryItemState;
          }
        }

        if (this.serialisedItem) {
          this.inventoryItemTransaction.SerialisedItem = this.serialisedItem;
          this.serialised = true;
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.inventoryItemTransaction.Facility = this.selectedFacility;

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.inventoryItemTransaction.id,
          objectType: this.inventoryItemTransaction.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
  }
}
