namespace App
{
    goTo.$inject = ["$cookies", "$state"];
    function goTo($cookies: ng.cookies.ICookiesService, $state: ng.ui.IStateService): void {
        const cookieName = "GoTo";

        const goTo = $cookies.get(cookieName);
        if (goTo) {
            $cookies.remove(cookieName);

            const parts = goTo.split(" ");
            const to = parts[0];
            const params = {
                id: parts[1]
            };

            $state.go(to, params);
        }
    }
    angular
        .module("app")
        .run(goTo);
}
