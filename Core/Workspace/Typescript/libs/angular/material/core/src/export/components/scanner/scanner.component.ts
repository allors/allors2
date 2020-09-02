import { Component } from '@angular/core';

import { AllorsBarcodeService } from '@allors/angular/services/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-scanner',
  templateUrl: './scanner.component.html',
})
export class AllorsMaterialScannerComponent {

  showScanner = false;
  barcode: string | null;

  constructor(
    private barcodeService: AllorsBarcodeService,
  ) {
  }

  scan() {
    try {
      if (this.barcode) {
        this.barcodeService.scan(this.barcode);
      }
    } finally {
      this.barcode = null;
      this.showScanner = false;
    }
  }
}
