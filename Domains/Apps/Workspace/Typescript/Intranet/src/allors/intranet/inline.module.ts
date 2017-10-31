import { NgModule } from "@angular/core";

import { PersonInlineModule } from "./apps/relations/people/person/person-inline.module";

const APPS_INLINE_MODULES: any[] = [
  PersonInlineModule,
];

@NgModule({
  exports: [
    PersonInlineModule,
  ],
  imports: [
    PersonInlineModule,
  ],
})
export class InlineModule { }
