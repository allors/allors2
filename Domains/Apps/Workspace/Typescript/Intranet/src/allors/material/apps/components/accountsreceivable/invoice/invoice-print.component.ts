import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';
import { SalesInvoice } from '../../../../../domain';
import { Fetch, PullRequest } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog/dialog.service';

@Component({
  encapsulation: ViewEncapsulation.Native,
  styleUrls: ['./invoice-print.component.scss'],
  templateUrl: './invoice-print.component.html',
})

export class InvoicePrintComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public invoice: SalesInvoice;
  public body: string;

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
            id,
            name: 'invoice',
          }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches }));
      })
      .subscribe((loaded) => {
        this.invoice = loaded.objects.invoice as SalesInvoice;
        const htmlContent = this.invoice.HtmlContent;

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
