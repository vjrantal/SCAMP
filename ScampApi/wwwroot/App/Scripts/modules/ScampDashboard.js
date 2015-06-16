function ScampDashboard($scope) { 
    var scope = $scope;
    
    //var ws = new ScampWebSocket(scope, "local", "");
    var resourceUsageEscalationLevels = [
        { maxUsage: 50, cssClass: "success" },
        { maxUsage: 70, cssClass: "info" },
        { maxUsage: 85, cssClass: "warning" },
        { maxUsage: 101, cssClass: "danger" }
    ];

    var isNumber = function (o) {
        return typeof o === 'number' && isFinite(o);
    };

    var chartsToRender = [
                { fieldName: 'usageEscalation', domElement: 'usageDonutChart' },
                { fieldName: 'groupName', domElement: 'groupDonutChart' },
                { fieldName: 'resourceTypeDesc', domElement: 'typeDonutChart' },
                { fieldName: 'stateDescription', domElement: 'stateDonutChart' }
    ];

    /*
    ws.wsConnection.onmessage = function (msg) {
        var data = msg.data;

        if(data && scope.resources)
            this.updateRsrcScopeFromWSUpdate(data);
    };*/

    this.updateRsrcScopeFromWSUpdate = function(msg){
        //make sure that the user on the message is the same person logged in
        if (msg.resource && msg.user === scope.userProfile.id)
            this.processResourceUpdate(msg);
        else
            console.error('Received the following resource update without an resource ID');
    }

    this.processResourceUpdate = function (modifiedRsc) {
        var resourceUpdated = false;
        
        scope.resources.map(function (item) {
            if (item.id === modifiedRsc.resource) {
                updateResouce(item, modifiedRsc.state);
                resourceUpdated = true; 
            }
        });
        
        //if the updated resource cannot be found on the scope, then refresh the dashboard by calling the service
        if (!resourceUpdated)
            scope.populate();
        //else
         //   renderCharts(scope.resources);
    };

    this.updateSummaryPanel = function(data){
        scope.summary = {
            'totUnitsUsed': data.totUnitsUsed,
            'pctUsed': (data.totUnitsAllocated > 0) ? Math.round((data.totUnitsUsed / data.totUnitsAllocated) * 100) : 0,  
            'pctAllocated': (data.unitsBudgeted > 0) ? Math.round((data.totUnitsAllocated / data.unitsBudgeted) * 100) : 0,
            'budget': data.unitsBudgeted,
            'noGroups': data.totGroups,
            'totUnitsAllocated': data.totUnitsAllocated
        };

        scope.summary['pctUnallocated'] = (scope.summary['pctAllocated'] != 0 ) ? 100 - scope.summary['pctAllocated'] : 0;
        scope.summary['pctRemaining'] = (scope.summary['pctUsed'] != 0 ) ? 100 - scope.summary['pctUsed'] : 0;
    }

    this.setCurrentUser = function (userId) {
        var user = scope.groupUsers.filter(function (item) {
            if (item.id == userId) return true;
            else return false;
        });

        if (user && user.length == 1)
            user = user[0];
        else
            throw new Error("Unable to find user " + userId + " in the group user list");

        scope.selectedUserId = user.id;
        scope.selectedUserName = user.name;
        var totalUnitsAllocated = user.totUnitsUsed + user.totUnitsRemaining;
        scope.selectedUsersUsage = (totalUnitsAllocated > 0) ? Math.round((user.totUnitsUsed / totalUnitsAllocated) * 100) : 0;
    }

    this.computeUsagePercentages = function (group) {
        if (scope.state.view == 'admin') {
            group.usageLabel = (group.totUnitsBudgeted > 0) ? Math.round((group.totUnitsUsed / group.totUnitsBudgeted) * 100) : 0;
            group.availLabel = (group.totUnitsBudgeted > 0) ? Math.round(((group.totUnitsAllocated - group.totUnitsUsed) / group.totUnitsBudgeted) * 100) : 0;
            group.unallocatedLabel = 100 - (group.usageLabel + group.availLabel);
        } else {
            var totalUnitsAllocated = group.totUnitsRemainingForUser + group.totUnitsUsedByUser;

            group.usageLabel = (totalUnitsAllocated > 0) ? Math.round((group.totUnitsUsedByUser / totalUnitsAllocated) * 100) : 0;
            group.availLabel = (totalUnitsAllocated > 0) ? Math.round((group.totUnitsRemainingForUser / totalUnitsAllocated) * 100) : 0;
        }

        return group;
    };

    var setResources = function(rspData){
        scope.resources = rspData.filter(function (item) {
            return scope.rscStateDescMapping[item.state];//Make sure the state code is valid
        }).map(function (rsc) {
            updateResouce(rsc, rsc.state);//prepare and update the resource object on the scope to be rendered on the dashboard

            return rsc;
        });
    };

    var updateResouce = function(rsc, currentState){
        var alertClassArr = resourceUsageEscalationLevels.filter(function (el) { return rsc.remaining < el.maxUsage; });

        rsc.usageEscalation = alertClassArr && alertClassArr.length > 0 ? alertClassArr[0].cssClass : "danger";
        rsc.state = currentState;
        rsc.stateDescription = scope.rscStateDescMapping[currentState].description;
        rsc.resourceTypeDesc = scope.resourceTypes[rsc.type];
        rsc.usage = (isNumber(rsc.totUnitsUsed)?rsc.totUnitsUsed.toFixed(2):0)+' Units';
    };

    var renderCharts = function(rscs){
        var groupBy = function (resources, groupField, cb) {
            return _.chain(resources).groupBy(groupField).map(cb);
        };

        var renderChart = function (groupByField, cb, chartDomEl, resources) {
            var chartDataSrc = groupBy(resources, groupByField, cb).value();

            Morris.Donut({
                element: chartDomEl,
                data: chartDataSrc
            });
        };

        chartsToRender.forEach(function (item) {
            renderChart(item.fieldName, function (val, key) {
                return { label: key, value: val.length }
            }, item.domElement, rscs);
        });
    };

    this.render = function (rspData) {
        setResources(rspData);
        //renderCharts(scope.resources);
        scope.dashboardStatus = 'loaded';
   }
}

ScampDashboard.prototype.initialize = function () {
    (function initGrid() {
        var table = $("table.table-hover");

        if (table) {
            table.on('mouseover', 'td', function () {
                var idx = $(this).index();
                var rows = $(this).closest('table').find('tr');
                rows.each(function () {
                    $(this).find('td:eq(' + idx + ')').addClass('beauty-hover');
                });
            })
            .on('mouseleave', 'td', function (e) {
                var idx = $(this).index();
                var rows = $(this).closest('table').find('tr');
                rows.each(function () {
                    $(this).find('td:eq(' + idx + ')').removeClass('beauty-hover');
                });
            });
        }
    })();
}