function ScampDashboard($scope) { 
    var scope = $scope;

    var resourceUsageEscalationLevels = [
        {maxUsage: 50, cssClass: "success"},
        {maxUsage: 70, cssClass: "info"},
        {maxUsage: 85, cssClass: "warning"},
        {maxUsage: 101,cssClass: "danger"}
    ];

    var chartsToRender = [
                { fieldName: 'usageEscalation', domElement: 'usageDonutChart' },
                { fieldName: 'groupName', domElement: 'groupDonutChart' },
                { fieldName: 'resourceTypeDesc', domElement: 'typeDonutChart' },
                { fieldName: 'stateDescription', domElement: 'stateDonutChart' }
    ];

    function setResources(rspData){
        scope.resources = rspData.filter(function (item) {
            return scope.rscStateDescMapping[item.state];//Make sure the state code is valid
        }).map(function (rsc) {
            var alertClassArr = resourceUsageEscalationLevels.filter(function (el) { return rsc.remaining < el.maxUsage; });

            rsc.usageEscalation = alertClassArr && alertClassArr.length > 0 ? alertClassArr[0].cssClass : "danger";
            rsc.groupName = rsc.resourceGroup.name;
            rsc.stateDescription = scope.rscStateDescMapping[rsc.state].description;
            rsc.resourceTypeDesc = scope.resourceTypes[rsc.resourceType];

            return rsc;
        });
    }

    this.render = function (rspData) {
        scope.dashboardStatus = 'loaded';
        setResources(rspData);

        var groupBy = function (rspData, groupField, cb) {
            return _.chain(rspData).groupBy(groupField).map(cb);
        };

        var renderChart = function (groupByField, cb, chartDomEl, srcData) {
            var chartDataSrc = groupBy(srcData, groupByField, cb).value();

            Morris.Donut({
                element: chartDomEl,
                data: chartDataSrc
            });
        };

        (function renderCharts() {
            chartsToRender.forEach(function (item) {
                renderChart(item.fieldName, function (val, key) {
                    return { label: key, value: val.length }
                }, item.domElement, rspData);
            });
        })();
   }
}

ScampDashboard.prototype.initializeGrid = function () {
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
}