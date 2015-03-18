'use strict';
angular.module('scamp')
.factory('resourcesSvc', ['$http', function ($http) {
    return {
        getItems : function(){
            return $http.get('/api/Resources');
        },
        getItem : function(id){
            return $http.get('/api/Resources/' + id);
        },
        postItem : function(item){
            return $http.post('/api/Resources/', item);
        },
        putItem : function(item){
            return $http.put('/api/Resources/', item);
        },
        deleteItem : function(id){
            return $http({
                method: 'DELETE',
                url: '/api/Resources/' + id
            });
        }
    };
}]);