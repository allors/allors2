import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Product, RequestItem, UnitOfMeasure, Request, Part, SerialisedItem, Good } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { CreateData } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';


@Component({
  templateUrl: './requestitem-edit.component.html',
  providers: [ContextService]
})
export class RequestItemEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;

  request: Request;
  requestItem: RequestItem;
  unitsOfMeasure: UnitOfMeasure[];
  parts: Part[];
  part: Part;
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];

  private previousProduct;
  private subscription: Subscription;
  goods: Good[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
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

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.RequestItem({
              object: this.data.id,
              include: {
                RequestItemState: x,
                Product: x,
                SerialisedItem: x
              }
            }),
            pull.Good(
              {
                sort: new Sort(m.Good.Name),
              }
            ),
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

          if (this.requestItem.CanWriteQuantity) {
            this.title = 'Edit Request Item';
          } else {
            this.title = 'View Request Item';
          }

          if (this.requestItem.Product) {
            this.previousProduct = this.requestItem.Product;
            this.refreshSerialisedItems(this.requestItem.Product);
          } else {
            this.serialisedItems.push(this.requestItem.SerialisedItem);
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(product: Product): void {
    if (product) {
      this.refreshSerialisedItems(product);
    }
  }

  public serialisedItemSelected(serialisedItem: SerialisedItem): void {
    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.requestItem.id,
          objectType: this.requestItem.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  private refreshSerialisedItems(product: Product): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.NonUnifiedGood({
        object: product.id,
        fetch: {
          Part: {
            SerialisedItems: x
          }
        }
      }),
      pull.UnifiedGood({
        object: product.id,
        include: {
          SerialisedItems: x
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.part = (loaded.objects.UnifiedGood || loaded.objects.Parts) as Part;
        this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true);

        if (this.requestItem.Product !== this.previousProduct) {
          this.requestItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.requestItem.Product;
        }

      }, this.errorService.handler);
  }
}
