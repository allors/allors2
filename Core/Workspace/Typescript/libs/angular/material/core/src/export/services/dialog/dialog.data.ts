import { DialogConfig, PromptType } from '@allors/angular/material/services/core';

export interface DialogData {
  alert?: boolean;
  confirmation?: boolean;
  prompt?: boolean;
  promptType?: PromptType;

  config: DialogConfig;
}
