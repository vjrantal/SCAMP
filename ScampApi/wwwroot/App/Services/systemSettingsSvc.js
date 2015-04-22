'use strict';
angular.module('scamp')
.factory('systemSettingsSvc', ['$http', '$q', function ($http, $q) {
    return {
        getSystemAdmins: function () {
            var deferred = $q.defer();

            $http({ method: 'GET', url: '/api/settings/admin' }).
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