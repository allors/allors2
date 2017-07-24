import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy , ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig, MdNativeDateModule } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta/index';
import { PullRequest, PushResponse, Contains, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Good, Organisation, OrganisationRole, ProductCharacteristicValue, ProductCharacteristic, ProductCategory, ProductType, Locale, LocalisedText, Singleton } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Filter } from '../../../../../angular';

@Component({
  templateUrl: './good.component.html',
})
export class GoodFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  
  

  m: MetaDomain;

  good: Good;

  title: string;
  singleton: Singleton;
  locales: Locale[];
  selectedLocaleName: string;
  categories: ProductCategory[];
  productTypes: ProductType[];
  productCharacteristicValues: ProductCharacteristicValue[];
  manufacturers: Organisation[];
  actualQuantityOnHand: number;

  manufacturersFilter: Filter;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.manufacturersFilter = new Filter(this.scope, this.m.Organisation, [this.m.Organisation.Name]);
  }

  get locale(): Locale {
    return this.locales.find((v: Locale) => v.Name === this.selectedLocaleName);
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'good',
            id: id,
            include: [
              new TreeNode({ roleType: m.Good.PrimaryPhoto }),
              new TreeNode({ roleType: m.Product.LocalisedNames }),
              new TreeNode({ roleType: m.Product.LocalisedDescriptions }),
              new TreeNode({ roleType: m.Product.LocalisedComments }),
              new TreeNode({ roleType: m.Product.ProductCategories }),
              new TreeNode({ roleType: m.Good.ManufacturedBy }),
              new TreeNode({
                roleType: m.Good.ProductType,
                nodes: [
                  new TreeNode({
                    roleType: m.ProductType.ProductCharacteristics,
                    nodes: [new TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames })],
                  }),
                ],
              }),
              new TreeNode({
                roleType: m.Product.ProductCharacteristicValues,
                nodes: [
                  new TreeNode({ roleType: m.ProductCharacteristicValue.ProductCharacteristic }),
                ],
              }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'singletons',
              objectType: this.m.Singleton,
              include: [
                new TreeNode({
                  roleType: m.Singleton.Locales,
                  nodes: [
                    new TreeNode({ roleType: m.Locale.Language }),
                    new TreeNode({ roleType: m.Locale.Country }),
                  ],
                }),
              ],
            }),
          new Query(
            {
              name: 'organisationRoles',
              objectType: this.m.OrganisationRole,
            }),
          new Query(
            {
              name: 'categories',
              objectType: this.m.ProductCategory,
            }),
          new Query(
            {
              name: 'productTypes',
              objectType: this.m.ProductType,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }))
          .switchMap((loaded: Loaded) => {

            this.good = loaded.objects.good as Good;
            if (!this.good) {
              this.good = this.scope.session.create('Good') as Good;
            }

            this.title = this.good.Name;
            this.actualQuantityOnHand = this.good.QuantityOnHand;
            this.categories = loaded.collections.categories as ProductCategory[];
            this.productTypes = loaded.collections.productTypes as ProductType[];
            this.singleton = loaded.collections.singletons[0] as Singleton;
            this.locales = this.singleton.Locales;
            this.selectedLocaleName = this.singleton.DefaultLocale.Name;

            this.setProductCharacteristicValues();

            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const customerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === 'Manufacturer');

            const manufacturersQuery: Query[] = [new Query(
              {
                name: 'manufacturers',
                objectType: m.Organisation,
                predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: customerRole }),
              })];

            return this.scope.load('Pull', new PullRequest({ query: manufacturersQuery }));
          });
      })
      .subscribe((loaded: Loaded) => {
        this.manufacturers = loaded.collections.manufacturers as Organisation[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  goBack(): void {
    window.history.back();
  }

  localisedName(productCharacteristic: ProductCharacteristic): string {
    const localisedText: LocalisedText = productCharacteristic.LocalisedNames.find((v: LocalisedText) => v.Locale === this.locale);
    if (localisedText) {
      return localisedText.Text;
    }

    return productCharacteristic.Name;
  }

  setProductCharacteristicValues(): void {
    this.productCharacteristicValues = this.good.ProductCharacteristicValues.filter((v: ProductCharacteristicValue) => v.Locale === this.locale);
  }
}
