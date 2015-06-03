/*
 This wrapper module is intended(but not limited to) being used as a typeahead front-end control sourced against a restful service
 Reference the API documentation for follow-on questions https://github.com/twitter/typeahead.js/blob/master/doc/jquery_typeahead.md#jquerytypeaheadoptions-datasets
 usage: 
        <div id="remote">
            <input class="typeahead" type="text" placeholder="Search for SCAMP users">
        </div>

 required 
    var myConfig = {
        componentId: 'remote',
        minLength: 3, //The minimum character length needed before suggestions start getting rendered. Defaults to 1
        scopeValueBindedPropertyOnSelection: 'userId',
        remote : {
             url : 'myremoteUrl.com/users/%QUERY/search',
             queryStr: '%QUERY',
             displayProperty: 'value'//This is the property referenced from the response to determine what display on the control
             
        }
    }

 valueProperty = 'id'; //The property name for the unique id of the selected option from the RPC
 var selItemCB = function (e, datum) { $(hiddenInput).val() = datum[valueProperty]; }//The CB referenced for each instance an item is selected from the typeahead. 
 var myTypeaheadControl = new Typeahead($scope, myConfig, selItemCB);

*/



function Typeahead($scope, config, itemSelectionCB) {
    this.scope = $scope;
    this.configuration = config;
    var remoteURL = this.configuration.remote.url;
    var queryStr = this.configuration.remote.queryStr;
    var displayProperty = this.configuration.remote.displayProperty;
    var componentId = this.configuration.componentId;
    var minLength = this.configuration.minLength;

    if (!remoteURL || !displayProperty || !componentId || !minLength || !queryStr)
        throw new Error('You have not provided all the required fields into the typeahead control');

    var remoteCall = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.whitespace,
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //prefetch: '../data/films/post_1960.json',
        remote: {
            url: remoteURL,
            wildcard: queryStr,
            transform: function (rsp) {
                console.log(rsp);
            }
        }
    });

    $(document).ready(function () {
        $('#' + componentId + ' .typeahead').typeahead({
            minLength: minLength,
            hint: true,
            highlight: true
        }, {
            display: displayProperty,
            source: remoteCall,
            name: 'users'
        });

        $('#' + componentId).on("typeahead:selected typeahead:autocompleted", itemSelectionCB);
    });


}