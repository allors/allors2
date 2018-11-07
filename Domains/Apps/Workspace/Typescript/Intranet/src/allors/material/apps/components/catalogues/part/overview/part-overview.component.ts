import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Saved, x, Allors, NavigationService, NavigationActivatedRoute } from '../../../../../../angular';
import { InternalOrganisation, Part, IGoodIdentification } from '../../../../../../domain';
import { PullRequest } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './part-overview.component.html',
  providers: [Allors]
})
export class PartOverviewComponent implements OnInit, OnDestroy {

  m: MetaDomain;

  title = 'Part Overview';
  part: Part;
  internalOrganisation: InternalOrganisation;
  suppliers: string;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  private fetcher: Fetcher;

  constructor(
    @Self() private allors: Allors,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { pull, tree, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Part({
              object: id,
              include: {
                GoodIdentifications: {
                  GoodIdentificationType: x
                },
                ProductType: x,
                InventoryItemKind: x,
                Brand: x,
                Model: x
              }
            }),
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.part = loaded.objects.Part as Part;

        if (this.part.SuppliedBy.length > 0) {
          this.suppliers = this.part.SuppliedBy
            .map(v => v.displayName)
            .reduce((acc: string, cur: string) => acc + ', ' + cur);
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.navigationService.back();
        },
      );
  }

  public deleteGoodIdentification(goodIdentification: IGoodIdentification): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(goodIdentification.Delete)
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
