import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { Scope } from '../../../allors/angular/base/Scope';
import { AllorsService } from '../../allors.service';
import { Organisation, PartyContactMechanism, WebAddress, Enumeration } from '../../../allors/domain';

@Component({
  templateUrl: './webAddress.component.html',
})
export class WebAddressAddComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  contactMechanismPurposes: Enumeration[ ];

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
          ContactMechanismPurposes: [''],
          ElectronicAddress: ['', Validators.required],
          Default: ['', Validators.required],
        });

        this.scope.session.reset();

        return this.scope
          .load('Organisation', { id: this.id })
          .do(() => {
            this.contactMechanismPurposes = this.scope.collections.contactMechanismPurposes as Enumeration[];

            this.form.controls.ContactMechanismPurposes.patchValue(this.contactMechanismPurposes);
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

    const organisation: Organisation = this.scope.session.get(this.id) as Organisation;

    let webAddress = this.scope.session.create('WebAddress') as WebAddress
    webAddress.ElectronicAddressString = this.form.controls.ElectronicAddress.value;

    let partyContactMechanism = this.scope.session.create('PartyContactMechanism') as PartyContactMechanism
    partyContactMechanism.ContactMechanism = webAddress;
    // partyContactMechanism.UseAsDefault = this.form.controls.Default.value;

    organisation.AddPartyContactMechanism(partyContactMechanism);


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
