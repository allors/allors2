import { Component, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MetaService, NavigationService, PanelService, RefreshService,  Invoked } from '@allors/angular/services/core';
import { RequestForQuote, Quote } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { ActionTarget } from '@allors/angular/core';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'requestforquote-overview-summary',
  templateUrl: './requestforquote-overview-summary.component.html',
  providers: [PanelService]
})
export class RequestForQuoteOverviewSummaryComponent {

  m: Meta;

  requestForQuote: RequestForQuote;
  quote: Quote;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public snackBar: MatSnackBar,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const requestForQuotePullName = `${panel.name}_${this.m.RequestForQuote.name}`;
    const productQuotePullName = `${panel.name}_${this.m.ProductQuote.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      pulls.push(
        pull.RequestForQuote(
          {
            name: requestForQuotePullName,
            object: this.panel.manager.id,
            include: {
              FullfillContactMechanism: {
                                  PostalAddress_Country: x

              },
              RequestItems: {
                Product: x,
              },
              Originator: x,
              ContactPerson: x,
              RequestState: x,
              Currency: x,
              CreatedBy: x,
              LastModifiedBy: x,
            }
          }),
        pull.RequestForQuote(
          {
            name: productQuotePullName,
            object: this.panel.manager.id,
            fetch: {
              QuoteWhereRequest: x
            }
          }
        )
      );
    };

    panel.onPulled = (loaded) => {
      this.requestForQuote = loaded.objects[requestForQuotePullName] as RequestForQuote;
      this.quote = loaded.objects.Quote as Quote;
    };
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Cancel)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Reject)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public submit(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Submit)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public hold(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Hold)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public createQuote(): void {

    this.panel.manager.context.invoke(this.requestForQuote.CreateQuote)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully created a quote.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }
}

