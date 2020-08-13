import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, UserId } from '@allors/angular/services/core';
import { Person } from '@allors/domain/generated';
import {  EditService } from '@allors/angular/material/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectService } from '@allors/angular/material/services/core';
import { Action } from '@allors/angular/core';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'userprofile-link',
  templateUrl: './userprofile-link.component.html',
  providers: [ContextService]
})
export class UserProfileLinkComponent implements OnInit, OnDestroy {

  edit: Action;

  private subscription: Subscription;
  user: Person;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public editService: EditService,
    private userId: UserId,
    ) {
      this.edit = editService.edit();
  }

  ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = this.refreshService.refresh$
      .pipe(
        switchMap(() => {

          const pulls = [
            pull.Person({
              object: this.userId.value,
              include: {
                UserProfile: {
                  DefaultInternalOrganization: x
                }
              }
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.user = loaded.objects.Person as Person;
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  toUserProfile() {
    this.edit.execute(this.user.UserProfile);
  }
}
