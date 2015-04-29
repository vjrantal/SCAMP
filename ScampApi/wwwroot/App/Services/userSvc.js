'use strict';
angular.module('scamp')
.factory('userSvc', ['$http', '$q', function ($http, $q) {
    return {

        // gets a list of resources for the selected user
        getResourceList: function (userId) {
            console.log("retrieving resource list for user " + userId);

            var deferred = $q.defer();

            $http({ method: 'GET', url: '/api/user/' + userId + '/resources/' }).
                success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).
                error(function (data, status, headers, config) {
                    deferred.reject(status);
                })

            return deferred.promise;
        }
};
}]);