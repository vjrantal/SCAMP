'use strict';
angular.module('scamp')
.controller('settingsCtrl', ['$scope', '$location', 'systemSettingsSvc', 'adalAuthenticationService', function ($scope, $location, systemSettingsSvc, adalService) {
    $scope.currentRouteName = 'Settings';

    var valueProperty = 'id'; //The property name for the unique id of the selected option from the RPC

    /// 
    // set up the type ahead control for system admins
    ///
    var sysAdminLookupConfig = {
        componentId: 'addSysAdmin',
        minLength: 3, //The minimum character length needed before suggestions start getting rendered. Defaults to 1
        scopeValueBindedPropertyOnSelection: 'id',
        remote: {
            url: '/api/users/FindbyUPN/%QUERY',
            queryStr: '%QUERY',
            displayProperty: 'name' //This is the property referenced from the response to determine what display on the control
        }
    };
    // callback method for when a user is selected
    var selectedSystemAdmin = function (e, datum) {
        systemSettingsSvc.grantSysAdmin(datum).then(
            // get succeeded
            function (data) {                
                $scope.getSystemAdmins(); // reload list of system admins
            },
            // get failed
            function (status) {
                window.alert("Add failed");
            }
        );
    };
    // connecting the config and callback with the control
    var sysAdminTypeaheadControl = new Typeahead($scope, sysAdminLookupConfig, selectedSystemAdmin);

    /// 
    // set up the type ahead control for group managers
    ///
    var sysGroupLookupConfig = { // configuration for the lookup control
        componentId: 'addGrpManager',
        minLength: 3, //The minimum character length needed before suggestions start getting rendered. Defaults to 1
        scopeValueBindedPropertyOnSelection: 'id',
        remote: {
            url: '/api/users/FindbyUPN/%QUERY',
            queryStr: '%QUERY',
            displayProperty: 'name' //This is the property referenced from the response to determine what display on the control
        }
    };
    // lookup controls callback
    var selectedGroupManager = function (e, datum) {
        // check if user is already in list

        // if not, set them as selected
        $scope.selectedManagerUser = datum;
    };
    // connecting the config and callback with the control
    var sysGroupTypeaheadControl = new Typeahead($scope, sysGroupLookupConfig, selectedGroupManager);

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
    $scope.getGroupAdmins = function () {
        console.log("calling: settingsCtrl.getGroupAdmins");
        systemSettingsSvc.getGroupAdmins().then(
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

    // launch the Manager modal pop-up and set up for add/edit
    $scope.confirmAdminUpdate = function (admin, $event) {
        console.log("callinng settingsCtrl.confirmAdminUpdate");

        $scope.managerActionLabel = (admin == null ? "Add" : "Update");
        if (admin == null)
            admin = {}; // create empty object

        $scope.selectedGroupManager = admin;

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

    // close execute subscription modal pop-up action and close window
    $scope.adminSave = function (groupAdmin, $event) {
        console.log("calling settingsCtrl.subscriptionSave");

        //TODO: validate parameters
        // https://github.com/SimpleCloudManagerProject/SCAMP/issues/196

        // if we were doing an add, update object with selected user
        if (groupAdmin.id == null) {
            groupAdmin.id = $scope.selectedManagerUser.id;
            groupAdmin.name = $scope.selectedManagerUser.name
        }

        // do insert/update
        systemSettingsSvc.updateAdmin(groupAdmin).then(
            // get succeeded
            function (data) {
                console.log("group Admin saved.")
                // reload list of system admins
                $scope.getGroupAdmins();
            },
            // get failed
            function (status) {
                window.alert("group admin Add/Update failed");
            }
        );

        $('#updateGroupAdminModal').modal('hide');
    };

    // revoke system administrator permissions for the selected user
    $scope.confirmDeleteSubscription = function (subscription) {
        console.log("calling settingsCtrl.confirmDeleteSubscription");

        var wndRsp = window.confirm("Are you sure you want to remove subscription '" + subscription.name + "'? All SCAMP managed resources within this subscription will be permanently destroyed.")
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

    // revoke group manager permissions for the selected user
    $scope.confirmDeleteAdmin= function (groupAdmin) {
        console.log("calling settingsCtrl.confirmDeleteAdmin");

        var wndRsp = window.confirm("Are you sure you want to remove '" + groupAdmin.name + "' as a group admin? All SCAMP managed resources associated with their groups will be permanently destroyed.")
        if (wndRsp == true) {
            systemSettingsSvc.deleteGroupAdmin(groupAdmin.id).then(
                // get succeeded
                function (data) {
                    window.alert("Group Admin resource deletion has been requested.")
                    // reload list of group managers
                    $scope.getGroupAdmins();
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

    // Set the minimun expiry date for today
    $scope.budgetExpiryMinDate = new Date();
    $scope.budgetExpiryDateOpened = false;
    $scope.budgetExpiryDateOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.budgetExpiryDateOpened = true;
    };
}]);