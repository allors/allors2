var App;
(function (App) {
    goTo.$inject = ["$cookies", "$state"];
    function goTo($cookies, $state) {
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
        }
    }
    angular
        .module("app")
        .run(goTo);
})(App || (App = {}));
//# sourceMappingURL=app.goto.js.map