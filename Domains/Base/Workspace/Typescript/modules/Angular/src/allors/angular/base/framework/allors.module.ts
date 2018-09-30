import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Allors } from './AllorsService';
import { DatabaseService } from './DatabaseService';
import { WorkspaceService } from './WorkspaceService';

@NgModule({
  imports: [
    CommonModule,
  ],
})
export class AllorsModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsModule,
      providers: [ Allors, DatabaseService, WorkspaceService ]
    };
  }
}
