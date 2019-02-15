import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, Invoked, RefreshService, ErrorService, Action } from '../../../../../../angular';
import { ProductQuote, SalesOrder, RequestForQuote } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material';
import { PrintService } from '../../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'productquote-overview-summary',
  templateUrl: './productquote-overview-summary.component.html',
  providers: [PanelService]
})
export class ProductQuoteOverviewSummaryComponent {

  m: Meta;

  productQuote: ProductQuote;
  salesOrder: SalesOrder;
  request: RequestForQuote;
  print: Action;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public printService: PrintService,
    public refreshService: RefreshService,
    public snackBar: MatSnackBar,
    public errorService: ErrorService) {

    this.m = this.metaService.m;

    this.print = printService.print();

    panel.name = 'summary';

    const productQuotePullName = `${panel.name}_${this.m.ProductQuote.name}`;
    const salesOrderPullName = `${panel.name}_${this.m.SalesOrder.name}`;
    const requestPullName = `${panel.name}_${this.m.RequestForQuote.name}`;

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
              },
              PrintDocument: {
                Media: x,
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
        ),
        pull.ProductQuote(
          {
            name: requestPullName,
            object: this.panel.manager.id,
            fetch: {
              Request: x,
            }
          }
        )
      );
    };

    panel.onPulled = (loaded) => {
      this.productQuote = loaded.objects[productQuotePullName] as ProductQuote;
      this.salesOrder = loaded.objects[salesOrderPullName] as SalesOrder;
      this.request = loaded.objects[requestPullName] as RequestForQuote;
    };
  }

  public approve(): void {

    this.panel.manager.context.invoke(this.productQuote.Approve)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public order(): void {

    this.panel.manager.context.invoke(this.productQuote.Order)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully created a salesorder.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.productQuote.Cancel)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.productQuote.Reject)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public Order(): void {

    this.panel.manager.context.invoke(this.productQuote.Order)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('SalesOrder successfully created.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
