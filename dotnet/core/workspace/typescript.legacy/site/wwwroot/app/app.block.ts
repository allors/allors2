/// <reference path="app.ts" />
module App
{
    var app = angular.module("app");

    app.config(config);

    config.$inject = ["blockUIConfig"];
    function config(blockUIConfig): void {

        blockUIConfig.message = "Processing, please wait.";

        blockUIConfig.blockBrowserNavigation = true;

        blockUIConfig.delay = 2000;
    }
}
