var App;
(function (App) {
    config.$inject = ["$provide"];
    function config($provide) {
        var jsnlog = JL("Angular");
        $provide.decorator("$log", ["$delegate",
            function ($delegate) {
                return new Logger(jsnlog, $delegate);
            }
        ]);
        $provide.decorator("$exceptionHandler", ["$delegate",
            function ($delegate) { return function (exception, cause) {
                $delegate(exception, cause);
                // Either select Error page or alert
                window.location.href = "/Error";
                //window.alert(exception);
            }; }
        ]);
    }
    angular
        .module("app")
        .config(config);
    var Logger = (function () {
        function Logger(logger, originalLogger) {
            this.logger = logger;
            this.originalLogger = originalLogger;
        }
        Logger.prototype.debug = function (args) {
            this.logger.debug(args);
            if (this.originalLogger) {
                this.originalLogger.debug(args);
            }
        };
        Logger.prototype.info = function (args) {
            this.logger.info(args);
            if (this.originalLogger) {
                this.originalLogger.info(args);
            }
        };
        Logger.prototype.warn = function (args) {
            this.logger.warn(args);
            if (this.originalLogger) {
                this.originalLogger.warn(args);
            }
        };
        Logger.prototype.error = function (args) {
            this.logger.error(args);
            if (this.originalLogger) {
                this.originalLogger.error(args);
            }
        };
        Logger.prototype.log = function (args) {
            this.logger.trace(args);
            if (this.originalLogger) {
                this.originalLogger.log(args);
            }
        };
        return Logger;
    }());
})(App || (App = {}));
//# sourceMappingURL=app.logging.js.map