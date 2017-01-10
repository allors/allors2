namespace App
{
    config.$inject = ["cfpLoadingBarProvider"];
    function config(loadingBar: any): void {
        loadingBar.includeSpinner = false;
    }
    angular
        .module("app")
        .config(config);
}
