import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { Scope } from '../../../../allors/angular/base/Scope';
import { AllorsService } from '../../../allors.service';
import { InternalOrganisation, Organisation, Locale, CustomerRelationship } from '../../../../allors/domain';

@Component({
  templateUrl: './organisation.component.html',
})
export class OrganisationFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  locales: Locale[];
  internalOrganisation: InternalOrganisation;

  form: FormGroup;
  id: string;

  constructor(private allors: AllorsService,
    private route: ActivatedRoute,
    public fb: FormBuilder,
    public snackBar: MdSnackBar,
    public media: TdMediaService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        this.id = this.route.snapshot.paramMap.get('id');

        this.form = this.fb.group({
          Name: ['', Validators.required],
          Locale: [''],
        });

        this.scope.session.reset();

        return this.scope
          .load('Organisation', { id: this.id })
          .do(() => {
            this.internalOrganisation = this.scope.objects.internalOrganisation as InternalOrganisation;
            this.locales = this.scope.collections.locales as Locale[];

            if (this.id) {
              const organisation: Organisation = this.scope.objects.organisation as Organisation;
              this.form.controls.Name.patchValue(organisation.Name);
              this.form.controls.Locale.patchValue(organisation.Locale);
            }
            else {
              this.form.controls.Locale.patchValue(this.locales);
            }

          })
          .catch((e: any) => {
            this.snackBar.open(e.toString(), 'close', { duration: 5000 });
            this.goBack();
            return Observable.empty()
          });
      })
      .subscribe();
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(event: Event): void {

    let organisation: Organisation;

    if (this.id) {
      organisation = this.scope.session.get(this.id) as Organisation;
    }
    else {
      organisation = this.scope.session.create('Organisation') as Organisation;

      let customerRelationship = this.scope.session.create('CustomerRelationship') as CustomerRelationship
      customerRelationship.InternalOrganisation = this.internalOrganisation;
      customerRelationship.Customer = organisation;
      customerRelationship.FromDate = new Date();
    }

    organisation.Name = this.form.controls.Name.value;
    organisation.Locale = this.form.controls.Locale.value;

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
