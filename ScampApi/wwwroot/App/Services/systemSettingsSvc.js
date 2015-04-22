'use strict';
angular.module('scamp')
.factory('systemSettingsSvc', ['$http', '$q', function ($http, $q) {
    return {
        // gets a list of all the SCAMP system admins
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
        },

        // revoke user's SCAMP system admin permissions
        revokeAdmin: function (id) {
            console.log("removing system admin permissions on id:" + id);

            var deferred = $q.defer();
           
            $http.post('/api/settings/admin', id).
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