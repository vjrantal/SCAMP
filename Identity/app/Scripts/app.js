'use strict';
angular.module('corsApp', ['ngRoute','AdalAngular'])
.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', function ($routeProvider, $httpProvider, adalProvider) {

    $routeProvider.when("/Home", {
        controller: "homeCtrl",
        templateUrl: "/App/Views/Home.html",
    }).when("/CorsCall", {
        controller: "corsCallCtrl",
        templateUrl: "/App/Views/CorsCall.html",
        requireADLogin: true,
    }).when("/UserData", {
        controller: "userDataCtrl",
        templateUrl: "/App/Views/UserData.html",
    }).otherwise({ redirectTo: "/Home" });

    //TODO: The endpoints setting ensures ADAL.js will automatically acquire the access tokens transparently during requests.
    var endpoints = {
        "https://localhost:44301/api": "82462670-0893-4317-9f72-d8526cf6f662",
        "https://zbrad.com/api": "82462670-0893-4317-9f72-d8526cf6f662"
    };

    //TODO: Replace these by your tenant and client ID after having created the Azure AD applications
    adalProvider.init(
        {
            tenant: 'zbrad.com',
            clientId: '82462670-0893-4317-9f72-d8526cf6f662',
            endpoints:endpoints
        },
        $httpProvider
        );
  
   
}]);
