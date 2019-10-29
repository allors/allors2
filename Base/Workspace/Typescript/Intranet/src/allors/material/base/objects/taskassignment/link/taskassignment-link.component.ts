import { Component, OnDestroy, OnInit, Self } from '@angular/core';

import { Subscription, } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { PullRequest } from '../../../../../framework';
import { AllorsFilterService, ContextService, NavigationService, RefreshService, MetaService, UserId } from '../../../../../angular';

import { Task, Person } from '../../../../../domain';

import { ObjectService } from '../../../../core/services/object';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'taskassignment-link',
  templateUrl: './taskassignment-link.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class TaskAssignmentLinkComponent implements OnInit, OnDestroy {

  tasks: Task[];

  private subscription: Subscription;

  get nrOfTasks() {
    if (this.tasks) {
      const count = this.tasks.filter(v => !v.DateClosed).length;
      if (count < 99) {
        return count;
      } else if (count < 1000) {
        return '99+';
      } else {
        return Math.round(count / 1000) + 'k';
      }
    }
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

    const { m, pull, x } = this.metaService;

    this.subscription = this.refreshService.refresh$
      .pipe(
        switchMap(() => {

          const pulls = [
            pull.Task({
              include: {
                Participants: x
              }
            }),
            pull.Person({
              object: this.userId.value
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const user = loaded.objects.Person as Person;

        const allTasks = loaded.collections.Tasks as Task[];
        this.tasks = allTasks.filter(v => v.Participants.indexOf(user) > -1);
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  toTasks() {
    this.navigation.list(this.metaService.m.TaskAssignment);
  }
}
