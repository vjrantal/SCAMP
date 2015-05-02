function ScampWebSocket($scope, host, port) {
    var scope = $scope;
    this.wsHost = host;
    this.wsPost = port;
    this.wsConnection = undefined;
    var attempts = 1;

    var userGUID;

    if (scope && scope.isLoggedOn && this.wsHost && this.wsPost)
        userGUID = $scope.userProfile.id;
    else
        throw new Exception("Unable to load dashboard due to an unauthenticated profile");

    createWebSocket(userGUID);
    
    var createWebSocket = function(clientId) {
        this.wsConnection = new WebSocket("ws://"+this.host+":"+this.wsPost+"/channel/"+clientId);
        attempts = 1;
        this.wsConnection.onclose = function () {
            var time = generateInterval(attempts);

            setTimeout(function () {
                attempts++;
                // Connection has closed so try to reconnect every wsConnectRetrySeconds seconds.
                createWebSocket(clientId);
            }, time);
        };

        this.wsConnection.onopen = function () {
            attempts = 1;
            console.log('Web Socket connected for client: '+ userGUID);
        };

        var generateInteval = function (k) {
            var maxInterval = (Math.pow(2, k) - 1) * 1000;

            if (maxInterval > 30 * 1000) {
                maxInterval = 30 * 1000; // If the generated interval is more than 30 seconds, truncate it down to 30 seconds.
            }
            // generate the interval to a random number between 0 and the maxInterval determined from above
            return Math.random() * maxInterval;
        };
    };
}