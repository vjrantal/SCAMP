'use strict';
angular.module('scamp')
.factory('homeSvc', ['$http', function ($http) {
    // https://localhost:44300/api/currentUser

    var apiPath = '/api/currentUser';

    return {        
        getUserProfile: function () {
            return $http.get(apiPath);
        }
    };
}]);