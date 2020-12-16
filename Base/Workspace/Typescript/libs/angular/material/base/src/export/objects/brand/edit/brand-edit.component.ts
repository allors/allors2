import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { TestScope } from '@allors/angular/core';
import { Brand, Model, Locale } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { FetcherService } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { MetaService, ContextService, RefreshService } from '@allors/angular/services/core';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';

@Component({
  templateUrl: './brand-edit.component.html',
  providers: [ContextService],
})
export class BrandEditComponent extends TestScope implements OnInit, OnDestroy {
  public title: string;
  public subTitle: string;

  public m: Meta;

  public brand: Brand;
  locales: Locale[];
  addModel = false;

  private subscription: Subscription;
  models: Model[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<BrandEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { pull, x } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$])
      .pipe(
        switchMap(() => {
          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.locales,
          ];

          if (!isCreate) {
            pulls.push(
              pull.Brand({
                object: this.data.id,
                include: {
                  LogoImage: x,
                  Models: x,
                  LocalisedDescriptions: x,
                },
              }),
            );
          }

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => ({ loaded, isCreate })));
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        if (isCreate) {
          this.title = 'Add Brand';
          this.brand = this.allors.context.create('Brand') as Brand;
        } else {
          this.brand = loaded.objects.Brand as Brand;

          if (this.brand.CanWriteName) {
            this.title = 'Edit Brand';
          } else {
            this.title = 'View Brand';
          }
        }

        this.models = this.brand.Models.sort((a, b) => (a.Name > b.Name ? 1 : b.Name > a.Name ? -1 : 0));
      });
  }

  public modelAdded(model: Model): void {
    this.brand.AddModel(model);
    this.models = this.brand.Models.sort((a, b) => (a.Name > b.Name ? 1 : b.Name > a.Name ? -1 : 0));
    this.allors.context.session.hasChanges = true;
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.brand.id,
        objectType: this.brand.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }
}
