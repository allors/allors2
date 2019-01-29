import { DialogConfig } from './dialog.config';

export interface DialogData {
  alert?: boolean;
  confirmation?: boolean;
  prompt?: boolean;

  config: DialogConfig;
}
