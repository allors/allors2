import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, RefreshService, Invoked, ErrorService } from '../../../../../../angular';
import { RequestForQuote, ProductQuote, Quote } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material';

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
    public snackBar: MatSnackBar,
    public navigation: NavigationService,
    public errorService: ErrorService) {

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
                PostalAddress_PostalBoundary: {
                  Country: x,
                }
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
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Reject)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public submit(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Submit)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public hold(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Hold)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public createQuote(): void {

    this.panel.manager.context.invoke(this.requestForQuote.CreateQuote)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully created a quote.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}

