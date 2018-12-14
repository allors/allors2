import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { ErrorService, Loaded, Saved, ContextService, MetaService } from '../../../../../angular';
import { ProductType, SerialisedItemCharacteristicType } from '../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './producttype-edit.component.html',
  providers: [ContextService]
})
export class ProductTypeComponent implements OnInit, OnDestroy {

  public title = 'Edit Product Type';
  public subTitle: string;

  public m: Meta;

  public productType: ProductType;

  public characteristics: SerialisedItemCharacteristicType[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.ProductType({
              object: id,
              include: {
                SerialisedItemCharacteristicTypes: x,
              }
            }),
            pull.SerialisedItemCharacteristicType({
              sort: new Sort(m.SerialisedItemCharacteristicType.Name),
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.productType = loaded.objects.productType as ProductType;
        if (!this.productType) {
          this.productType = this.allors.context.create('ProductType') as ProductType;
        }

        this.characteristics = loaded.collections.characteristics as SerialisedItemCharacteristicType[];
      }, this.errorService.handler);
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
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
