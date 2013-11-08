function Category(data) {
    this.Name = ko.observable(data);
}

function Country(data) {
    this.Name = ko.observable(data);
}

function vmWorldCheck(domId)
{
    var self = this;
    self.DomId = domId;
    self.IsSearching = ko.observable(false);
    self.IsInitialised = ko.observable(false);
    self.countries = ko.observableArray([]);
    self.categories = ko.observableArray([]);
    self.DirtyFlag = null;

    self.Model = {
        Name: ko.observable(""),
        Keywords: ko.observable(""),
        Country: ko.observable(""),
        Category: ko.observable(""),
        Uid: ko.observable("")
    };

    self.Initialise = function () {
        if (!domId) {
            toastr.error("No DOM Id specified, cannot initialise User view model");

            self.Cancel();
        }
        else {

            //self.DirtyFlag = new ko.DirtyFlag(self);

            self.BindKO();

            setTimeout(self.InitialiseDropdowns, 100);

        }
    };

    self.BindKO = function () {
        ko.applyBindings(self, document.getElementById(domId));
    };

    self.InitialiseDropdowns = function () {
        $.ajax(
		{
		    url: "/WorldCheck/_WorldCheckSearchFormData",
		    type: "GET",
		    dataType: "json",
		    contentType: "application/json",
		    success: function (data, status, xhr) {

		        self.IsSearching(false);

		        if (data.Countries) {
		            var _countries = $.map(data.Countries, function (item) { return new Country(item); });
		            self.countries(_countries);
		        }
		        if (data.Categories) {
		            var _categories = $.map(data.Categories, function (item) { return new Category(item); });
		            self.categories(_categories);
		        }
		        if (data.Criteria) {
		            self.syncJSON(data.Criteria);
		            self.InitialiseForm();
		        }
		    }
		});
    };

    self.InitialiseForm = function () {
        self.InitialisePane();
        //self.DirtyFlag.Reset();
        self.IsInitialised(true);
    };

    self.InitialisePane = function (element) {
        var tableConfig = new BaseDataTable(null,
            "<p>Please contact Compliance for any queries.</p>");

        tableConfig.aaSorting = [[0, "desc"]];
        tableConfig.aoColumnDefs = [{ "bSortable": false, "aTargets": [1] }];

        $(".val-worldcheck-searchresults-datatable", self.Form).dataTable(tableConfig);
        element = (!element) ? (!self.Form) ? document.body : self.Form : element;
        self.InitialiseDatePickers(element);
    };
    
    self.InitialiseDatePickers = function (element) {
        $("input[type='text'].datepicker:not(.ui-datepicker)", element).each(function (index, item) {
            $(item).datepicker(
			{
			    // TODO: Restrict selectable dates ?, i.e. minDate: -20, maxDate: "+1M +10D"
			    dateFormat: "yy-mm-dd",
			    numberOfMonths: 1,
			    showOtherMonths: true,
			    selectOtherMonths: true,
			    changeMonth: true,
			    changeYear: true
			});

            $(item).siblings("button.datepicker").click(function (e) {
                e.preventDefault();

                $(item).focus();
            });
        });
    };

    self.Search = function (element, e, callback) {
        self.IsSearching(true);
        var modelJson = self.toJS();
        //var modelJason = JSON.parse();
        toastr.info("Searching...");

        $.ajax({
            url: "/WorldCheck/_Search",
            type: "GET",
            data: modelJson,
            dataType: "html",
            success: function (data) {

                var tableConfig = new BaseDataTable(20,
		    		"<p>Please contact Compliance for any queries.</p>");

                tableConfig.aaSorting = [[0, "desc"]];
                tableConfig.aoColumnDefs = [{ "bSortable": false, "aTargets": [4] }];

                $(".val-worldcheck-searchresults-matches", self.Form).html(data);
                $(".val-worldcheck-searchresults-datatable", self.Form).dataTable(tableConfig);
            }
        });
        self.IsSearching(false);

    };
    
    self.Initialise();

}

vmWorldCheck.prototype.syncJSON = function (searchData) {
    var vmSearch = this.Model;

    vmSearch.Name(searchData.Name);
    vmSearch.Keywords(searchData.Keywords);
    vmSearch.Country(searchData.Country);
    vmSearch.Category(searchData.Category);
    vmSearch.Uid(searchData.Uid);

    return vmSearch;

};

vmWorldCheck.prototype.toJS = function () {
    var vmSearch = ko.toJS(this.Model);

    delete vmSearch.koTrim;
    delete vmSearch.syncJSON;
    delete vmSearch.toJSON;

    return vmSearch;
};

vmWorldCheck.prototype.toJSON = function () {
    var vmSearch = this.toJS();
    return ko.toJSON(vmSearch);
};