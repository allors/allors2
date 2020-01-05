import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { UserProfile, Organisation } from '../../../../../domain';
import { PullRequest, Sort, IObject, Equals } from '../../../../../framework';
import { ObjectData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

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

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<UserProfileEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
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
            pull.UserProfile({
              object: this.data.id,
              include: {
                DefaultInternalOrganization: x,
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
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.userProfile.id,
          objectType: this.userProfile.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
