import { Subject } from 'rxjs';

import { Printable } from '../../../../../domain';
import { Action, ActionTarget, MediaService } from '../../../../../angular';

import { PrintConfig } from './print.config';

export class PrintAction implements Action {

  constructor(config: PrintConfig) {
    this.execute = (target: ActionTarget) => {
      const printable = target as Printable;
      const url = `${config.url}Print/Download/${printable.id}`;
      window.open(url);
    };
  }

  result = new Subject<boolean>();

  execute: (target: ActionTarget) => void;

  name = () => 'Print';
  description = () => 'Print';
  disabled = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      return true;
    } else {
      return !(target as Printable).CanReadPrintDocument || !(target as Printable).PrintDocument;
    }
  }
}
