import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService } from '../../../../../angular';
import { InternalOrganisation, Organisation, Good, IGoodIdentification } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './good-overview.component.html',
  providers: [ContextService]
})
export class GoodOverviewComponent implements OnInit, OnDestroy {

  m: Meta;

  title = 'Product Overview';
  good: Good;
  organisation: Organisation;
  internalOrganisation: InternalOrganisation;
  categories: string;
  productNumber: string;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  private fetcher: Fetcher;

  constructor(
    @Self() private allors: ContextService,
    public navigationService: NavigationService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Good({
              object: id,
              include: {
                GoodIdentifications: {
                  GoodIdentificationType: x
                },
                ProductCategories: x,
                Part: {
                  Brand: x,
                  Model: x
                }
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.good = loaded.objects.Good as Good;

        if (this.good.ProductCategories.length > 0) {
          this.categories = this.good.ProductCategories
            .map(v => v.Name)
            .reduce((acc: string, cur: string) => acc + ', ' + cur);
        }

      }, this.errorService.handler);
  }

  public deleteGoodIdentification(goodIdentification: IGoodIdentification): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.context.invoke(goodIdentification.Delete)
            .subscribe(() => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }
}
