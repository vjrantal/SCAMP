'use strict';
angular.module('scamp')
.controller('settingsCtrl', ['$scope', '$location', 'systemSettingsSvc', 'adalAuthenticationService', function ($scope, $location, systemSettingsSvc, adalService) {
    $scope.currentRouteName = 'Settings';

    var grantAdmin = function (userId) {
        window.alert("you selected" + userId);
    };

    var grantAdmin = function (userId) {
        window.alert("you selected" + userId);
    };

    var valueProperty = 'id'; //The property name for the unique id of the selected option from the RPC

    var sysAdminLookupConfig = {
        componentId: 'addSysAdmin',
        minLength: 3, //The minimum character length needed before suggestions start getting rendered. Defaults to 1
        scopeValueBindedPropertyOnSelection: 'id',
        remote: {
            url: '/api/user/FindbyUPN/%QUERY',
            queryStr: '%QUERY',
            displayProperty: 'name' //This is the property referenced from the response to determine what display on the control
        }
    };

    var sysGroupLookupConfig = {
        componentId: 'addGrpManager',
        minLength: 3, //The minimum character length needed before suggestions start getting rendered. Defaults to 1
        scopeValueBindedPropertyOnSelection: 'id',
        remote: {
            url: '/api/user/FindbyUPN/%QUERY',
            queryStr: '%QUERY',
            displayProperty: 'name' //This is the property referenced from the response to determine what display on the control
        }
    };

    var selItemCB = function (e, datum) {
        selectedUser(datum[valueProperty]);
    }; //The CB referenced for each instance an item is selected from the typeahead.
    var sysAdminTypeaheadControl = new Typeahead($scope, sysAdminLookupConfig, selItemCB);
    var sysGroupTypeaheadControl = new Typeahead($scope, sysGroupLookupConfig, selItemCB);

    // set default setting "tab" view
    if (!$scope.settingsView)
        $scope.settingsView = "sysAdmins";

    // get the list of current system administrators
    $scope.getSystemAdmins = function () {
        console.log("calling: settingsCtrl.getSystemAdmins");
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
    };
    
    // get the list of current system administrators
    $scope.getGroupManagers = function () {
        console.log("calling: settingsCtrl.getGroupManagers");
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
    };

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
    }

    // get the list of configured subscriptions
    $scope.getSubscriptions = function () {
        console.log("calling: settingsCtrl.getSubscriptions");
        systemSettingsSvc.getSubscriptions().then(
            // get succeeded
            function (data) {
                $scope.subList = data;
            },
            // get failed
            function (statusCode) {
                console.log(statusCode);
            }
        );
    };

    // launch the subscription modal pop-up and set up for add/edit
    $scope.confirmSubscriptionUpdate = function (subscription, $event) {
        console.log("calling settingsCtrl.confirmSubscriptionUpdate");

        $scope.subscriptionActionLabel = (subscription == null ? "Add" : "Update");
        $scope.selectedSubscription = subscription;

        $event.preventDefault();
    };

    // close execute subscription modal pop-up action and close window
    $scope.subscriptionSave = function (subscription, $event) {
        console.log("calling settingsCtrl.subscriptionSave");

        //TODO: validate parameters
        // https://github.com/SimpleCloudManagerProject/SCAMP/issues/192

        // do insert/update
        systemSettingsSvc.upsertSubscription(subscription).then(
            // get succeeded
            function (data) {
                window.alert("Subscription successfully saved")
                // reload list of system admins
                $scope.getSubscriptions();
            },
            // get failed
            function (status) {
                window.alert("Add/Update failed");
            }
        );
        
        $('#updateSubscriptionModal').modal('hide');
    };

    // revoke system administrator permissions for the selected user
    $scope.confirmDeleteSubscription = function (subscription) {
        console.log("calling settingsCtrl.confirmDeleteSubscription");

        var wndRsp = window.confirm("Are you sure you want to remove subscription '" + subscription.name + "'? All SCAMP managed resources withing this subscription will be permanently destroyed.")
        if (wndRsp == true) {
            systemSettingsSvc.deleteSubscription(subscription.id).then(
                // get succeeded
                function (data) {
                    window.alert("Subscription deletion has been requested.")
                    // reload list of system admins
                    $scope.getSubscriptions();
                },
                // get failed
                function (status) {
                    window.alert("deletion failed");
                }
            );
        } else {
            window.alert("Operation Cancelled.");
        }
    };

}]);