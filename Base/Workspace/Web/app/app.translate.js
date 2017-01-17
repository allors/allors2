var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["$translateProvider", "tmhDynamicLocaleProvider"];
    function config($translateProvider, tmhDynamicLocaleProvider) {
        tmhDynamicLocaleProvider.localeLocationPattern("/lib/angular-i18n/angular-locale_{{locale}}.js");
        $translateProvider.useSanitizeValueStrategy("sanitize");
        $translateProvider.useUrlLoader("/database/translate");
        $translateProvider.preferredLanguage("en");
        $translateProvider.fallbackLanguage("en");
        $translateProvider.registerAvailableLanguageKeys(["en", "fr", "nl", "de"], {
            'en_*': "en",
            'fr_*': "fr",
            'nl_*': "nl",
            'de_*': "de"
        });
        $translateProvider.determinePreferredLanguage();
    }
})(App || (App = {}));
//# sourceMappingURL=app.translate.js.map