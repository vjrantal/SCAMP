'use strict';
angular.module('scamp', ['ngRoute','AdalAngular'])
.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', function ($routeProvider, $httpProvider, adalProvider) {

    $routeProvider.when("/Home", {
        controller: "homeCtrl",
        templateUrl: "/App/Views/Home.html",
    }).when("/dashboard", {
        controller: "dashboardCtrl",
        templateUrl: "/App/Views/Dashboard.html",
        requireADLogin: true,
    }).when("/resources", {
        controller: "resourcesCtrl",
        templateUrl: "/App/Views/Resources.html",
    }).when("/logs", {
        controller: "logsCtrl",
        templateUrl: "/App/Views/Logs.html",
    }).when("/settings", {
        controller: "settingsCtrl",
        templateUrl: "/App/Views/Settings.html",
    }).otherwise({ redirectTo: "/Home" });

    adalProvider.init(
        {
            tenant: 'zbrad.com',
            clientId: '82462670-0893-4317-9f72-d8526cf6f662',
            extraQueryParameter: 'nux=1',
            cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
        },
        $httpProvider
        );
   
}]);
