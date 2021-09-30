namespace App {

    templates.$inject = ["$templateCache"];
    function templates($templateCache: ng.ITemplateCacheService): void {
        
        Allors.Bootstrap.registerTemplates($templateCache);
    }

    angular
        .module("app")
        .run(templates);
}