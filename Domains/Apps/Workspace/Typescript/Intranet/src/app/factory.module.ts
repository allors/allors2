import { NgModule, ModuleWithProviders, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { ids } from '../allors/meta/generated';

import { FactoryService, FactoryConfig } from '../allors/angular/base/factory';

import { PersonAddModule, PersonAddComponent } from '../allors/material/apps/objects/person/add/person-add.module';

const factoryConfig: FactoryConfig = new FactoryConfig({
  items:
    [
      { id: ids.Person, component: PersonAddComponent }
    ]
});

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,
    PersonAddModule
  ],
  entryComponents: [
    PersonAddComponent
  ],
  providers: [
    FactoryService,
    { provide: FactoryConfig, useValue: factoryConfig },
  ]
})
export class FactoryModule {

  constructor(@Optional() @SkipSelf() core: FactoryModule) {
    if (core) {
      throw new Error('Use FactoryModule from AppModule');
    }
  }

}
