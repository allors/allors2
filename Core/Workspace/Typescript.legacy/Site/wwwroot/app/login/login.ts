module App.General.Tasks {
    class LoginController {

        error: string;

        user: string;
        password: string;

        static $inject = ["$http", "$state", "$scope", "allorsService", "$cookies"];
        constructor(public $http: ng.IHttpService, public $state: angular.ui.IStateService, public $scope: ng.IScope, public allors: Services.AllorsService, $cookies: angular.cookies.ICookiesService) {

            const authenticationUrl = this.allors.baseUrl + "/TestAuthentication/Token";
            let cookieName = "autoLogin";

            var username = $cookies.get(cookieName);
            if (username) {
                $cookies.remove(cookieName);

                this.$http
                    .post(authenticationUrl, { 'UserName': username, 'Password': "" })
                    .then((callbackArg: angular.IHttpResponse<any>) => {
                        var response = callbackArg.data;
                        this.allors.database.authorization = `Bearer ${response.token}`;

                        // Update main
                        var main = (<any>this.$scope.$parent)["controller"];
                        main.refresh();

                        cookieName = "AutoLoginGoTo";

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
                    })
                    .catch((e) => {
                        window.alert("Error: " + e);
                    });
            }

        }

        login() {

            const authenticationUrl = this.allors.baseUrl + "/TestAuthentication/Token";

            return this.$http
                .post(authenticationUrl, { 'UserName': this.user, 'Password': this.password })
                .then((callbackArg: angular.IHttpResponse<any>) => {
                    var response = callbackArg.data;
                    if (!response.authenticated) {
                        this.error = "Not authorized";
                        return;
                    }

                    this.allors.database.authorization = `Bearer ${response.token}`;

                    // Update main
                    var main = (<any>this.$scope.$parent)["controller"];
                    main.refresh();

                    // Go to home
                    this.$state.go("home");
                })
                .catch((error) => {
                    this.error = error.message;
                });
        }
    }
    angular
        .module("app")
        .controller("loginController",
            LoginController);

}
