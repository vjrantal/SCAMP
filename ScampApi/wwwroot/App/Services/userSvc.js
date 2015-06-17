'use strict';
angular.module('scamp')
.factory('userSvc', ['$http', '$q', function ($http, $q) {

    return {
        // gets a list of resources for the selected user
        getResourceList: function (userId, groupId) {
            console.log("retrieving resource list for user " + userId + " Group " + groupId);
            var url = '/api/groups/' + groupId + '/users/' + userId + '/resources';

            return scamp.utils.restAjaxPromise($http, $q, 'GET', url);
        },

        getGroupList: function(userId, view){
            console.log("retrieving group list for user " + userId + " view: " + view);
            var restCall = '/api/users/' + userId + '/groups/' + view;

            return scamp.utils.restAjaxPromise($http, $q, 'GET', restCall);
        },

        searchUsers: function (keyword) {
            var url = '/api/users/FindbyUPN/' + keyword;
            return scamp.utils.restAjaxPromise($http, $q, 'GET', url);
        }
    };
}]);