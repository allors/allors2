import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { SearchFactory, ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { InventoryItem, NonSerialisedInventoryItem, Product, ProductQuote, QuoteItem, RequestItem, SerialisedInventoryItem, UnitOfMeasure, SerialisedItem, Part, Good } from '../../../../../domain';
import { ObjectData } from '../../../../../material/base/services/object';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { SaveService, FiltersService } from '../../../../../material';

@Component({
  templateUrl: './quoteitem-edit.component.html',
  providers: [ContextService]
})
export class QuoteItemEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  quote: ProductQuote;
  quoteItem: QuoteItem;
  requestItem: RequestItem;
  inventoryItems: InventoryItem[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  unitsOfMeasure: UnitOfMeasure[];
  goodsFilter: SearchFactory;
  part: Part;
  parts: Part[];
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];

  private previousProduct;
  private subscription: Subscription;
  goods: Good[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<QuoteItemEditComponent>,
    public metaService: MetaService,
    private saveService: SaveService,
    public refreshService: RefreshService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const create = (this.data as IObject).id === undefined;

          const pulls = [
            pull.QuoteItem(
              {
                object: this.data.id,
                include: {
                  QuoteItemState: x,
                  RequestItem: x,
                  Product: x,
                  SerialisedItem: x
                }
              }
            ),
            pull.QuoteItem(
              {
                object: this.data.id,
                fetch: {
                  RequestItem: x
                }
              }
            ),
            pull.Good(
              {
                sort: new Sort(m.Good.Name),
              }
            ),
            pull.UnitOfMeasure({
              predicate: new Equals({ propertyType: m.UnitOfMeasure.IsActive, value: true }),
              sort: [
                new Sort(m.UnitOfMeasure.Name),
              ],
            })
          ];

          if (create && this.data.associationId) {
            pulls.push(
              pull.ProductQuote({
                object: this.data.associationId
              }),
            );
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create }))
            );
        })
      )
      .subscribe(({ loaded, create }) => {
        this.allors.context.reset();

        this.quote = loaded.objects.ProductQuote as ProductQuote;
        this.quoteItem = loaded.objects.QuoteItem as QuoteItem;
        this.requestItem = loaded.objects.RequestItem as RequestItem;
        this.goods = loaded.collections.Goods as Good[];
        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        const piece = this.unitsOfMeasure.find((v: UnitOfMeasure) => v.UniqueId === 'f4bbdb523441476892d4729c6c5d6f1b');

        if (create) {
          this.title = 'Add Quote Item';
          this.quoteItem = this.allors.context.create('QuoteItem') as QuoteItem;
          this.quoteItem.UnitOfMeasure = piece;
          this.quote.AddQuoteItem(this.quoteItem);
        } else {

          if (this.quoteItem.Product) {
            this.previousProduct = this.quoteItem.Product;
            this.refreshSerialisedItems(this.quoteItem.Product);
          } else {
            this.serialisedItems.push(this.quoteItem.SerialisedItem);
          }

          if (this.quoteItem.CanWriteQuantity) {
            this.title = 'Edit Quote Item';
          } else {
            this.title = 'View Quote Item';
          }
        }
      });
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

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.quoteItem.id,
          objectType: this.quoteItem.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }

  private refreshSerialisedItems(product: Product): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.NonUnifiedGood({
        object: product.id,
        fetch: {
          Part: {
            include: {
              SerialisedItems: x
            }
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
        this.part = (loaded.objects.UnifiedGood || loaded.objects.Part) as Part;
        this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true);

        if (this.quoteItem.Product !== this.previousProduct) {
          this.quoteItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.quoteItem.Product;
        }

      });
  }
}
