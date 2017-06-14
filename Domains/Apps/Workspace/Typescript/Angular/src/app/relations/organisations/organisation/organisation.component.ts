import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { Scope } from '../../../../allors/angular/base/Scope';
import { AllorsService } from '../../../allors.service';
import { PullRequest, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page, Locale, OrganisationRole, Organisation } from '../../../../allors/domain';
import { MetaDomain } from '../../../../allors/meta/index';

@Component({
  templateUrl: './organisation.component.html',
})
export class OrganisationFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;
  private id: string;

  locales: Locale[];
  roles: OrganisationRole[];

  form: FormGroup;

  constructor(private allors: AllorsService,
    private route: ActivatedRoute,
    public fb: FormBuilder,
    public snackBar: MdSnackBar,
    public media: TdMediaService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  createForm(): void {
    this.form = this.fb.group(
      {
        Roles: ['', Validators.required],
        Name: ['', Validators.required],
        Locale: [''],
      },
    );
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        this.id = this.route.snapshot.paramMap.get('id');

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'organisation',
            id: this.id,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'locales',
              objectType: m.Locale,
            }),
          new Query(
            {
              name: 'roles',
              objectType: m.OrganisationRole,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }))
      })
      .subscribe(() => {

        this.locales = this.scope.collections.locales as Locale[];
        this.roles = this.scope.collections.roles as OrganisationRole[];

        if (this.id) {
          const organisation: Organisation = this.scope.objects.organisation as Organisation;
          // this.form.controls.Roles.patchValue(organisation.OrganisationRoles);
          this.form.controls.Name.patchValue(organisation.Name);
          this.form.controls.Locale.patchValue(organisation.Locale ? organisation.Locale.id : undefined);
        }
        // else {
        //   this.form.controls.Name.patchValue(undefined);
        //   this.form.controls.Locale.patchValue(undefined);
        // }
      },
      (error: any) => {
        this.snackBar.open(error, 'close', { duration: 5000 });
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

  save(event: Event): void {

    let organisation: Organisation;

    if (this.id) {
      organisation = this.scope.session.get(this.id) as Organisation;
    } else {
      organisation = this.scope.session.create('Organisation') as Organisation;
    }

    organisation.Name = this.form.controls.Name.value;
    organisation.Locale = this.scope.session.get(this.form.controls.Locale.value) as Locale;

    this.scope
      .save()
      .toPromise()
      .then(() => {
        this.goBack();
      })
      .catch((e: any) => {
        this.snackBar.open(e.toString(), 'close', { duration: 5000 });
      });
  }

  goBack(): void {
    window.history.back();
  }
}
