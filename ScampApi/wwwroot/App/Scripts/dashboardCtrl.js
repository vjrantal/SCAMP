'use strict';

angular.module('scamp')
.controller('dashboardCtrl', ['$scope', '$modal', '$location', 'dashboardSvc', 'groupsSvc', 'userSvc', 'adalAuthenticationService', 'resourcesSvc', 'fileSvc', function ($scope, $modal, $location, dashboardSvc, groupsSvc, userSvc, adalService, resourcesSvc, fileSvc) {


    var scampDashboard = new ScampDashboard($scope);
    $scope.currentRouteName = 'Dashboard';
    $scope.isAdmin = true;

	$scope.userList = null;
	$scope.rscStateDescMapping = {
	    12: {description: "Unknown", allowableActions : [] },
	    0: {description: "Allocated", allowableActions : ["Start", "Delete"] },
	    2: {description: "Starting", allowableActions : [], previousAction: "Start" },
	    3: {description: "Running", allowableActions : ["Connect", "Stop"] },
	    4: {description: "Stopping", allowableActions : [], previousAction: "Stop" },
	    5: {description: "Stopped", allowableActions: ["Start", "Delete"] },
	    6: {description: "Suspended", allowableActions: ["Start", "Delete"] },
	    7: {description: "Deleting", allowableActions: [] }
	};

	$scope.resourceTypes = {
	    0: "Virtual Machine",
	    2: "Web App"
	};

	$scope.populate = function () {
	  var populateDashboard = function () {
	    scampDashboard.initialize();
	    var userGUID = $scope.userProfile.id;

	    //Default the view to admin view as long as the user has administrative priviledges or keep the view as it is if it's set to admin
	    if (!$scope.dashboardView && $scope.isAdmin || ($scope.dashboardView && $scope.dashboardView == 'admin'))
	      $scope.dashboardView = 'admin';
	    else
	      $scope.dashboardView = 'user';

	    $scope.dashboardStatus = 'loading';

	    userSvc.getGroupList(userGUID, $scope.dashboardView)
      .then(function (data) {
        if (data && data.length > 0) {
          $scope.groups = data.map(function (item) {
            return scampDashboard.computeUsagePercentages(item);
          });
          $scope.selectedGroupId = data[0].id;
          $scope.selectedGroupName = data[0].name;
          var summaryPanelType = 'usage';

          if ($scope.dashboardView == 'admin') {
            $scope.loadUsers($scope.selectedGroupId);
            summaryPanelType = 'budget';
          } else
            $scope.loadResources($scope.selectedGroupId, userGUID);

          $scope.updateSummaryPanel(userGUID, summaryPanelType);
          $scope.dashboardStatus = 'loaded';
        } else {
          throw new Error("User " + userGUID + " doesnt have permission to any groups for " + $scope.dashboardView + " view");
        }
      })
      .catch(function (statusCode) {
        // resource REST call failed
        console.error(statusCode);
        $scope.dashboardStatus = 'loaded';
      });
	  };
	  if ($scope.userProfile) {
	    populateDashboard();
	  } else {
	    $scope.$on('userProfileFetched', populateDashboard);
	  }
	};

	$scope.loadUsers = function (groupId) {
	    if (!groupId)
	        throw new Error("Mandatory paramter groupId needs to be specified");

	    groupsSvc.getUsers(groupId).then(
            function (data) {
                if (data && data.length > 0) {
                    $scope.groupUsers = data.map(function (item) {
                        var totalUnitsAllocated = item.totUnitsUsed + item.totUnitsRemaining;
                        item.userUsage = (totalUnitsAllocated > 0) ? Math.round((item.totUnitsUsed / totalUnitsAllocated) * 100) : 0;

                        return item;
                    });
                    
                    $scope.loadResources(groupId, data[0].id);
                }
            },
            // resource REST call failed
            function (statusCode) {
                console.error(statusCode);
            }
        );
	};

	$scope.updateSummaryPanel = function (userId, type) {
	    dashboardSvc.getSummary(userId, type).then(
            function (data) {
                if (data) 
                    scampDashboard.updateSummaryPanel(data, type);
            },
            // resource REST call failed
            function (statusCode) {
                console.error(statusCode);
            }
        );
	};

	$scope.loadResources = function (groupId, userId) {
	    if (!groupId || !userId)
	        throw new Error("Mandatory parameter groupId and userId need to be specified");

	    scampDashboard.setCurrentUser(userId);
	    userSvc.getResourceList(userId, groupId).then(
            function (data) {
                if (data && data.length > 0) {
                    scampDashboard.render(data);
                }
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

	var callResourceService = function (item, action, duration, cb) {
	    console.log("Attempting to " + action + " resource" + item.id + " in group " + item.groupId)
	    resourcesSvc.sendAction(item.groupId, item.id, action, duration).success(function (results) {
	        $scope.loadingMessage = "";
	        cb();
	    }).error(function (err) {
	        console.log('Error attempting to Start a resource');
	        $scope.error = err;
	        $scope.loadingMessage = "";
	    });
	}
	
	$scope.testWebSockStateUpdate = function(){
	    var testMsg = {
	        state: 2,
	        resource: '8808789d-e889-4127-b6c2-baeb59c39d09',
	        user: 'a144e3bd-5b25-4162-b927-3c17e4f7235a-8bc0004a-1af0-42d1-87e6-ac92bed0c93f',
	        action: 'update',
	        date: 'March 3, 2015 14:34:0338'
	    }

	    var targetRsc = $scope.resources.filter(function (item) { return item.id === testMsg.resource });
	    if (!targetRsc)
	        console.error('Couldnt find targeted resource ' + testMsg.resource);
	    targetRsc = targetRsc[0];

	    testMsg.state = targetRsc.state === 9 ? 0 : targetRsc.state + 1;//increment the state with each click. If'ts 9 then reset it to 0

	    scampDashboard.updateRsrcScopeFromWSUpdate(testMsg);
	}

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

	$scope.confirmResourceAction = function (actionSelection, rsc, event) {
	    var defaultDurationHrs = 8;
	    console.log(actionSelection);
	    console.log("Action '" + actionSelection + "' requested on resource " + rsc.id);
	    $scope.dashboardStatus = 'loading';

	    if (actionSelection.action == "Connect")
	    {
	        var groupId = $scope.selectedGroupId,
                resourceId = rsc.id,		
                contentType = "application/rdp; charset=utf-8",		
                fileName = "service.rdp"		
        		
	        var Fileurl = "/api/groups/" + groupId + "/resources/" + resourceId + "/rdp";		
        		
	        console.log("Get Rdp: " + Fileurl)		
        		
	        fileSvc.downloadFile(Fileurl, contentType, fileName).then(		
                function (fileName) {		
                    console.log("File downloaded: " + fileName);
                    $scope.dashboardStatus = 'loaded';
                },function (error) {		
                    console.log("Failed to download file" + error);
                    $scope.dashboardStatus = 'loaded';
                });		
	    }
	    else
	    {
	        $scope.resourceSave = {
	            name: rsc.name,
	            currentStateDesc: rsc.stateDescription,
	            id: rsc.id,
	            duration: defaultDurationHrs,
	            newStateDesc: actionSelection.action
	        };

	        event.preventDefault();
	        $scope.dashboardStatus = 'loaded';
	    }
	};

	$scope.resourceSendAction = function () {
	    if ($scope.resourceSave && $scope.resourceSave.newStateDesc && $scope.resourceSave.id) {
	        var resourceIdToSave = $scope.resourceSave.id, updatedDashboardState = -1;
	        var rscAction = $scope.resourceSave.newStateDesc;
	        var resource = $scope.resources.filter(function (item) { return item.id === resourceIdToSave});
	        resource = (resource && resource.length > 0) ? resource[0] : resource;
	        var duration = (rscAction==='Start')?$scope.resourceSave.duration * 60:-1; //convert the duration from hours to minutes

	        _.each($scope.rscStateDescMapping, function (value, key) {
	            if (value.previousAction && value.previousAction === rscAction)
	                updatedDashboardState = key; });

	        var dashboardRscUpdateMsg = {
	            state: updatedDashboardState,
	            resource: resourceIdToSave,
	        };

	        var callback = function () {scampDashboard.processResourceUpdate(dashboardRscUpdateMsg)}
	        callResourceService(resource, $scope.resourceSave.newStateDesc, duration, callback);
	    }else
	        throw "Unsupported action was called for resourceSendAction";

	    $('#resourceSendActionModal').modal('hide');
	};

}])
.directive("radioButton", function () {//This is a workaround due to a Bootstrap 3 bug with Angular which doesnt support radio buttons. 
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {
            element.bind('click', function () {
                if (!element.hasClass('active')) {
                    scope.$apply(function () {
                        scope.dashboardView = attrs.value;
                        scope.populate();
                    });
                }
              }
            );
        }
    }
});

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