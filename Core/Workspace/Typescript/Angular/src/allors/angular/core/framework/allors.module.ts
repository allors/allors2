import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContextService } from './ContextService';
import { DatabaseService } from './DatabaseService';
import { WorkspaceService } from './WorkspaceService';
import { DatabaseConfig } from './DatabaseConfig';

@NgModule({
  imports: [
    CommonModule,
  ],
})
export class AllorsModule {
  static forRoot(config: DatabaseConfig): ModuleWithProviders<AllorsModule> {
    return {
      ngModule: AllorsModule,
      providers: [
        DatabaseService,
        WorkspaceService,
        ContextService,
        { provide: DatabaseConfig, useValue: config },
      ]
    };
  }
}
