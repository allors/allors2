import { Subject } from 'rxjs';

import { Printable } from '../../../../../domain';
import { Action, ActionTarget, ErrorService } from '../../../../../angular';

import { PrintConfig } from './print.config';

export class PrintAction implements Action {

  constructor(config: PrintConfig, errorService: ErrorService) {
    this.execute = (target: ActionTarget) => {
      const printable = target as Printable;

      let revision: string;
      try {
        revision = printable.PrintDocument && printable.PrintDocument.Media ? printable.PrintDocument.Media.Revision : undefined;
      } catch (exception) {
        errorService.handle(exception);
      }

      const url = revision ?
        `${config.url}Print/DownloadWithRevision/${printable.id}?revision=${revision}` :
        `${config.url}Print/Download/${printable.id}`;
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
