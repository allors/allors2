import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs/Subscription';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { ProductType, SerialisedInventoryItemCharacteristicType } from '../../../../../domain';
import { Fetch, PullRequest, Query, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './producttype.component.html',
})
export class ProductTypeComponent implements OnInit, OnDestroy {

  public title = 'Edit Product Type';
  public subTitle: string;

  public m: MetaDomain;

  public productType: ProductType;

  public characteristics: SerialisedInventoryItemCharacteristicType[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            name: 'productType',
            id,
            include: [
              new TreeNode({ roleType: m.ProductType.SerialisedInventoryItemCharacteristicTypes }),
            ],
          }),
        ];

        const queries: Query[] = [
          new Query(
            {
            name: 'characteristics',
            objectType: m.SerialisedInventoryItemCharacteristicType,
            sort: [new Sort({ roleType: m.SerialisedInventoryItemCharacteristicType.Name, direction: 'Asc' })],
            }),
         ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {

        this.productType = loaded.objects.productType as ProductType;
        if (!this.productType) {
          this.productType = this.scope.session.create('ProductType') as ProductType;
        }

        this.characteristics = loaded.collections.characteristics as SerialisedInventoryItemCharacteristicType[];
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.scope
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
