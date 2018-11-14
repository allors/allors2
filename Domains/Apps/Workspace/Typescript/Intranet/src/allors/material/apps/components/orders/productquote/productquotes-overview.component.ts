import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, PdfService, Scope, WorkspaceService, Allors, x } from '../../../../../angular';
import { InternalOrganisation, ProductQuote, QuoteState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Predicate, PullRequest, Sort, TreeNode, Filter } from '../../../../../framework';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

interface SearchData {
  company: string;
  description: string;
  quoteNumber: string;
  state: string;
}

@Component({
  templateUrl: './productquotes-overview.component.html',
  providers: [Allors]
})
export class ProductQuotesOverviewComponent implements OnInit, OnDestroy {
  public searchForm: FormGroup; public advancedSearch: boolean;

  public title = 'Quotes';
  public data: ProductQuote[];
  public filtered: ProductQuote[];
  public total: number;

  public internalOrganisations: InternalOrganisation[];
  public selectedInternalOrganisation: InternalOrganisation;

  public quoteStates: QuoteState[];
  public selectedQuoteState: QuoteState;
  public quoteState: QuoteState;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    public pdfService: PdfService,
    private stateService: StateService) {

    titleService.setTitle(this.title);

    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      company: [''],
      description: [''],
      quoteNumber: [''],
      state: [''],
    });
  }

  ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
          return [data, date, internalOrganisationId];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data, , internalOrganisationId]) => {

          const pulls = [
            pull.QuoteState(
              {
                sort: new Sort(m.QuoteState.Name)
              }
            ),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded: Loaded) => {
                this.quoteStates = loaded.collections.quoteStates as QuoteState[];
                this.quoteState = this.quoteStates.find((v: QuoteState) => v.Name === data.state);

                this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                predicates.push(new Equals({ propertyType: m.ProductQuote.Issuer, value: internalOrganisationId }));

                if (data.quoteNumber) {
                  const like: string = '%' + data.quoteNumber + '%';
                  predicates.push(new Like({ roleType: m.ProductQuote.QuoteNumber, value: like }));
                }

                if (data.company) {
                  const partyQuery = new Filter({
                    objectType: m.Party, predicate: new Like({
                      roleType: m.Party.PartyName, value: data.company.replace('*', '%') + '%',
                    }),
                  });

                  const containedIn: ContainedIn = new ContainedIn({ propertyType: m.ProductQuote.Receiver, extent: partyQuery });
                  predicates.push(containedIn);
                }

                if (data.description) {
                  const like: string = data.description.replace('*', '%') + '%';
                  predicates.push(new Like({ roleType: m.ProductQuote.Description, value: like }));
                }

                if (data.state) {
                  predicates.push(new Equals({ propertyType: m.ProductQuote.QuoteState, object: this.quoteState }));
                }

                const queries = [
                  pull.ProductQuote(
                    {
                      predicate,
                      include: {
                        Receiver: x,
                        QuoteState: x,
                      },
                      sort: new Sort(m.ProductQuote.QuoteNumber)
                    }
                  )
                ];

                return scope.load('Pull', new PullRequest({ pulls: queries }));
              })
            );
        })
      )
      .subscribe((loaded) => {
        this.data = loaded.collections.quotes as ProductQuote[];
        this.total = loaded.values.invoices_total;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public print(quote: ProductQuote) {
    this.pdfService.display(quote);
  }

  public goBack(): void {
    window.history.back();
  }

  public onView(quote: ProductQuote): void {
    this.router.navigate(['/orders/productQuotes/' + quote.id]);
  }
}
