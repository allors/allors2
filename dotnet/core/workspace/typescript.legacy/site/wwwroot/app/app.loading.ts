module App
{
    var app = angular.module("app");
    
    app.config(config);

    config.$inject = ["cfpLoadingBarProvider"];
    function config(loadingBar: any): void {
        loadingBar.includeSpinner = true;
    }
}
