import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationActivatedRoute, NavigationService, MetaService } from '../../../../../angular';
import { GoodIdentificationType, Good, Part, SkuIdentification } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './skuidentification-edit.component.html',
  providers: [ContextService]
})
export class EditSkuIdentificationComponent implements OnInit, OnDestroy {

  title = 'Good Identification';

  add: boolean;
  edit: boolean;

  m: Meta;

  good: Good;
  part: Part;
  iGoodIdentification: SkuIdentification;
  goodIdentificationTypes: GoodIdentificationType[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  item: Good | Part;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService,
  ) {
    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
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
            pull.GoodIdentificationType({
              sort: new Sort(m.GoodIdentificationType.Name)
            }),
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
              pull.IGoodIdentification({
                object: id,
                include: {
                  GoodIdentificationType: x,
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

        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        const identificationType = this.goodIdentificationTypes.find((v) => v.UniqueId === '2bfb8f67-b2e1-4730-ac3d-3f2af39b7eaf');

        this.good = loaded.objects.Good as Good;
        this.part = loaded.objects.Part as Part;

        if (add) {
          this.add = !(this.edit = false);

          this.iGoodIdentification = this.allors.context.create('SkuIdentification') as SkuIdentification;
          this.iGoodIdentification.GoodIdentificationType = identificationType;

          if (this.good) {
            this.good.AddGoodIdentification(this.iGoodIdentification);
          }

          if (this.part) {
            this.part.AddGoodIdentification(this.iGoodIdentification);
          }

        } else {
          this.edit = !(this.add = false);

          this.iGoodIdentification = loaded.objects.IGoodIdentification as SkuIdentification;
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
