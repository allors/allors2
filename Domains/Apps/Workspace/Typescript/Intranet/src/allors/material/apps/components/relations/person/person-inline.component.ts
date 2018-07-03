import { Component, EventEmitter, OnInit , Output } from '@angular/core';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, LayoutService } from '../../../../../angular';
import { Enumeration, Locale, Person } from '../../../../../domain';
import { PullRequest, Query, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  selector: 'person-inline',
  templateUrl: './person-inline.component.html',
})
export class PersonInlineComponent implements OnInit {

  @Output()
  public saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public person: Person;

  public m: MetaDomain;

  public locales: Locale[];
  public genders: Enumeration[];
  public salutations: Enumeration[];

  private scope: Scope;

  constructor(
    public layout: LayoutService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    const queries: Query[] = [
      new Query(
        {
          name: 'locales',
          objectType: this.m.Locale,
          sort: [
            new Sort({ roleType: this.m.Locale.Name, direction: 'Asc' }),
          ],
      }),
      new Query(
        {
          name: 'genders',
          objectType: this.m.GenderType,
          sort: [
            new Sort({ roleType: this.m.GenderType.Name, direction: 'Asc' }),
          ],
      }),
      new Query(
      {
        name: 'salutations',
        objectType: this.m.Salutation,
        sort: [
          new Sort({ roleType: this.m.Salutation.Name, direction: 'Asc' }),
        ],
      }),
    ];

    this.scope
      .load('Pull', new PullRequest({ queries }))
      .subscribe((loaded) => {
        this.locales = loaded.collections.locales as Locale[];
        this.genders = loaded.collections.genders as Enumeration[];
        this.salutations = loaded.collections.salutations as Enumeration[];

        this.person = this.scope.session.create('Person') as Person;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.person.id);
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }
}
