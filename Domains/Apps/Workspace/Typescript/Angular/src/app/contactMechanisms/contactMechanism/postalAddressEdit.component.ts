import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { Scope } from '../../../allors/angular/base/Scope';
import { AllorsService } from '../../allors.service';
import { Organisation, PartyContactMechanism, PostalAddress, PostalBoundary, Country, Enumeration } from '../../../allors/domain';

@Component({
  templateUrl: './postalAddress.component.html',
})
export class PostalAddressEditComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  contactMechanismPurposes: Enumeration[ ];
  countries: Country[ ];

  form: FormGroup;
  id: string;

  constructor(private allors: AllorsService,
    private route: ActivatedRoute,
    public fb: FormBuilder,
    public snackBar: MdSnackBar,
    public media: TdMediaService) {
    this.scope = new Scope('PartyContactMechanism', allors.database, allors.workspace);
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        this.id = this.route.snapshot.paramMap.get('contactMechanismId');

        this.form = this.fb.group({
          ContactMechanismPurposes: [''],
          Address1: ['', Validators.required],
          Address2: [''],
          Address3: [''],
          City: ['', Validators.required],
          PostalCode: ['', Validators.required],
          Country: ['', Validators.required],
          Default: ['', Validators.required]
        });

        this.scope.session.reset();

        return this.scope
          .load({ id: this.id })
          .do(() => {
            this.contactMechanismPurposes = this.scope.collections.contactMechanismPurposes as Enumeration[];
            this.countries = this.scope.collections.countries as Country[];

            const partyContactMechanism: PartyContactMechanism = this.scope.objects.partyContactMechanism as PartyContactMechanism;
            const contactMechanism: PostalAddress = partyContactMechanism.ContactMechanism as PostalAddress;

            // this.form.controls.ContactMechanismPurposes.patchValue(partyContactMechanism.ContactPurposes);
            this.form.controls.Address1.patchValue(contactMechanism.Address1);
            this.form.controls.Address2.patchValue(contactMechanism.Address1);
            this.form.controls.Address3.patchValue(contactMechanism.Address1);
            this.form.controls.City.patchValue(contactMechanism.PostalBoundary.Locality);
            this.form.controls.PostalCode.patchValue(contactMechanism.PostalBoundary.PostalCode);
            this.form.controls.Country.patchValue(contactMechanism.PostalBoundary.Country);
            this.form.controls.Default.patchValue(partyContactMechanism.UseAsDefault);
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

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(this.id) as PartyContactMechanism;
    // partyContactMechanism.ContactPurposes = this.form.controls.ContactPurposes.value;
    partyContactMechanism.UseAsDefault = this.form.controls.Default.value;

    let postalBoundary = this.scope.session.create('PostalBoundary') as PostalBoundary
    postalBoundary.Locality = this.form.controls.City.value;
    postalBoundary.PostalCode = this.form.controls.PostalCode.value;
    postalBoundary.Country = this.form.controls.Country.value;

    let postalAddress = this.scope.session.create('PostalAddress') as PostalAddress
    postalAddress.Address1 = this.form.controls.Address1.value;
    postalAddress.Address2 = this.form.controls.Address2.value;
    postalAddress.Address3 = this.form.controls.Address3.value;
    postalAddress.PostalBoundary = postalBoundary;

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
