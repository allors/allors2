import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Good, InventoryItem, NonSerialisedInventoryItem, Product, RequestItem, SerialisedInventoryItem, UnitOfMeasure, Request, Part, SerialisedItem } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';

import { CreateData, EditData, ObjectData } from '../../../../../material/base/services/object';

@Component({
  templateUrl: './requestitem-edit.component.html',
  providers: [ContextService]
})
export class RequestItemEditComponent implements OnInit, OnDestroy {

  public m: Meta;

  title: string;

  request: Request;
  requestItem: RequestItem;
  goods: Good[];
  unitsOfMeasure: UnitOfMeasure[];
  part: Part;
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];
  private previousProduct;

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
          this.previousProduct = this.requestItem.Product;
          this.serialisedItem = this.requestItem.SerialisedItem;
          this.refreshSerialisedItems(this.requestItem.Product);
        }
      }, this.errorService.handler);
  }

  public goodSelected(good: Product): void {
    if (good) {
      this.refreshSerialisedItems(good);
    }
  }

  public serialisedItemSelected(serialisedItem: SerialisedItem): void {
    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
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

  private refreshSerialisedItems(good: Product): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Good({
        object: good.id,
        fetch: {
          Part: {
            include: {
              SerialisedItems: x
            }
          }
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.part = loaded.objects.Part as Part;
        this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true );

        if (this.requestItem.Product !== this.previousProduct) {
          this.requestItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.requestItem.Product;
        }

      }, this.errorService.handler);
  }
}
