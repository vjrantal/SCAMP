var scamp = {};

scamp.utils = (function () {
    var self = {};
    
    self.restAjaxPromise = function ($http, $q, action, url, data) {
        var deferred = $q.defer();

        $http({ method: action, url: url, data: data }).
            success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).
            error(function (data, status, headers, config) {
                deferred.reject(status);
            });

        return deferred.promise;
    };

    return self;
})();