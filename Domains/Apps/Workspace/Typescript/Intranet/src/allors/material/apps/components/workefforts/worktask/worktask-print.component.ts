import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { ErrorService, Loaded, Scope, WorkspaceService, DataService } from '../../../../../angular';
import { SalesOrder } from '../../../../../domain';
import { Fetch, PullRequest } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  encapsulation: ViewEncapsulation.Native,
  styleUrls: ['./worktask-print.component.scss'],
  templateUrl: './worktask-print.component.html',
})

export class WorkTaskPrintComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public order: SalesOrder;
  public body: string;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.SalesOrder({
              object: id,
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.order = loaded.objects.order as SalesOrder;
        const htmlContent = this.order.HtmlContent;

        const wrapper = document.createElement('div');
        wrapper.innerHTML = htmlContent;
        this.body = wrapper.querySelector('#dataContainer').innerHTML;
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

  public goBack(): void {
    window.history.back();
  }
}
