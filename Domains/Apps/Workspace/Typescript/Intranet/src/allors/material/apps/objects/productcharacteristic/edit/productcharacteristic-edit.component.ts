import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, MetaService } from '../../../../../angular';
import { IUnitOfMeasure, Locale, SerialisedItemCharacteristicType, Singleton, TimeFrequency, UnitOfMeasure } from '../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { Title } from '@angular/platform-browser';

@Component({
  templateUrl: './productcharacteristic-edit.component.html',
  providers: [ContextService]
})
export class ProductCharacteristicComponent implements OnInit, OnDestroy {

  public title = 'Product Characteristic';
  public subTitle: string;

  public m: Meta;

  public productCharacteristic: SerialisedItemCharacteristicType;

  public singleton: Singleton;
  public locales: Locale[];
  public uoms: UnitOfMeasure[];
  public timeFrequencies: TimeFrequency[];
  public allUoms: IUnitOfMeasure[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.SerialisedItemCharacteristic(
              {
                object: id,
                include: {
                  SerialisedItemCharacteristicType: {
                    LocalisedNames: {
                      Locale: x,
                    }
                  }
                }
              }
            ),
            pull.Singleton({
              include: {
                AdditionalLocales: {
                  Language: x,
                }
              }
            }),
            pull.UnitOfMeasure({
              predicate: new Equals({ propertyType: m.UnitOfMeasure.IsActive, value: true }),
              sort: new Sort(m.UnitOfMeasure.Name),
            }),
            pull.TimeFrequency({
              predicate: new Equals({ propertyType: m.TimeFrequency.IsActive, value: true }),
              sort: new Sort(m.TimeFrequency.Name),
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.productCharacteristic = loaded.objects.productCharacteristic as SerialisedItemCharacteristicType;
        if (!this.productCharacteristic) {
          this.productCharacteristic = this.allors.context.create('SerialisedItemCharacteristicType') as SerialisedItemCharacteristicType;
        }

        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.locales = this.singleton.AdditionalLocales;
        this.uoms = loaded.collections.uoms as UnitOfMeasure[];
        this.timeFrequencies = loaded.collections.timeFrequencies as TimeFrequency[];
        this.allUoms = this.uoms.concat(this.timeFrequencies).sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
