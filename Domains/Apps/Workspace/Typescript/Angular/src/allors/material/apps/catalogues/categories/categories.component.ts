import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';
import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { ProductCategory } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../angular';

@Component({
  templateUrl: './categories.component.html',
})
export class CategoriesComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: ProductCategory[];
  filtered: ProductCategory[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService) {

    this.scope = new Scope(allors.database, allors.workspace);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle('Categories');
    this.search();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  search(criteria?: string): void {

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m: MetaDomain = this.allors.meta;

    const query: Query[] = [new Query(
      {
        name: 'categories',
        objectType: m.ProductCategory,
        include: [
          new TreeNode({ roleType: m.ProductCategory.CategoryImage }),
          new TreeNode({ roleType: m.ProductCategory.LocalisedNames }),
          new TreeNode({ roleType: m.ProductCategory.LocalisedDescriptions }),
        ],
      })];

    this.scope.session.reset();

    this.subscription = this.scope
      .load('Pull', new PullRequest({ query: query }))
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.categories as ProductCategory[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  delete(category: ProductCategory): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this category?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(category: ProductCategory): void {
    this.router.navigate(['/catalogues/categories/' + category.id + '/edit']);
  }
}
