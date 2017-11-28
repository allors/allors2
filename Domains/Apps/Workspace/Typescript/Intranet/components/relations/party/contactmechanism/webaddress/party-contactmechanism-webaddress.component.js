"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.template = `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="contactMechanism" (submit)="save()">

    <div class="pad">
      <a-mat-select [object]="partyContactMechanism" [roleType]="m.PartyContactMechanism.ContactPurposes" [options]="contactMechanismPurposes"
        display="Name" label="Contact purposes"></a-mat-select>
      <a-mat-input [object]="contactMechanism" [roleType]="m.WebAddress.ElectronicAddressString"></a-mat-input>
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
