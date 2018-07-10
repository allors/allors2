import { enableProdMode, Testability } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .then(ref => {

    if (window && !environment.production) {
      // Testability.findProviders() is not implemented yet, use window hack
      (window as any).testService = ref.injector.get("TestService");
    }

  })
  .catch(err => console.log(err));
