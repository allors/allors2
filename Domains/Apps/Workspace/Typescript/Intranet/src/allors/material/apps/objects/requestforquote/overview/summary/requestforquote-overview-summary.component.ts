import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { RequestForQuote, ProductQuote, Quote } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

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
      this.quote = loaded.objects.quote as Quote;
    };
  }
}
