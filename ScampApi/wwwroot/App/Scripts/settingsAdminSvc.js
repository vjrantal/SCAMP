'use strict';
angular.module('scamp')
.factory('settingsAdminsSvc', ['$http', '$q', function ($http, $q) {

    return {
        getSystemAdmins: function () {
            return [
                {
                    name: "John Doe",
                    email: "johndoe@hotmail.com",
                },
                {
                    name: "Jane Smith",
                    email: "janesmith@outlook.com",
                },
                {
                    name: "Bob the Builder",
                    email: "bob@narnia.com",
                }
            ];
        }

    };
}]);