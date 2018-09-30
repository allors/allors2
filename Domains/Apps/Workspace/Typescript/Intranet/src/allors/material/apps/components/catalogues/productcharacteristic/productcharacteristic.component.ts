import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { IUnitOfMeasure, Locale, SerialisedInventoryItemCharacteristicType, Singleton, TimeFrequency, UnitOfMeasure } from '../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { Title } from '../../../../../../../node_modules/@angular/platform-browser';

@Component({
  templateUrl: './productcharacteristic.component.html',
  providers: [Allors]
})
export class ProductCharacteristicComponent implements OnInit, OnDestroy {

  public title = 'Product Characteristic';
  public subTitle: string;

  public m: MetaDomain;

  public productCharacteristic: SerialisedInventoryItemCharacteristicType;

  public singleton: Singleton;
  public locales: Locale[];
  public uoms: UnitOfMeasure[];
  public timeFrequencies: TimeFrequency[];
  public allUoms: IUnitOfMeasure[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.SerialisedInventoryItemCharacteristic(
              {
                object: id,
                include: {
                  SerialisedInventoryItemCharacteristicType: {
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

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.productCharacteristic = loaded.objects.productCharacteristic as SerialisedInventoryItemCharacteristicType;
        if (!this.productCharacteristic) {
          this.productCharacteristic = scope.session.create('SerialisedInventoryItemCharacteristicType') as SerialisedInventoryItemCharacteristicType;
        }

        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.locales = this.singleton.AdditionalLocales;
        this.uoms = loaded.collections.uoms as UnitOfMeasure[];
        this.timeFrequencies = loaded.collections.timeFrequencies as TimeFrequency[];
        this.allUoms = this.uoms.concat(this.timeFrequencies).sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
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
