'use strict';
angular.module('scamp')
.factory('dashboardSvc', ['$http', '$q', function ($http, $q) {
    var apiPath = '/api/users';

    return {
        getItems: function () {
            return $http.get(apiPath);
        },
        getItem: function (id) {
            return $http.get(apiPath + id);
        },
        getSummary: function(userId, summaryType){
            var url = apiPath + '/' + userId + '/' + summaryType + '/summary';

            return scamp.utils.restAjaxPromise($http, $q, 'GET', url);
        },
        postItem: function (item) {
            return $http.post(apiPath, item);
        },
        putItem: function (item) {
            return $http.put(apiPath, item);
        },
        deleteItem: function (id) {
            return $http({
                method: 'DELETE',
                url: apiPath + id
            });
        }
    };
}]);