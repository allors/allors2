import { Subject } from 'rxjs';

import { RoleType, ISessionObject } from './../../../../../framework';
import { Printable } from '../../../../../domain';
import { Action, ActionTarget} from '../../../../../angular';

import { PrintConfig } from './print.config';

export class PrintAction implements Action {

  constructor(config: PrintConfig, roleType?: RoleType) {
    this.execute = (target: ActionTarget) => {

      let printable = target as Printable;

      if (roleType) {
        printable = printable.get(roleType.name);
      }

      const revision = printable.PrintDocument && printable.PrintDocument.Media ? printable.PrintDocument.Media.Revision : undefined;

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
