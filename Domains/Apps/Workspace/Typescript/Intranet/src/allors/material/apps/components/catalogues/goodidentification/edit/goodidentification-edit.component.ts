import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, x, Allors, NavigationActivatedRoute, NavigationService } from '../../../../../../angular';
import { FaceToFaceCommunication, Organisation, OrganisationContactRelationship, Person, GoodIdentification, GoodIdentificationType, Good, Part } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './goodidentification-edit.component.html',
  providers: [Allors]
})
export class EditGoodIdentificationComponent implements OnInit, OnDestroy {

  title = 'Good Identification';

  add: boolean;
  edit: boolean;

  m: MetaDomain;

  good: Good;
  part: Part;
  goodIdentification: GoodIdentification;
  goodIdentificationTypes: GoodIdentificationType[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  item: Good | Part;

  constructor(
    @Self() private allors: Allors,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private stateService: StateService,
  ) {
    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();
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
              pull.GoodIdentification({
                object: id,
                include: {
                  GoodIdentificationType: x,
                }
              }),
            ];
          }

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {

        scope.session.reset();

        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];

        this.good = loaded.objects.Good as Good;
        this.part = loaded.objects.Part as Part;

        if (add) {
          this.add = !(this.edit = false);

          this.goodIdentification = scope.session.create('GoodIdentification') as GoodIdentification;
          this.good.AddGoodIdentification(this.goodIdentification);

        } else {
          this.edit = !(this.add = false);

          this.goodIdentification = loaded.objects.GoodIdentification as GoodIdentification;
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
    const { scope } = this.allors;

    scope
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
