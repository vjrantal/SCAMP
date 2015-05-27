'use strict';
angular.module('scamp')
.factory('userSvc', ['$http', '$q', function ($http, $q) {

    return {
        // gets a list of resources for the selected user
        getResourceList: function (userId, groupId) {
            console.log("retrieving resource list for user " + userId + " Group " + groupId);
            var url = '/api/group/' + groupId + '/user/' + userId + '/resources/';

            return scamp.utils.restAjaxPromise($http, $q, 'GET', url);
        },

        getGroupList: function(userId, view){
            console.log("retrieving group list for user " + userId + " view: " + view);
            var restCall = '/api/groups/' + view + '/' + userId;

            return scamp.utils.restAjaxPromise($http, $q, 'GET', restCall);
        }
  };
}]);