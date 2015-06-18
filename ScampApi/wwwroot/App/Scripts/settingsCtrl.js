'use strict';
angular.module('scamp')
.controller('settingsCtrl', ['$scope', 'systemSettingsSvc', 'userSvc', function ($scope, systemSettingsSvc, userSvc) {
    $scope.currentRouteName = 'Settings';

    var valueProperty = 'id'; //The property name for the unique id of the selected option from the RPC

    /// 
    // Set up the type ahead controls.
    ///
    var searchUsers = function (keyword) {
        return userSvc.searchUsers(keyword)
        .then(function (response) {
            return response;
        });
    };
    $scope.addSystemAdmin = {
        'searchUsers' : searchUsers,
        'userSelected': function ($item, $model, $label) {
            $scope.addSystemAdmin.selectedUser = $item;
        },
        'add': function () {
            systemSettingsSvc.grantSysAdmin($scope.addSystemAdmin.selectedUser)
            .then(function (response) {
                $scope.getSystemAdmins(); // reload list of system admins
            })
            .catch(function (status) {
                window.alert('Add failed');
            })
            .finally(function () {
                $scope.addSystemAdmin.selectedUserName = '';
            })
        }
    };
    $scope.addGroupAdmin = {
        'searchUsers' : searchUsers,
        'userSelected': function ($item, $model, $label) {
            // check if user is already in list

            // if not, set them as selected
            $scope.selectedGroupAdmin = $item;
            $scope.addGroupAdmin.selectedUserName = '';
        }
    };

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

    // launch the group manager modal pop-up and set up for add/edit
    $scope.confirmAdminUpdate = function (admin, $event) {
        console.log("callinng settingsCtrl.confirmAdminUpdate");

        $scope.adminActionLabel = (admin == null ? "Add" : "Update");
        if (admin == null)
            admin = {}; // create empty object

        $scope.selectedGroupAdmin = admin;

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
            groupAdmin.id = $scope.selectedGroupAdmin.id;
            groupAdmin.name = $scope.selectedGroupAdmin.name
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