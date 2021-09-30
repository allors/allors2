namespace App {
  angular.module("app",
    [
      "allors",

      // Angular
      "ngSanitize", "ngAnimate", "ngCookies", "ngMessages",

      // Angular UI
      "ui.router", "ui.bootstrap", "ui.select",

      // Third Party
      "pascalprecht.translate", "toastr", "angular-loading-bar", "blockUI"
    ]);
}
