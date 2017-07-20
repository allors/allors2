import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';
import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { Good } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../angular';

@Component({
  templateUrl: './goods.component.html',
})
export class GoodsComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: Good[];
  filtered: Good[];

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle('Products');
    this.media.broadcast();
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

    const m: MetaDomain = this.allorsService.meta;

    const query: Query[] = [new Query(
      {
        name: 'goods',
        objectType: m.Good,
        include: [
          new TreeNode({ roleType: m.Good.PrimaryPhoto }),
          new TreeNode({ roleType: m.Good.LocalisedNames }),
          new TreeNode({ roleType: m.Good.LocalisedDescriptions }),
          new TreeNode({ roleType: m.Good.PrimaryProductCategory }),
        ],
      })];

    this.scope.session.reset();

    this.subscription = this.scope
      .load('Pull', new PullRequest({ query: query }))
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.goods as Good[];
      },
      (error: any) => {
        alert(error);
        this.goBack();
      });
  }

  delete(good: Good): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this product?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(good: Good): void {
    this.router.navigate(['/catalogues/goods/' + good.id + '/edit']);
  }
}
