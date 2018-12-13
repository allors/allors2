import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Good, InventoryItem, NonSerialisedInventoryItem, Product, RequestForQuote, RequestItem, SerialisedInventoryItem, UnitOfMeasure, Request } from '../../../../../domain';
import { PullRequest, Sort, Equals, Pull } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';

import { CreateData, EditData, ObjectData } from '../../../../../../allors/angular/base/object/object.data';

@Component({
  templateUrl: './requestitem-edit.component.html',
  providers: [ContextService]
})
export class RequestItemEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string;

  public request: Request;
  public requestItem: RequestItem;
  public goods: Good[];
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public unitsOfMeasure: UnitOfMeasure[];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<RequestItemEditComponent>,
    public metaService: MetaService,
    private errorService: ErrorService,
    public stateService: StateService,
    public refreshService: RefreshService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          const pulls = [
            pull.RequestItem({
              object: this.data.id,
              include: { RequestItemState: x }
            }),
            pull.Good({
              sort: new Sort(m.Good.Name)
            }),
            pull.UnitOfMeasure({
              predicate: new Equals({ propertyType: m.UnitOfMeasure.IsActive, value: true }),
              sort: new Sort(m.UnitOfMeasure.Name)
            })
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Request({
                object: this.data.associationId
              })
            );
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.requestItem = loaded.objects.RequestItem as RequestItem;
        this.goods = loaded.collections.Goods as Good[];
        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        const piece = this.unitsOfMeasure.find((v: UnitOfMeasure) => v.UniqueId.toUpperCase() === 'F4BBDB52-3441-4768-92D4-729C6C5D6F1B');

        if (isCreate) {
          this.title = 'Create Request Item';
          this.request = loaded.objects.Request as Request;
          this.requestItem = this.allors.context.create('RequestItem') as RequestItem;
          this.requestItem.UnitOfMeasure = piece;
          this.request.AddRequestItem(this.requestItem);
        } else {
          this.title = 'Edit Request Item';
          this.refreshInventory(this.requestItem.Product);
        }
      }, this.errorService.handler);
  }

  public goodSelected(object: any): void {
    if (object) {
      this.refreshInventory(object as Product);
    }
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: ObjectData = {
          id: this.requestItem.id,
          objectType: this.requestItem.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  private refreshInventory(product: Product): void {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Good({
        object: product,
        // TODO:
        // fetch: {
        //   InventoryItemsWhereGood
        // }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.inventoryItems = loaded.collections.inventoryItem as InventoryItem[];
        if (this.inventoryItems[0].objectType.name === 'SerialisedInventoryItem') {
          this.serialisedInventoryItem = this.inventoryItems[0] as SerialisedInventoryItem;
        }
        if (this.inventoryItems[0].objectType.name === 'NonSerialisedInventoryItem') {
          this.nonSerialisedInventoryItem = this.inventoryItems[0] as NonSerialisedInventoryItem;
        }
      }, this.errorService.handler);
  }
}
