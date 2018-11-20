import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Locale, Person } from '../../../../../domain';
import { PullRequest, Pull } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { ErrorService, Loaded, SessionService } from '../../../../../angular';

@Component({
  templateUrl: './person.component.html',
  providers: [SessionService]
})
export class PersonComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;
  public locales: Locale[];
  public person: Person;

  private subscription: Subscription;

  constructor(
    @Self() private sessionService: SessionService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Person';
    this.titleService.setTitle(this.title);

    this.m = this.sessionService.m;
  }

  public ngOnInit(): void {
    const { pull } = this.sessionService;

    this.subscription = this.route.url
      .pipe(
        switchMap(() => {
          const x = {};
          const id: string = this.route.snapshot.paramMap.get('id');
          const pulls: Pull[] = [
            pull.Person({
              object: id,
              include: {
                Photo: x,
                Pictures: x,
              }
            }),
            pull.Locale()
          ];
          this.sessionService.session.reset();
          return this.sessionService
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {

        this.person = loaded.objects.Person as Person || this.sessionService.session.create('Person') as Person;
        this.locales = loaded.collections.Locales as Locale[];
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

  public save(): void {

    this.sessionService
      .save()
      .subscribe(() => {
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
