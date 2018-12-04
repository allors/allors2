import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, Saved, SessionService, MetaService } from '../../../../../angular';
import { Good, InventoryItem, NonSerialisedInventoryItem, Product, RequestForQuote, RequestItem, SerialisedInventoryItem, UnitOfMeasure } from '../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './requestitem-edit.component.html',
  providers: [SessionService]
})
export class RequestItemEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public request: RequestForQuote;
  public requestItem: RequestItem;
  public goods: Good[];
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public unitsOfMeasure: UnitOfMeasure[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: SessionService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public stateService: StateService,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const itemId: string = this.route.snapshot.paramMap.get('itemId');

          const pulls = [
            pull.RequestForQuote({ object: id }),
            pull.RequestItem({ object: itemId, include: { RequestItemState: x } }),
            pull.Good({ sort: new Sort(m.Good.Name) }),
            pull.UnitOfMeasure({
              predicate: new Equals({ propertyType: m.UnitOfMeasure.IsActive, value: true }),
              sort: new Sort(m.UnitOfMeasure.Name)
            })
          ];

          return this.allors
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();

        this.request = loaded.objects.requestForQuote as RequestForQuote;
        this.requestItem = loaded.objects.requestItem as RequestItem;
        this.goods = loaded.collections.goods as Good[];
        this.unitsOfMeasure = loaded.collections.unitsOfMeasure as UnitOfMeasure[];
        const piece = this.unitsOfMeasure.find((v: UnitOfMeasure) => v.UniqueId.toUpperCase() === 'F4BBDB52-3441-4768-92D4-729C6C5D6F1B');

        if (!this.requestItem) {
          this.title = 'Add Request Item';
          this.requestItem = this.allors.session.create('RequestItem') as RequestItem;
          this.requestItem.UnitOfMeasure = piece;
          this.request.AddRequestItem(this.requestItem);
        } else {

          if (this.requestItem.CanWriteQuantity) {
            this.title = 'Edit Request Item';
          } else {
            this.title = 'View Request Item';
          }

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

  public cancel(): void {

    const cancelFn: () => void = () => {
      this.allors.invoke(this.requestItem.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.allors.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.allors
              .save()
              .subscribe((saved: Saved) => {
                this.allors.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public save(): void {

    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(['/orders/request/' + this.request.id]);
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

    this.allors
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
