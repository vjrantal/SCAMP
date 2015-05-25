'use strict';
angular.module('scamp')
.factory('userSvc', ['$http', '$q', function ($http, $q) {

    var restAjaxPromise = function (action, url) {
        var deferred = $q.defer();

        $http({ method: action, url: url }).
            success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).
            error(function (data, status, headers, config) {
                deferred.reject(status);
            })

        return deferred.promise;
    };

    return {
        // gets a list of resources for the selected user
        getResourceList: function (userId, groupId) {
            console.log("retrieving resource list for user " + userId + " Group " + groupId);
            var url = '/api/group/' + groupId + '/user/' + userId + '/resources/';

            return restAjaxPromise('GET', url);
        },

        getGroupList: function(userId, view){
            console.log("retrieving group list for user " + userId + " view: " + view);
            var restCall = '/api/groups/' + view + '/' + userId;

            return restAjaxPromise('GET', restCall);
        }
  };
}]);