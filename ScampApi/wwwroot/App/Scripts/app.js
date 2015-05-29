'use strict';
angular.module('scamp', ['ngRoute', 'AdalAngular', 'ui.bootstrap'])
.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', '$locationProvider', function ($routeProvider, $httpProvider, adalProvider, $locationProvider) {

    $routeProvider.when("/Home", {
        // Not setting the home controller here, because it is included already
        // in the HTML content.
        templateUrl: "/App/Views/Home.html",
    }).when("/settings/groupMgr", {
        controller: "GroupManagerController",
        templateUrl: "/App/Views/GroupManager.html",
        requireADLogin: true
    }).when("/dashboard", {
        controller: "dashboardCtrl",
        templateUrl: "/App/Views/Dashboard.html",
        requireADLogin: true
    }).when("/resources", {
        controller: "resourcesCtrl",
        templateUrl: "/App/Views/Resources.html",
        requireADLogin: true
    }).when("/settings", {
        controller: "settingsAdminsCtrl",
        templateUrl: "/App/Views/Settings.html",
        requireADLogin: true
    }).when("/settings/admins", {
        controller: "settingsAdminsCtrl",
        templateUrl: "/App/Views/SettingsAdmins.html",
        requireADLogin: true
    }).otherwise({ redirectTo: "/Home" });

    // to help disable the '#' in URLs
    // additional server side configuration may be required
    //$locationProvider.html5Model(true);

    if (scampConfig.settings.clientId === null)
        console.error("Missing $env:APPSETTING_ClientId");
    if (scampConfig.settings.tenantId === null)
        console.error("Missing $env:APPSETTING_TenantId");

    adalProvider.init(
        {
            tenant: scampConfig.settings.tenantId,
            clientId: scampConfig.settings.clientId,
            extraQueryParameter: scampConfig.settings.extraQueryParameter,
            cacheLocation: scampConfig.settings.cacheLocation,
            redirectUri: scampConfig.settings.redirectUri
        },
        $httpProvider
        );
   
}]);
