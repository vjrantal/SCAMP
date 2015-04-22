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
    
    // revoke system administrator permissions for the selected user
    $scope.confirmDeleteAdmin = function (user) {

        // make sure we have more than 1 system admin
        if ($scope.adminList.length > 1)
        {
            var wndRsp = window.confirm("Are you sure you want to remove " + user.name + " as a System Administrator?")
            if (wndRsp == true) {
                systemSettingsSvc.revokeAdmin(user.id).then(
                    // get succeeded
                    function (data) {
                        window.alert("System Administrator Permissions revoked")
                    },
                    // get failed
                    function (statusCode) {
                        console.log(statusCode);
                    }
                );
            } else {
                window.alert("Operation Cancelled.");
            }
        } else { // if had < 2 system admins
            window.alert("There has to be at least one System Administrator. Action not allowed.")
        }
    };

}]);