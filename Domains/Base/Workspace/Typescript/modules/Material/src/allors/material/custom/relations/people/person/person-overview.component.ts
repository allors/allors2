import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, WorkspaceService, SessionService } from '../../../../../angular';
import { Locale, Person } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { MetaDomain, PullFactory } from '../../../../../meta';

@Component({
  templateUrl: './person-overview.component.html',
  providers: [SessionService]
})
export class PersonOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public person: Person;
  public locales: Locale[];

  public title: string;

  private subscription: Subscription;

  constructor(
    @Self() private sessionService: SessionService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Person Overview';
    this.titleService.setTitle(this.title);
    this.m = this.sessionService.m;
  }

  public ngOnInit(): void {

    const { x, pull } = this.sessionService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Person({
              object: id,
              include: {
                Photo: x,
              }
            })
          ];

          this.sessionService.session.reset();

          return this.sessionService
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {
        this.person = loaded.objects.Person as Person;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngAfterViewInit(): void {
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }
}
