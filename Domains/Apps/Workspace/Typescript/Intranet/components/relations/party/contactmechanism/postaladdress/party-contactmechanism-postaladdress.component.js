"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.template = `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="contactMechanism" (submit)="save()">

    <div class="pad">
      <a-mat-select [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.ContactPurposes" [options]="contactMechanismPurposes"
        display="Name" label="Contact purposes"></a-mat-select>
      <a-mat-input [object]="contactMechanism" [roleType]="m.PostalAddress.Address1" label="Address line 1"></a-mat-input>
      <a-mat-input [object]="contactMechanism" [roleType]="m.PostalAddress.Address2" label="Address line 2"></a-mat-input>
      <a-mat-input [object]="contactMechanism" [roleType]="m.PostalAddress.Address3" label="Address line 3"></a-mat-input>
      <a-mat-input [object]="contactMechanism.PostalBoundary" [roleType]="m.PostalBoundary?.Locality" label="City"></a-mat-input>
      <a-mat-input [object]="contactMechanism.PostalBoundary" [roleType]="m.PostalBoundary?.PostalCode" label="Postal code"></a-mat-input>
      <a-mat-autocomplete [object]="contactMechanism.PostalBoundary" [roleType]="m.PostalBoundary?.Country" [options]="countries"
        display="Name"></a-mat-autocomplete>
      <a-mat-slide-toggle [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.UseAsDefault" label="Use as default"></a-mat-slide-toggle>
    </div>

    <mat-divider></mat-divider>

    <mat-card-actions>
      <button mat-button color="primary" type="submit" [disabled]="!form.form.valid">SAVE</button>
      <button mat-button (click)="goBack()" type="button">CANCEL</button>
    </mat-card-actions>

  </form>
</td-layout-card-over>
`;
