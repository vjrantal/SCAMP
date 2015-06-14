'use strict';
angular.module('scamp')
.controller('groupManagerCtrl', ['$scope', 'userSvc', 'groupsSvc', function ($scope, userSvc, groupsSvc) {
    $scope.viewLoading = true;

    // A variable to store group information locally
    $scope.groups = [];
    var removeLocalGroup = function (groupId) {
        $scope.groups = $scope.groups.filter(function (value) {
            return value.id !== groupId;
        });
    };
    var updateLocalGroup = function (group) {
        for (var i = 0; i < $scope.groups.length; i++) {
            if ($scope.groups[i].id === group.id) {
                $scope.groups[i] = group;
            }
        }
    };

    userSvc.getGroupList($scope.userProfile.id, 'admin')
    .then(function (response) {
        // TODO: Handle case where user doesn't belong to any groups
        $scope.groups = response;
        $scope.selectedGroup = response[0];
        loadGroupDetails($scope.selectedGroup);
    })
    .finally(function () {
        $scope.viewLoading = false;
    });

    var loadGroupDetails = function (group) {
        $scope.groupDetailsLoading = true;
        groupsSvc.getGroup(group.id)
        .then(function (response) {
            $scope.selectedGroup = response;
        })
        .finally(function () {
            $scope.groupDetailsLoading = false;
        });
    };

    $scope.groupSelected = function ($event, group) {
        $scope.selectedGroup = group;
        $scope.groupDetailsLoading = true;
        setTimeout(function () {
          loadGroupDetails(group);
        }, 1000);
    };

    $scope.onlyNumbersPattern = /^\d+$/;
    // Set the minimun expiry date for today
    $scope.groupExpiryMinDate = new Date();
    $scope.groupExpiryDateOpened = false;
    $scope.groupExpiryDateOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.groupExpiryDateOpened = true;
    };

    $scope.groupSummaryFormSubmitted = function () {
        $scope.groupDetailsLoading = true;
        if ($scope.selectedGroup.unsaved) {
            groupsSvc.addGroup($scope.selectedGroup)
            .then(function (response) {
                $scope.groups.push(response);
                $scope.selectedGroup = response;
            })
            .finally(function () {
                $scope.groupDetailsLoading = false;
            });
        } else {
            groupsSvc.updateGroup($scope.selectedGroup)
            .then(function (response) {
                updateLocalGroup(response);
                $scope.selectedGroup = response;
            })
            .finally(function () {
                $scope.groupDetailsLoading = false;
            });
        }
    };

    $scope.addGroup = function () {
        var group = {
            'id': 'the-temporary-id-of-unsaved-group',
            'unsaved': true
        };
        $scope.selectedGroup = group;
    };

    $scope.removeGroup = function () {
        if ($scope.selectedGroup.unsaved) {
            removeLocalGroup('the-temporary-id-of-unsaved-group');
            $scope.selectedGroup = $scope.groups[0];
        } else {
            $scope.groupDetailsLoading = true;
            groupsSvc.removeGroup($scope.selectedGroup)
            .then(function () {
                removeLocalGroup($scope.selectedGroup.id);
                $scope.selectedGroup = $scope.groups[0];
                loadGroupDetails($scope.selectedGroup);
            })
            .finally(function () {
                $scope.groupDetailsLoading = false;
            });
        }
    };
}]);