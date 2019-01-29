import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Locale, Person } from '../../../../../domain';
import { PullRequest, Pull } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { ErrorService, Loaded, ContextService, MetaService } from '../../../../../angular';

@Component({
  templateUrl: './person.component.html',
  providers: [ContextService]
})
export class PersonComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: Meta;
  public locales: Locale[];
  public person: Person;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private metaService: MetaService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Person';
    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { pull } = this.metaService;

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
          this.allors.context.reset();
          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {

        this.person = loaded.objects.Person as Person || this.allors.context.create('Person') as Person;
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

    this.allors.context
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
