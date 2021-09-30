module App
{
    var app = angular.module("app");

    app.run(goTo);

    goTo.$inject = ["$cookies", "$state"];
    function goTo($cookies: angular.cookies.ICookiesService, $state: angular.ui.IStateService): void {
        var cookieName = "GoTo";

        var goTo = $cookies.get(cookieName);
        if (goTo) {
            $cookies.remove(cookieName);

            var parts = goTo.split(" ");
            var to = parts[0];
            var params = {
                id: parts[1]
            };

            $state.go(to, params);
        } else {
            $state.go("home");
        }
    }
}
