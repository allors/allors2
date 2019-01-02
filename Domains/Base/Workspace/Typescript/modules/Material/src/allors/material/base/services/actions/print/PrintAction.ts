import { Subject } from 'rxjs';

import { Printable } from '../../../../../domain';
import { Action, ActionTarget, MediaService } from '../../../../../angular';

export class PrintAction implements Action {

  constructor(mediaService: MediaService) {
    this.execute = (target: ActionTarget) => {
      const printable = target as Printable;
      const url = mediaService.url(printable.PrintDocument);
      window.open(url);
    };
  }

  result = new Subject<boolean>();

  execute: (target: ActionTarget) => void;

  name = () => 'Print';
  description = () => 'Print';
  disabled = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      return target.length > 0 ? !(target[0] as Printable).CanReadPrintDocument || !(target[0] as Printable).PrintDocument : true;
    } else {
      return !(target as Printable).CanReadPrintDocument || !(target as Printable).PrintDocument;
    }
  }
}
