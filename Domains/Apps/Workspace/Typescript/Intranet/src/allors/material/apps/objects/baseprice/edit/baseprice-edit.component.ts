import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationActivatedRoute, NavigationService, MetaService } from '../../../../../angular';
import { Good, Part, PriceComponent, InternalOrganisation } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './baseprice-edit.component.html',
  providers: [ContextService]
})
export class EditBasepriceComponent implements OnInit, OnDestroy {

  title = 'Default price';

  add: boolean;
  edit: boolean;

  m: MetaDomain;

  good: Good;
  part: Part;
  priceComponent: PriceComponent;
  item: Good | Part;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;
  internalOrganisation: InternalOrganisation;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService,
  ) {
    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const goodId = navRoute.queryParam(m.Good);
          const partId = navRoute.queryParam(m.Part);

          let pulls = [
            this.fetcher.internalOrganisation,
          ];

          if (!!goodId) {
            pulls = [
              ...pulls,
              pull.Good({
                object: goodId,
              })
            ];
          }

          if (!!partId) {
            pulls = [
              ...pulls,
              pull.Part({
                object: partId,
              }),
            ];
          }

          const add = !id;

          if (!add) {
            pulls = [
              ...pulls,
              pull.PriceComponent({
                object: id,
                include: {
                  Currency: x,
                }
              }),
            ];
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {

        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.good = loaded.objects.Good as Good;
        this.part = loaded.objects.Part as Part;

        if (add) {
          this.add = !(this.edit = false);

          this.priceComponent = this.allors.context.create('BasePrice') as PriceComponent;
          this.priceComponent.PricedBy = this.internalOrganisation;

          if (this.good) {
            this.priceComponent.Product = this.good;
          }

          if (this.part) {
            this.priceComponent.Part = this.part;
          }

        } else {
          this.edit = !(this.add = false);

          this.priceComponent = loaded.objects.PriceComponent as PriceComponent;
        }
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

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
