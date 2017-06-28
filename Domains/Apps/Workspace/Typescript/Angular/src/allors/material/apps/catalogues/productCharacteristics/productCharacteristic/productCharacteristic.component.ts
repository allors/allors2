import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { Scope } from '../../../../../angular/base/Scope';
import { MetaDomain } from '../../../../../meta';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { ProductCharacteristic, Singleton, Locale } from '../../../../../domain';

import { AllorsService } from '../../../../../../app/allors.service';

@Component({
  templateUrl: './productCharacteristic.component.html',
})
export class ProductCharacteristicFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  flex: string = '1 1 30rem';
  m: MetaDomain;

  productCharacteristic: ProductCharacteristic;

  singleton: Singleton;
  locales: Locale[];

  constructor(private allors: AllorsService,
    private route: ActivatedRoute,
    public snackBar: MdSnackBar,
    public media: TdMediaService) {
    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'productCharacteristic',
            id: id,
            include: [
              new TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'singletons',
              objectType: this.m.Singleton,
              include: [
                new TreeNode({ roleType: m.Singleton.Locales }),
              ],
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe(() => {

        this.productCharacteristic = this.scope.objects.productCharacteristic as ProductCharacteristic;
        if (!this.productCharacteristic) {
          this.productCharacteristic = this.scope.session.create('ProductCharacteristic') as ProductCharacteristic;
        }

        this.singleton = this.scope.collections.singletons[0] as Singleton;
        this.locales = this.singleton.Locales;
      },
      (error: any) => {
        this.snackBar.open(error, 'close', { duration: 5000 });
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {

    this.scope
      .save()
      .subscribe((pushResponse: PushResponse) => {
        this.goBack();
      },
      (e: any) => {
        this.snackBar.open(e.toString(), 'close', { duration: 5000 });
      });
  }

  goBack(): void {
    window.history.back();
  }
}
