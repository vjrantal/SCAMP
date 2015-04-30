'use strict';

angular.module('scamp')
.controller('dashboardCtrl', ['$scope', '$modal', '$location', 'dashboardSvc', 'groupsSvc', 'userSvc', 'adalAuthenticationService', function ($scope, $modal, $location, dashboardSvc, groupsSvc, userSvc, adalService) {
    var scampDashboard = new ScampDashboard($scope);
    $scope.currentRouteName = 'Dashboard';
	$scope.userList = null;
	$scope.rscStateDescMapping = {
	    0: {description: "Allocated", allowableActions : ["Start", "Delete"] },
	    1: {description: "Starting", allowableActions : [] },
	    2: {description: "Running", allowableActions : ["Stop"] },
	    3: {description: "Stopping", allowableActions : [] },
	    4: {description: "Stopped", allowableActions: ["Start", "Delete"] },
	    5: {description: "Suspended", allowableActions: ["Start", "Delete"] },
	    6: {description: "Deleting", allowableActions: [] }
	};

	$scope.resourceTypes = {
	    1: "Virtual Machine",
	    2: "Web App"
	};

	$scope.populate = function () {
	    scampDashboard.initializeGrid();

	    var userGUID = $scope.userProfile.id;
	    $scope.dashboardStatus = 'loading';

	    userSvc.getResourceList(userGUID).then(
            // resource REST call was a success
            function (data) {
                scampDashboard.render(data);
            },
            // resource REST call failed
            function (statusCode) {
                console.error(statusCode);
            }
        );
	};

	$scope.manageGroup = function (groupId) {
		var modalInstance = $modal.open({
			templateUrl: 'GroupUsers.html',
			controller: 'GroupUsersModalCtrl',
			size: 'lg',
			resolve: {
				groupSvc: function () {
					return groupsSvc;
				},
				group: function () {
					return groupsSvc.getItem(groupId);
				},
				users: function () {
					return $scope.userList;
				},
				currentUser: function () {
					return $scope.userProfile;
				}
			}
		});
	};

    // get the list of current system administrators
	$scope.testGetCurrentUserResources = function () {
	    userSvc.getResourceList($scope.userProfile.id).then(
            // get succeeded
            function (data) {
                console.log(data);
            },
            // get failed
            function (statusCode) {
                console.log(statusCode);
            }
        );
	};

}]);


angular.module('scamp')
.controller('GroupUsersModalCtrl', function ($scope, $modalInstance, groupSvc, group, users, currentUser) {

	$scope.group = group.data;

	$scope.isGroupAdmin = userInArray(currentUser.id, $scope.group.admins);

	$scope.currentUser = currentUser;

	// TODO: load only if it's necessary
	$scope.users = users;

	$scope.done = function () {
		$modalInstance.dismiss('done');
	};

	$scope.addAdmin = function (user) {
		var newGroup = JSON.parse(JSON.stringify($scope.group)); // clone group

		newGroup.admins.push(user);
		groupSvc.putItem(newGroup.groupId, newGroup).success(function (result) {
			if (!result) { // error
				alert("An error occured");
			}
			else {
				$scope.group = result;

				if (user.userId == currentUser.id) // current user adds him-self 
					$scope.isGroupAdmin = true;
			}
		});
	};

	$scope.removeAdmin = function (user) {
		var newGroup = JSON.parse(JSON.stringify($scope.group)); // clone group

		var index = $scope.group.admins.indexOf(user);
		if (index > -1) {
			newGroup.admins.splice(index, 1);
		}

		groupSvc.putItem(newGroup.groupId, newGroup).success(function (result) {
			if (!result) { // error
				alert("An error occured");
			}
			else {
				$scope.group = result;

				if (user.userId == currentUser.id) // current user removes him-self 
					$scope.isGroupAdmin = false;
			}
		});
	};
})
.filter('notAdmin', function () {
	return function (users, admins) {
		var filtered = [];
		var a;
		var found = false;
		for (var u in users) {
			if (!userInArray(u.userId, admins))
				filtered.push(u);
		}
		return filtered;
	};
});

function userInArray(userId, array) {
	for (var u in array) {
		if (userId == u.userId) {
			return true;
		}
	}
	return false;
}