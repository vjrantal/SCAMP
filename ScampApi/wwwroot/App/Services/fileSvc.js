'use strict';
angular.module('scamp')
.factory('fileSvc', ['$http', '$q', function ($http, $q) {
    return {

        // downloads a file via $http call
        downloadFile: function (fileUrl,
                                defaultContentType,
                                defaultFileName) {
            // defaults
            defaultContentType = defaultContentType || 'application/octet-stream';
            defaultFileName = defaultFileName || 'newfile.bin';
            
        

            var deferred = $q.defer();
            // the assumption here that all files will be downloaded via get. 
            // if you want to do something funky like downloading a file using post or put
            // them rewrite method to support passing in HTTP method along with writing to the upstream. 

            $http.get(fileUrl,  { responseType: 'arraybuffer' }).
                success(function (data, status, headers, config) {
                    headers = headers();
                    // server always wins
                    //var contentType = headers['content-type'] || defaultContentType;
                    if (headers['content-disposition'])
                    {
                        defaultFileName = headers['content-disposition'];
                        defaultFileName = defaultFileName.substring(defaultFileName.indexOf("filename=") + 9);
                    }
                    console.log(defaultFileName);
                    

                  try {
                       
                    var a = document.createElement("a");
                    document.body.appendChild(a);

                    var blob = new Blob([data], { type: defaultContentType });
                    URL = window.URL || window.webkitURL;
                    var url = URL.createObjectURL(blob);

                    a.href = url;
                    a.download = defaultFileName;
                    a.click();
                    URL.revokeObjectURL(url);
                    document.body.removeChild(a);


                    console.log("saveBlob succeeded");
                    deferred.resolve(defaultFileName);
                    }
                    catch (e)
                    {
                        deferred.reject("failed to save file: " + defaultFileName + "[" + e.message + "]");
                    }
                }).
                error(function (data, status, headers, config) {
                    deferred.reject(status);
                })

            return deferred.promise;
        }
};
}]);