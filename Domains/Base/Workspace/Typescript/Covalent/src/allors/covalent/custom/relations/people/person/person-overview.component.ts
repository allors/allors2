import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Person, Locale, Organisation } from '../../../../../domain';
import { Scope, Loaded, AllorsService, ErrorService } from '../../../../../angular';

@Component({
  templateUrl: './person-overview.component.html',
})
export class PersonOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;
  m: MetaDomain;

  person: Person;
  locales: Locale[];

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;
  }

  ngOnInit(): void {

    this.subscription = this.route.url
      .mergeMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'person',
            id: id,
            include: [
              new TreeNode({roleType: m.Person.Locale}),
            ],
          }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch }));
      })
      .subscribe((loaded: Loaded) => {
        this.person = loaded.objects.person as Person;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
      );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  goBack(): void {
    window.history.back();
  }

  checkType(obj: any): string {
    return obj.objectType.name;
  }
}
