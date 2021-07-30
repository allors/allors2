import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, UserId, NavigationService } from '@allors/angular/services/core';
import { Person, Notification } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { ObjectService } from '@allors/angular/material/services/core';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'notification-link',
  templateUrl: './notification-link.component.html',
  providers: [ContextService]
})
export class NotificationLinkComponent implements OnInit, OnDestroy {

  notifications: Notification[];

  private subscription: Subscription;

  get nrOfNotifications() {
    if (this.notifications) {
      const count = this.notifications.length;
      if (count < 99) {
        return count;
      } else if (count < 1000) {
        return '99+';
      } else {
        return Math.round(count / 1000) + 'k';
      }
    }

    return "?";
  }

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private userId: UserId,
    ) {
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
                NotificationList: {
                  UnconfirmedNotifications: x
                }
              }
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const user = loaded.objects.Person as Person;
        this.notifications = user.NotificationList.UnconfirmedNotifications;
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  toNotifications() {
    this.navigation.list(this.metaService.m.Notification);
  }
}
