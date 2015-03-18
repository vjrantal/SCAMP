'use strict';
angular.module('scamp')
.factory('resourcesSvc', ['$http', function ($http) {
    return {
        getItems : function(){
            return $http.get('/api/user/resources');
        },
        getItem : function(id){
            return $http.get('/api/user/resources/' + id);
        },
        postItem : function(item){
            return $http.post('/api/user/resources/', item);
        },
        putItem : function(item){
            return $http.put('/api/user/resources/', item);
        },
        deleteItem : function(id){
            return $http({
                method: 'DELETE',
                url: '/api/user/resources/' + id
            });
        }
    };
}]);