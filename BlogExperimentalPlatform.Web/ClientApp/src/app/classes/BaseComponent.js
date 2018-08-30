"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BaseComponent = /** @class */ (function () {
    function BaseComponent(notificationService) {
        this.notificationService = notificationService;
    }
    // events
    BaseComponent.prototype.ngOnInit = function () {
    };
    BaseComponent.prototype.checkForErrorsOnRequest = function (error, errorMessage) {
        // Later - handle 401 / 403 status code when adding security
        this.notificationService.printErrorMessage(errorMessage + ' ' + error);
        // Maybe redirect to error page if 500
    };
    return BaseComponent;
}());
exports.BaseComponent = BaseComponent;
//# sourceMappingURL=BaseComponent.js.map