'use strict';
angular.module('scamp')
.controller('settingsCtrl', ['$scope', '$location', 'systemSettingsSvc', 'adalAuthenticationService', function ($scope, $location, systemSettingsSvc, adalService) {
    $scope.currentRouteName = 'Settings';

 
    // set default setting "tab" view
    if (!$scope.settingsView)
        $scope.settingsView = "sysAdmins";

    // get the list of current system administrators
    $scope.getSystemAdmins = function () {
        systemSettingsSvc.getSysAdmins().then(
            // get succeeded
            function (data) {
                $scope.adminList = data;
            },
            // get failed
            function (statusCode) {
                console.log(statusCode);
            }
        );
    }
    
    // get the list of current system administrators
    $scope.getGroupManagers = function () {
        systemSettingsSvc.getGroupManagers().then(
            // get succeeded
            function (data) {
                $scope.mgrList = data;
            },
            // get failed
            function (statusCode) {
                console.log(statusCode);
            }
        );
    }

    // revoke system administrator permissions for the selected user
    $scope.confirmDeleteAdmin = function (user) {

        // make sure we have more than 1 system admin
        if ($scope.adminList.length <= 1) {
            window.alert("There has to be at least one System Administrator. Action not allowed.");
        } else { // if had < 2 system admins
            var wndRsp = window.confirm("Are you sure you want to remove " + user.name + " as a System Administrator?")
            if (wndRsp == true) {
                systemSettingsSvc.revokeSysAdmin(user.id).then(
                    // get succeeded
                    function (data) {
                        window.alert("System Administrator Permissions revoked")
                        // reload list of system admins
                        $scope.getSystemAdmins();
                    },
                    // get failed
                    function (status) {
                        window.alert("Revoke failed");
                    }
                );
            } else {
                window.alert("Operation Cancelled.");
            }
        }
    };
}]);