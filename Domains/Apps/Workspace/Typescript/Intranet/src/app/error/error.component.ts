import { Component } from '@angular/core';
import { Response, ResponseError, DerivationError } from '../../allors/framework';

@Component({
  templateUrl: './error.component.html'
})
export class ErrorComponent {

  title: string;
  message: string;

  // constructor() {
  //   const error = null;

  //   if (error instanceof ResponseError) {
  //     const responseError: ResponseError = error;
  //     const response: Response = error.response;

  //     if (response.accessErrors && response.accessErrors.length > 0) {
  //       this.title = 'Access Error';
  //       this.message = 'You do not have the required rights.';
  //     } else if (response.versionErrors && response.versionErrors.length > 0) {
  //       this.title = 'Concurrency Error';
  //       this.message = 'Modifications were detected since last access.';
  //     } else if (response.missingErrors && response.missingErrors.length > 0) {
  //       this.title = 'Missing Error';
  //       this.message = 'Access to a deleted object was requested.';
  //     } else if (response.derivationErrors && response.derivationErrors.length > 0) {
  //       this.title = 'Derivation Errors';
  //       response.derivationErrors.map((derivationError: DerivationError) => {
  //         this.message += `\n* ${derivationError.m}`;
  //       });
  //     } else {
  //       this.title = 'Error';
  //       this.message = responseError.message;
  //     }
  //   } else {
  //     this.title = 'Error';
  //     if (error.message) {
  //       this.message = error.message;
  //     } else {
  //       this.message = JSON.stringify(error);
  //     }
  //   }
  // }

}
