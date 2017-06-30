import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';
import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { Catalogue } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded } from '../../../../angular';

@Component({
  templateUrl: './catalogues.component.html',
})
export class CataloguesComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: Catalogue[];
  filtered: Catalogue[];

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    public dialogService: TdDialogService,
    public media: TdMediaService,
    ) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.titleService.setTitle('Catalogues');
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

    const m: MetaDomain = this.allorsService.meta;

    const query: Query[] = [new Query(
      {
        name: 'catalogues',
        objectType: m.Catalogue,
        include: [
          new TreeNode({ roleType: m.Catalogue.CatalogueImage }),
          new TreeNode({ roleType: m.Catalogue.ProductCategories }),
        ],
      })];

    this.scope.session.reset();

    this.subscription = this.scope
      .load('Pull', new PullRequest({ query: query }))
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.catalogues as Catalogue[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  delete(catalogue: Catalogue): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this catalogue?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(catalogue: Catalogue): void {
    this.router.navigate(['/catalogues/catalogues/' + catalogue.id + '/edit']);
  }
}
