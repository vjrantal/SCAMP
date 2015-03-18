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
            tenant: 'dpe1.onmicrosoft.com',
            clientId: '5480d52a-a26b-47f5-a0a7-c4838f543f7e',
            extraQueryParameter: 'nux=1',
            cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
        },
        $httpProvider
        );
   
}]);
