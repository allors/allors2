namespace App {
    var app = angular.module("app");

    app.config(config);

    config.$inject = ["$provide"];
    function config($provide: any): void {

        $provide.decorator("taOptions", ["$delegate", taOptions => {
            taOptions.toolbar = [
                ["h1", "h2", "h3"],
                ["bold", "italics", "underline", "ul"]
            ];
            return taOptions;
        }]);
    }
}
