'use strict';
angular.module('scamp')
.factory('groupsSvc', ['$http', '$q', function ($http, $q) {
    var apiPath = '/api/groups';

    return {
        getGroup: function (groupId) {
            return scamp.utils.restAjaxPromise($http, $q, 'GET', apiPath + '/' + groupId);
        },
        getGroupUsers: function (groupId) {
            return scamp.utils.restAjaxPromise($http, $q, 'GET', apiPath + '/' + groupId + '/users');
        },
        addGroup: function (group) {
            return scamp.utils.restAjaxPromise($http, $q, 'POST', apiPath, group);
        },
        updateGroup: function (group) {
            return scamp.utils.restAjaxPromise($http, $q, 'PUT', apiPath + '/' + group.id, group);
        },
        removeGroup: function (group) {
            return scamp.utils.restAjaxPromise($http, $q, 'DELETE', apiPath + '/' + group.id);
        }
    };
}]);