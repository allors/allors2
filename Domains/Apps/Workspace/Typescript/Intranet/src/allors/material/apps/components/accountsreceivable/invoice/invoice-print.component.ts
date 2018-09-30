import { Component, OnDestroy, OnInit, ViewEncapsulation, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, Scope, WorkspaceService, Allors } from '../../../../../angular';
import { SalesInvoice } from '../../../../../domain';
import { Fetch, PullRequest } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog/dialog.service';

@Component({
  encapsulation: ViewEncapsulation.Native,
  styleUrls: ['./invoice-print.component.scss'],
  templateUrl: './invoice-print.component.html',
  providers: [Allors]

})

export class InvoicePrintComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public invoice: SalesInvoice;
  public body: string;

  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { pull, scope } = this.allors;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Invoice({ object: id })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })

      )
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
