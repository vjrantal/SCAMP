'use strict';
angular.module('scamp')
.factory('groupsSvc', ['$http', '$q', function ($http, $q) {
    var apiPath = '/api/group/';

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
        getItems: function () {
            return $http.get(apiPath);
        },
        getUsers: function (id) {
            return restAjaxPromise('GET', apiPath + id + '/users');
        },
        postItem: function (item) {
            return $http.post(apiPath, item);
        },
        putItem: function (id, item) {
            return $http.put(apiPath + id, item);
        },
        deleteItem: function (id) {
            return $http({
                method: 'DELETE',
                url: apiPath + id
            });
        }
    };
}]);