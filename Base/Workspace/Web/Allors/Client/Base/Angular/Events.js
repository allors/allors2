var Allors;
(function (Allors) {
    var Events = (function () {
        function Events(context, $rootScope, scope) {
            this.context = context;
            this.$rootScope = $rootScope;
            this.scope = scope;
        }
        Events.prototype.on = function (eventName, handler) {
            this.scope.$on(eventName, handler);
        };
        Events.prototype.onRefresh = function (handler) {
            this.on(Events.refreshEventName, handler);
        };
        Events.prototype.broadcast = function (eventName) {
            this.$rootScope.$broadcast(eventName, this.context.name);
        };
        Events.prototype.broadcastRefresh = function () {
            this.broadcast(Events.refreshEventName);
        };
        return Events;
    }());
    Events.refreshEventName = "allors.refresh";
    Allors.Events = Events;
})(Allors || (Allors = {}));
//# sourceMappingURL=Events.js.map