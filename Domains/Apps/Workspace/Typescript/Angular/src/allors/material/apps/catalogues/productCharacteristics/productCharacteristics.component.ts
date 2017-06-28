import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';
import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { Scope } from '../../../../angular/base/Scope';
import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { ProductCharacteristic } from '../../../../domain';

import { AllorsService } from '../../../.././../app/allors.service';

@Component({
  templateUrl: './productCharacteristics.component.html',
})
export class ProductCharacteristicsComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: ProductCharacteristic[];
  filtered: ProductCharacteristic[];

  constructor(private titleService: Title,
    private router: Router,
    private loadingService: TdLoadingService,
    private dialogService: TdDialogService,
    private snackBarService: MdSnackBar,
    public media: TdMediaService,
    private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.titleService.setTitle('Product Characteristics');
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  search(criteria: string): void {

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m: MetaDomain = this.allors.meta;

    const query: Query[] = [new Query(
      {
        name: 'productCharacteristic',
        objectType: m.ProductCharacteristic,
        include: [
          new TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames }),
        ],
      })];

    this.scope.session.reset();

    this.subscription = this.scope
      .load('Pull', new PullRequest({ query: query }))
      .subscribe(() => {
        this.data = this.scope.collections.productCharacteristic as ProductCharacteristic[];
      },
      (error: any) => {
        alert(error);
        this.goBack();
      });
  }

  delete(productCharacteristic: ProductCharacteristic): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this characteristic?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(productCharacteristic: ProductCharacteristic): void {
    this.router.navigate(['/catalogues/productCharacteristics/' + productCharacteristic.id + '/edit']);
  }
}
