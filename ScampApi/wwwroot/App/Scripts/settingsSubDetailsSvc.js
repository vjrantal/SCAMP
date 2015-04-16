'use strict';
angular.module('scamp')
.factory('settingsSubDetailsSvc', ['$http', '$q', function ($http, $q) {

    return {

        // gets specified subscription details
        getSub: function (subID) {
            return {
                subname: "Primary Subscription",
                subID: subID,
                subAdmin: "johndoe@hotmail.com",
                LastChgd: "10/10/2015",
                limits : [
                    {
                        type: "cloud service",
                        current: 20,
                        maximum: 25
                    },
                    {
                        type: "storage accounts",
                        current: 10,
                        maximum: 100
                    },
                    {
                        type: "resource groups",
                        current: 40,
                        maximum: 500
                    },
                    {
                        type: "compute cores",
                        current: 40,
                        maximum: 350
                    },
                    {
                        type: "web hosting plans",
                        current: 40,
                        maximum: 5000
                    }

                ],
                events : [
                    {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }, {
                        timestamp: "10/10/2015 12:00:00pm",
                        level: "info",
                        description: "blah blah blah"
                    }
                ]
            };
        }
    };
}]);