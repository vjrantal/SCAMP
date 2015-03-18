'use strict';
angular.module('scamp')
.controller('resourcesCtrl', ['$scope', '$location', 'resourcesSvc', 'adalAuthenticationService', function ($scope, $location, resourcesSvc, adalService) {
    $scope.error = "";
    $scope.loadingMessage = "Loading...";
    $scope.resourceList = null;
    $scope.editingInProgress = false;
    $scope.newresourceCaption = "";


    $scope.editInProgressresource = {
        Description: "",
        ID: 0
    };

    //$scope.addResource = function (resource) {
    //    var name = resource.rName,
    //        state = resource.rState,
    //        remaining = resource.rRemaining,
    //        groupString = resource.rGroup.id,
    //        group = parseInt(groupString);

    //    console.log(group);

    //    var newResource = this.store.createRecord("resource", {
    //        name: name,
    //        state: state,
    //        remaining: remaining
    //    });

    //    this.store.find("group", group).then(function (foundGroup) {
    //        newResource.set("group", foundGroup);
    //        newResource.save();
    //    });
    //}

    $scope.editSwitch = function (resource) {
        resource.edit = !resource.edit;
        if (resource.edit) {
            $scope.editInProgressresource.Description = resource.Description;
            $scope.editInProgressresource.ID = resource.ID;
            $scope.editingInProgress = true;
        } else {
            $scope.editingInProgress = false;
        }
    };

    $scope.populate = function () {
        resourceListSvc.getItems().success(function (results) {
            $scope.resourceList = results;
            $scope.loadingMessage = "";
        }).error(function (err) {
            $scope.error = err;
            $scope.loadingMessage = "";
        })
    };
    $scope.delete = function (id) {
        resourceListSvc.deleteItem(id).success(function (results) {
            $scope.loadingMessage = "";
            $scope.populate();
        }).error(function (err) {
            $scope.error = err;
            $scope.loadingMessage = "";
        })
    };
    $scope.update = function (resource) {
        resourceListSvc.putItem($scope.editInProgressresource).success(function (results) {
            $scope.loadingMsg = "";
            $scope.populate();
            $scope.editSwitch(resource);
        }).error(function (err) {
            $scope.error = err;
            $scope.loadingMessage = "";
        })
    };
    $scope.add = function () {

        resourceListSvc.postItem({
            'Description': $scope.newresourceCaption,
            'Owner': adalService.userInfo.userName
        }).success(function (results) {
            $scope.loadingMsg = "";
            $scope.newresourceCaption = "";
            $scope.populate();
        }).error(function (err) {
            $scope.error = err;
            $scope.loadingMsg = "";
        })
    };
}]);