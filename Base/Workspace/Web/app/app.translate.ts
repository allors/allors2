namespace App {
    var app = angular.module("app");

    app.config(config);

    config.$inject = ["$translateProvider", "tmhDynamicLocaleProvider"];
    function config($translateProvider: angular.translate.ITranslateProvider, tmhDynamicLocaleProvider: angular.dynamicLocale.tmhDynamicLocaleProvider): void {

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
}
