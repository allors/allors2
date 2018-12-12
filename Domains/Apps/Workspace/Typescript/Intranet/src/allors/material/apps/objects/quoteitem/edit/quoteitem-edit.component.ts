import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatSnackBar } from '@angular/material';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Invoked, Saved, SearchFactory, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Good, InventoryItem, NonSerialisedInventoryItem, Product, ProductQuote, QuoteItem, RequestItem, SerialisedInventoryItem, UnitOfMeasure } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { CreateData, EditData, ObjectData } from 'src/allors/angular/base/object/object.data';

@Component({
  templateUrl: './quoteitem-edit.component.html',
  providers: [ContextService]
})
export class QuoteItemEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title = 'Edit Quote Item';
  public subTitle: string;
  public quote: ProductQuote;
  public quoteItem: QuoteItem;
  public requestItem: RequestItem;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public goods: Good[];
  public unitsOfMeasure: UnitOfMeasure[];

  public goodsFilter: SearchFactory;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<QuoteItemEditComponent>,
    public metaService: MetaService,
    private errorService: ErrorService,
    public stateService: StateService,
    private dialogService: AllorsMaterialDialogService,
    public refreshService: RefreshService,
    public snackBar: MatSnackBar
  ) {
    this.m = this.metaService.m;

    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refresh$)
      .pipe(
        switchMap(([]) => {

          const create = (this.data as EditData).id === undefined;

          const pulls = [
            pull.QuoteItem(
              {
                object: this.data.id,
                include: {
                  QuoteItemState: x,
                  RequestItem: x,
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
        const piece = this.unitsOfMeasure.find((v: UnitOfMeasure) => v.UniqueId.toUpperCase() === 'F4BBDB52-3441-4768-92D4-729C6C5D6F1B');

        if (create) {
          this.title = 'Add Quote Item';
          this.quoteItem = this.allors.context.create('QuoteItem') as QuoteItem;
          this.quoteItem.UnitOfMeasure = piece;
          this.quote.AddQuoteItem(this.quoteItem);
        } else {
          this.update(this.quoteItem.Product);
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(object: any) {
    if (object) {
      this.update(object as Product);
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe((saved: Saved) => {
        const data: ObjectData = {
          id: this.quoteItem.id,
          objectType: this.quoteItem.objectType,
        };

        this.dialogRef.close(data);
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

  private update(product: Product) {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Good(
        {
          object: product,
          // TODO:
          // fetch: {
          //   InventoryItemsWhereGood: x
          // }
        }
      )
    ];

    this.allors.context.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        // this.inventoryItems = loaded.collections.inventoryItem as InventoryItem[];
        // if (this.inventoryItems[0].objectType.name === 'SerialisedInventoryItem') {
        //   this.serialisedInventoryItem = this.inventoryItems[0] as SerialisedInventoryItem;
        // }
        // if (this.inventoryItems[0].objectType.name === 'NonSerialisedInventoryItem') {
        //   this.nonSerialisedInventoryItem = this.inventoryItems[0] as NonSerialisedInventoryItem;
        // }
      }, this.errorService.handler);
  }
}
