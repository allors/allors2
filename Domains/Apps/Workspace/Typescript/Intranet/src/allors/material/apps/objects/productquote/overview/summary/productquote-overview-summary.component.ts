import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, Invoked, RefreshService, ErrorService } from '../../../../../../angular';
import { ProductQuote, Quote, Good, SalesOrder } from '../../../../../../domain';
import { MetaDomain } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'productquote-overview-summary',
  templateUrl: './productquote-overview-summary.component.html',
  providers: [PanelService]
})
export class ProductQuoteOverviewSummaryComponent {

  m: MetaDomain;

  public productQuote: ProductQuote;
  public salesOrder: SalesOrder;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public refreshService: RefreshService,
    public snackBar: MatSnackBar,
    public errorService: ErrorService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const productQuotePullName = `${panel.name}_${this.m.ProductQuote.objectType.name}`;
    const salesOrderPullName = `${panel.name}_${this.m.SalesOrder.objectType.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      pulls.push(
        pull.ProductQuote(
          {
            name: productQuotePullName,
            object: this.panel.manager.id,
            include: {
              QuoteItems: {
                Product: x,
                QuoteItemState: x,
              },
              Receiver: x,
              ContactPerson: x,
              QuoteState: x,
              CreatedBy: x,
              LastModifiedBy: x,
              Request: x,
              FullfillContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x,
                }
              }
            }
          }),
        pull.ProductQuote(
          {
            name: salesOrderPullName,
            object: this.panel.manager.id,
            fetch: {
              SalesOrderWhereQuote: x,
            }
          }
        )
      );
    };

    panel.onPulled = (loaded) => {
      this.productQuote = loaded.objects[productQuotePullName] as ProductQuote;
      this.salesOrder = loaded.objects[salesOrderPullName] as SalesOrder;
    };
  }

  public approve(): void {

    this.panel.manager.context.invoke(this.productQuote.Approve)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.productQuote.Reject)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public Order(): void {

    this.panel.manager.context.invoke(this.productQuote.Order)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('SalesOrder successfully created.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
