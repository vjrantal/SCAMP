'use strict';
angular.module('scamp')
.controller('groupManagerCtrl', ['$scope', 'userSvc', 'groupsSvc', function ($scope, userSvc, groupsSvc) {
    $scope.viewLoading = true;

    userSvc.getGroupList($scope.userProfile.id, 'admin')
    .then(function (data) {
        $scope.groups = data;
        $scope.selectedGroup = data[0];
        loadGroupDetails($scope.selectedGroup);
    })
    .catch(function () {
    })
    .finally(function () {
        $scope.viewLoading = false;
    });

    var loadGroupDetails = function (group) {
        $scope.groupDetailsLoading = true;
        groupsSvc.getItem(group.id)
        .then(function (data) {
            $scope.selectedGroup = data;
        })
        .catch(function () {
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
}]);