'use strict';
angular.module('scamp')
.factory('homeSvc', ['$http', function ($http) {

    var apiPath = '/api/user';

    return {        
        getUserProfile: function () {
            return $http.get(apiPath);
        }
    };
}]);