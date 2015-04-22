'use strict';
angular.module('scamp')
.controller('settingsAdminsCtrl', ['$scope', 'adalAuthenticationService', '$location', 'settingsAdminsSvc', function ($scope, adalService, $location, settingsAdminsSvc) {
    $scope.currentRouteName = 'settingsAdmins';

    // gets a list of currently configuration subscription
    $scope.getSystemAdmins = function () {
        $scope.adminList = settingsAdminsSvc.getSystemAdmins();
        console.log($scope.adminList);
    };

}]);