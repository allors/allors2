import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved, SingletonId } from '@allors/angular/services/core';
import { Locale, Organisation, Singleton, UserProfile } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { IObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './userprofile-edit.component.html',
  providers: [ContextService]
})
export class UserProfileEditComponent extends TestScope implements OnInit, OnDestroy {

  public title: string;
  public subTitle: string;

  public m: Meta;

  public userProfile: UserProfile;

  private subscription: Subscription;
  internalOrganizations: Organisation[];
  supportedLocales: Locale[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<UserProfileEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private singletonId: SingletonId,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const pulls = [
            pull.Singleton({
              object: this.singletonId.value,
              include: {
                Locales: x,
              }
            }),
            pull.UserProfile({
              object: this.data.id,
              include: {
                DefaultInternalOrganization: x,
                DefaulLocale: x,
              }
            }),
            pull.Organisation(
              {
                predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
                sort: new Sort(m.Organisation.PartyName)
              }
            )
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded }))
            );
        })
      )
      .subscribe(({ loaded }) => {

        this.allors.context.reset();

        this.userProfile = loaded.objects.UserProfile as UserProfile;
        this.internalOrganizations = loaded.collections.Organisations as Organisation[];

        const singleton = loaded.objects.Singleton as Singleton;
        this.supportedLocales = singleton.Locales;

        this.title = 'Edit User Profile';
      });
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
        const data: IObject = {
          id: this.userProfile.id,
          objectType: this.userProfile.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
