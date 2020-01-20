import { Component } from '@angular/core';

import { AllorsBarcodeService } from '../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-scanner',
  templateUrl: './scanner.component.html',
})
export class AllorsMaterialScannerComponent {

  showScanner: boolean;
  barcode: string;

  constructor(
    private barcodeService: AllorsBarcodeService,
  ) {
  }

  scan() {
    try {
      this.barcodeService.scan(this.barcode);
    } finally {
      this.barcode = null;
      this.showScanner = null;
    }
  }
}
