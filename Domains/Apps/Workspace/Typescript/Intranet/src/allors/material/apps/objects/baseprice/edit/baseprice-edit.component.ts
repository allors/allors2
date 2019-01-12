import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationActivatedRoute, MetaService, RefreshService } from '../../../../../angular';
import { Good, Part, PriceComponent, InternalOrganisation, Organisation } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { Fetcher } from '../../Fetcher';
import { EditData, CreateData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './baseprice-edit.component.html',
  providers: [ContextService]
})
export class BasepriceEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  good: Good;
  part: Part;
  priceComponent: PriceComponent;
  internalOrganisation: InternalOrganisation;
  item: Good | Part;
  title: string;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<BasepriceEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          let pulls = [
            this.fetcher.internalOrganisation,
          ];

          if (isCreate) {
            pulls = [
              ...pulls,
              pull.Good({
                object: this.data.associationId,
              }),
              pull.Part({
                object: this.data.associationId,
              }),
            ];
          }

          if (!isCreate) {
            pulls = [
              ...pulls,
              pull.PriceComponent({
                object: this.data.id,
                include: {
                  Currency: x,
                }
              }),
            ];
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.good = loaded.objects.Good as Good;
        this.part = loaded.objects.Part as Part;

        if (isCreate) {
          this.title = 'Add base price';

          this.priceComponent = this.allors.context.create('BasePrice') as PriceComponent;
          this.priceComponent.FromDate = new Date();
          this.priceComponent.PricedBy = this.internalOrganisation;

          if (this.good) {
            this.priceComponent.Product = this.good;
          }

          if (this.part) {
            this.priceComponent.Part = this.part;
          }

        } else {

          this.priceComponent = loaded.objects.PriceComponent as PriceComponent;

          if (this.priceComponent.CanWritePrice) {
            this.title = 'Edit base price';
          } else {
            this.title = 'View base price';
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.priceComponent.id,
          objectType: this.priceComponent.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
