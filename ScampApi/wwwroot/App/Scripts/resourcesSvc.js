'use strict';
angular.module('scamp')
.factory('resourcesSvc', ['$http', function ($http) {
    return {
        getItems : function(){
            return $http.get('/api/resources');
        },
        getItem : function(id){
            return $http.get('/api/resources/' + id);
        },
        postItem : function(item){
            return $http.post('/api/resources/', item);
        },
        putItem : function(item){
            return $http.put('/api/resources/', item);
        },
        deleteItem : function(id){
            return $http({
                method: 'DELETE',
                url: '/api/resources/' + id
            });
        }
    };
}]);