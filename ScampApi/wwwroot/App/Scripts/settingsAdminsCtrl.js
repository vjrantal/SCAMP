'use strict';
angular.module('scamp')
.controller('settingsAdminsCtrl', ['$scope', 'adalAuthenticationService', '$location', 'systemSettingsSvc', function ($scope, adalService, $location, systemSettingsSvc) {
    $scope.currentRouteName = 'settingsAdmins';

    // ?? could this be placed into the resolve of the route ??
    // get the list of current system administrators
    systemSettingsSvc.getSystemAdmins().then(
        // get succeeded
        function (data) {
            $scope.adminList = data;
        },
        // get failed
        function (statusCode) {
            console.log(statusCode);
        }
    );
    
    //$scope.getSystemAdmins = function () {
    //    $scope.adminList = systemSettings.getSystemAdmins();
    //    console.log($scope.adminList);
    //};

}]);