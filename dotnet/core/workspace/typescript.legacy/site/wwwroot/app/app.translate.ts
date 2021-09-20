namespace App {
  var app = angular.module("app");

  app.config(config);

  config.$inject = ["$translateProvider"];
  function config($translateProvider: angular.translate.ITranslateProvider): void {

    $translateProvider.useSanitizeValueStrategy("sanitize");

    $translateProvider.useUrlLoader("/translation/translate");

    $translateProvider.preferredLanguage("en");
    $translateProvider.fallbackLanguage("en");

    $translateProvider.registerAvailableLanguageKeys(["en"], {
      'en_*': "en",
    });

    $translateProvider.determinePreferredLanguage();

    $translateProvider.use('en');
  }
}
