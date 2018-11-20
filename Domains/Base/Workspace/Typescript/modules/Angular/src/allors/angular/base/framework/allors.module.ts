import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SessionService } from './SessionService';
import { DatabaseService } from './DatabaseService';
import { WorkspaceService } from './WorkspaceService';
import { DatabaseConfig } from './DatabaseConfig';

@NgModule({
  imports: [
    CommonModule,
  ],
})
export class AllorsModule {
  static forRoot(config: DatabaseConfig): ModuleWithProviders {
    return {
      ngModule: AllorsModule,
      providers: [
        DatabaseService,
        WorkspaceService,
        SessionService,
        { provide: DatabaseConfig, useValue: config },
      ]
    };
  }
}
