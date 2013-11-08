
/* 
	TODO: Move/Remove global variables 
	
	These should be stored somewhere else or more likely removed altogether. If we really 
	want to retain a reference to an initialised jQuery datatable, then we should make sure 
	we do it for all of them consistently (perhaps something in the helper.js) other values 
	should be retrieved only when needed using val().
*/


$(document).ready(function()
{
	// TODO: InitPV_About__AboutModal
	$(".val-about-modal").modal("hide");
	$(".val-about-modal").on("show", function()
	{
		var aboutBody = $(".val-about-modal .modal-body [class^='span']");
		
		if (aboutBody.children(".val-loading"))
			aboutBody.load("/About/_AboutModal");
	});
	$(".val-accelerator-modal").modal("hide");
	$(".val-accelerator-modal").on('show', function () {
	    var acceleratorBody = $(".val-accelerator-modal .modal-body [class^='span']");
	    acceleratorBody.empty();
	    acceleratorBody.load("/Admin/AcceleratorIndex");
	});
	$(".val-console-menu a[href='#About']").click(function(e)
	{
		$(".val-about-modal").modal("toggle");

		e.preventDefault();
	});

	Val_InitTabs();
});

//function getCookie(name) {
//    var parts = document.cookie.split(name + "=");
//    if (parts.length == 2) return parts.pop().split(";").shift();
//}

function Val_InitialiseSearch()
{
    var a = $("#advancedFilters");
    a.collapse({ toggle: false });
    $("#searchArea").css("background-color", "rgb(240,240,240)");

    $("#advancedFiltersToggle").click(function ()
    {
        a.collapse("toggle");
    });

    a.collapse().on('shown', function () {
        window.ConsoleApp.SearchFiltersVM.ExpandButtonChevron("up");
        $("#searchArea").css("background-color", "rgb(217, 217, 217)");
    });

    a.collapse().on('hidden', function () {
        window.ConsoleApp.SearchFiltersVM.ExpandButtonChevron("down");
        $("#searchArea").css("background-color", "rgb(240,240,240)");
    });
    
	$(".val-search .val-searchterm").keydown(function(e)
	{
		if (e.which === 13)
		{
            e.preventDefault();
            $(".val-search .val-searchbutton").trigger("click");
		}
    });

	$('.val-renewals-datatable .dropdown-menu, .val-workflowtasks-datatable .dropdown-menu').click(function (event) {
	    event.stopPropagation();
	});

    //Filters stuff
    var filtersDiv = $("#searchArea").get(0);
	var homeTab = $("#tab1-home").get(0);
    
    var filtersVM = function() {
        var self = this;
        self.filters = ko.observableArray();

        self.Save = function () {
            var l = self.SavedSearches.length;
            self.SavedSearches.push({ Name: "Saved search " + (l + 1)  });
        };

        self.SavedSearches = ko.observableArray();
        self.RenewalLimitDays = ko.observable(30);
        self.RenewalLimitDate = ko.computed(function () {
            return moment().add("days", self.RenewalLimitDays()).format("YYYY MM DD");
        });

        self.ApplyProfileFilters = ko.observable(true);
        self.WFApplyProfileFilters = ko.observable(true);
        self.ESApplyProfileFilters = ko.observable(false);
        self.RenApplyProfileFilters = ko.observable(true);
        self.SubApplyProfileFilters = ko.observable(true);

        self.WFMin = ko.observable(false);
        self.ESMin = ko.observable(false);
        self.RenMin = ko.observable(false);
        self.SubMin = ko.observable(false);
        self.PrevMin = ko.observable(false);

        self.Keywords = ko.observable("");
        self.addFilter = function () {

            var added = false;
            for (var i = 0; i < self.PropertyOptions.length ; i++)
	{
                var alreadyThere = false;
                ko.utils.arrayForEach(self.filters(), function (filter)
	{
                    var propInfilter = filter.Prop();
                    if (self.PropertyOptions[i].val == propInfilter)
                        alreadyThere = true;
	});

                if (!alreadyThere) {
                    var obs = new self.Filter(self.PropertyOptions[i].val, "CONTAINS", "");
                    self.filters.push(obs);
                    added = true;
                    return;
                }
            }

            if (!added)
		{
                var obs = new self.Filter(self.PropertyOptions[0].val, "CONTAINS", "");
                self.filters.push(obs);
		}
        };

        self.Filter = function (p, f, v) {
            s = this;
            this.Prop = ko.observable(p);
            this.Fn = ko.observable(f);
            this.Val = ko.observable(v)
            this.Fns = ko.computed(function () {
                if (s.Prop() !== "" && s.Prop() !== undefined) {
                    for (var i = 0; i < window.ConsoleApp.SearchFiltersVM.PropertyOptions.length; i++) {
                        if (window.ConsoleApp.SearchFiltersVM.PropertyOptions[i].val === s.Prop())
                            return window.ConsoleApp.SearchFiltersVM.PropertyOptions[i].fns;
                    }
                }
            });
        };

        self.removeFilter = function () {
            self.filters.remove(this);
}

        self.FilterQuery = ko.computed(function () {
            var pieces = [];
            ko.utils.arrayForEach(self.filters(), function (filter) {
                var filterVal = filter.Val().toString().trim();
                if (filterVal !== null && filterVal !== undefined && filterVal !== "")
                {
                    switch (filter.Fn())
{
                        case "CONTAINS":
                            pieces.push(filter.Prop().split(":")[0] + ":" + filter.Val());
                            break;
                        case "DOESNOTCONTAIN":
                            pieces.push("-" + filter.Prop().split(":")[0] + ":" + filter.Val());
                            break;
                        case "EQUALS":
                            pieces.push(filter.Prop().split(":")[0] + "=" + filter.Val());
                            break;
                        case "DOESNOTEQUAL":
                            pieces.push(filter.Prop().split(":")[0] + "<>" + filter.Val());
                            break;
                    }
                }
            });
            return pieces.length === 0 ? "" : "(" + pieces.join(" AND ") + ")";
        });

        self.UserFilters = ko.observable();

        self.ProfileFilterQuery = ko.computed(function myfunction() {
            if (self.UserFilters() !== undefined) {
                var profileFilterQuery = ""
                var filterCOBs = self.UserFilters().FilterCOBs
                var filterCOBsQuery = []
                for (var i = 0; i < filterCOBs.length ; i++) {
                    filterCOBsQuery.push("COB:" + filterCOBs[i])
                }
                var filterOffices = self.UserFilters().FilterOffices
                var filterOfficesQuery = []
                for (var i = 0; i < filterOffices.length ; i++) {
                    filterOfficesQuery.push("OrigOff:" + filterOffices[i])
                }
                var filterMembers = self.UserFilters().FilterMembers
                var filterMembersQuery = []
                for (var i = 0; i < filterMembers.length ; i++) {
                    filterMembersQuery.push("UwrPsu:" + filterMembers[i])
                }
                var additionalCOBs = self.UserFilters().AdditionalCOBs
                var additionalCOBsQuery = []
                for (var i = 0; i < additionalCOBs.length ; i++) {
                    additionalCOBsQuery.push("COB:" + additionalCOBs[i])
                }
                var additionalOffices = self.UserFilters().AdditionalOffices
                var additionalOfficesQuery = []
                for (var i = 0; i < additionalOffices.length ; i++) {
                    additionalOfficesQuery.push("OrigOff:" + additionalOffices[i])
                }
                var additionalMembers = self.UserFilters().AdditionalMembers
                var additionalMembersQuery = []
                for (var i = 0; i < additionalMembers.length ; i++) {
                    additionalMembersQuery.push("UwrPsu:" + additionalMembers[i])
                }

                var partsToAnd = [];
                if (filterCOBsQuery.length > 0)
                    partsToAnd.push("(" + filterCOBsQuery.join(" OR ") + ")");
                if (filterOfficesQuery.length > 0)
                    partsToAnd.push("(" + filterOfficesQuery.join(" OR ") + ")");
                if (filterMembersQuery.length > 0)
                    partsToAnd.push("(" + filterMembersQuery.join(" OR ") + ")");

                profileFilterQuery = "(" + (partsToAnd.length > 0 ? "(" + partsToAnd.join(" AND ") + ")" : "")
                                    + (additionalCOBsQuery.length > 0 ? " OR (" + additionalCOBsQuery.join(" OR ") + ")" : "")
                                    + (additionalOfficesQuery.length > 0 ? " OR (" + additionalOfficesQuery.join(" OR ") + ")" : "")
                                    + (additionalMembersQuery.length > 0 ? " OR (" + additionalMembersQuery.join(" OR ") + ")" : "")
                                    + ")";
                return profileFilterQuery;
            }

            return "";
            });

        self.ProfileFilterSummary = ko.computed(function myfunction() {
            if (self.UserFilters() !== undefined) {
                var profileFilterQuery = ""
                var filterCOBs = self.UserFilters().FilterCOBs
                var filterCOBsQuery = []
                for (var i = 0; i < filterCOBs.length ; i++) {
                    filterCOBsQuery.push("COB:" + filterCOBs[i])
                }
                var filterOffices = self.UserFilters().FilterOffices
                var filterOfficesQuery = []
                for (var i = 0; i < filterOffices.length ; i++) {
                    filterOfficesQuery.push("OrigOff:" + filterOffices[i])
                }
                var filterMembers = self.UserFilters().FilterMembers
                var filterMembersQuery = []
                for (var i = 0; i < filterMembers.length ; i++) {
                    filterMembersQuery.push("UwrPsu:" + filterMembers[i])
                }
                var additionalCOBs = self.UserFilters().AdditionalCOBs
                var additionalCOBsQuery = []
                for (var i = 0; i < additionalCOBs.length ; i++) {
                    additionalCOBsQuery.push("COB:" + additionalCOBs[i])
                }
                var additionalOffices = self.UserFilters().AdditionalOffices
                var additionalOfficesQuery = []
                for (var i = 0; i < additionalOffices.length ; i++) {
                    additionalOfficesQuery.push("OrigOff:" + additionalOffices[i])
                }
                var additionalMembers = self.UserFilters().AdditionalMembers
                var additionalMembersQuery = []
                for (var i = 0; i < additionalMembers.length ; i++) {
                    additionalMembersQuery.push("UwrPsu:" + additionalMembers[i])
                }

                var partsToAnd = [];
                if (filterCOBsQuery.length > 0)
                    partsToAnd.push("(" + filterCOBsQuery.join(" OR ") + ")");
                if (filterOfficesQuery.length > 0)
                    partsToAnd.push("(" + filterOfficesQuery.join(" OR ") + ")");
                if (filterMembersQuery.length > 0)
                    partsToAnd.push("(" + filterMembersQuery.join(" OR ") + ")");

                profileFilterQuery = "Check the box to apply your profile filters:\n\n"
                                    + "Team-related filter:\n\n(\n" + partsToAnd.join(" \nAND \n") + "\n)\n\nAdditionals:\n"
                                    + (additionalCOBsQuery.length > 0 ? "\nOR (" + additionalCOBsQuery.join(" OR ") + ")" : "")
                                    + (additionalOfficesQuery.length > 0 ? "\nOR (" + additionalOfficesQuery.join(" OR ") + ")" : "")
                                    + (additionalMembersQuery.length > 0 ? "\nOR (" + additionalMembersQuery.join(" OR ") + ")" : "");                                    
                return profileFilterQuery;
            }

            return "";
        });

        self.PropertyOptions = [{ val: "VALCob:COBId", name: "COB", fns: [{ val: "EQUALS", label: "Equals" }] },
                                { val: "VALAcctgYr:AccountYear", name: "Acc Yr", fns: [{ val: "EQUALS", label: "Equals" }] },
                                { val: "VALInsdNm:InsuredName", name: "Insured Name", fns: [{ val: "CONTAINS", label: "Contains" }] },
                                { val: "VALDescription:Description", name: "Description", fns: [{ val: "CONTAINS", label: "Contains" }] },
                                { val: "VALBkrPsu:BrokerPseudonym", name: "Broker Pseudonym", fns: [{ val: "CONTAINS", label: "Contains" }] },
                                { val: "VALUwrPsu:UnderwriterCode", name: "Underwriter Code", fns: [{ val: "EQUALS", label: "Equals" }] }];

        self.Query = function () {
            alert(self.FilterQuery());
        }

        self.ClearFilters = function () {
            self.filters.removeAll();
        };

        self.NonEmptyFilterCount = ko.computed(function () {
            var total = 0;
            ko.utils.arrayForEach(self.filters(), function (filter) {
                var valInFilter = filter.Val().toString().trim();
                if (valInFilter !== null && valInFilter !== undefined && valInFilter !== "")
		{
                    total++;
                }
            });
            return total;
        });

        self.ExpandButtonChevron = ko.observable("down");

        self.ButtonHtml = ko.computed(function () {
            return '<span>Advanced Filters (' + self.NonEmptyFilterCount() + ') </span><i class="icon-chevron-' + self.ExpandButtonChevron() + '"></i>';
		});

        self.HasNonWFCompatFilters = ko.computed(function () {
            ko.utils.arrayForEach(self.filters(), function (filter) {
                var valInFilter = filter.Val().toString().trim();
                var propInfilter = filter.Prop();
                if (valInFilter !== null && valInFilter !== undefined && valInFilter !== "" && (propInfilter == "VALDescription:Description" || propInfilter == "VALDescription:Description")) {
                    return true;
}
            });
        });
    };

    window.ConsoleApp.SearchFiltersVM = new filtersVM();

    $.ajax({
        type: 'GET',
        url: '/user/mysettings',
        dataType: 'json',
        success: function (data) {
            window.ConsoleApp.SearchFiltersVM.UserFilters(data);
        }
    });

    var obsCOB = new window.ConsoleApp.SearchFiltersVM.Filter(window.ConsoleApp.SearchFiltersVM.PropertyOptions[0].val, window.ConsoleApp.SearchFiltersVM.PropertyOptions[0].fns[0].val, "");
    var obsYear = new window.ConsoleApp.SearchFiltersVM.Filter(window.ConsoleApp.SearchFiltersVM.PropertyOptions[1].val, window.ConsoleApp.SearchFiltersVM.PropertyOptions[1].fns[0].val, "");
    var obsInsd = new window.ConsoleApp.SearchFiltersVM.Filter(window.ConsoleApp.SearchFiltersVM.PropertyOptions[2].val, window.ConsoleApp.SearchFiltersVM.PropertyOptions[2].fns[0].val, "");

    window.ConsoleApp.SearchFiltersVM.filters.push(obsCOB);
    window.ConsoleApp.SearchFiltersVM.filters.push(obsYear);
    window.ConsoleApp.SearchFiltersVM.filters.push(obsInsd);
    
    if (homeTab)
        ko.applyBindings(window.ConsoleApp.SearchFiltersVM, homeTab);
    
	$("#searchArea .val-searchbutton").click(function(e)
	{
	    e.preventDefault();
	    //var searchBox = $("#searchArea .val-searchterm");
	    //searchTerm = searchBox.val();
	    if (window.ConsoleApp.SearchFiltersVM.Keywords() !== undefined) {
	        var trimmedTerm = window.ConsoleApp.SearchFiltersVM.Keywords().replace(/\b([A-Z]{2})\*(\d{2})\b/gi, "").trim();

	        if (/\b([A-Z]{2})\*(\d{2})\b/gi.test(window.ConsoleApp.SearchFiltersVM.Keywords())) {
	            var reference = RegExp.$1.toUpperCase(),
                    year = moment(RegExp.$2, "YY").year();

	            var gotCOB = false;
	            var gotYear = false;
	            ko.utils.arrayForEach(window.ConsoleApp.SearchFiltersVM.filters(), function (filter) {
	                var propInFilter = filter.Prop().toString().trim();
	                if (propInFilter === window.ConsoleApp.SearchFiltersVM.PropertyOptions[0].val) {
	                    filter.Val(reference);
	                    gotCOB = true;
	                }
	                else if (propInFilter === window.ConsoleApp.SearchFiltersVM.PropertyOptions[1].val) {
	                    filter.Val(year);
	                    gotYear = true;
	                }
	            });

	            if (!gotCOB) {
	                var obsCOB = new window.ConsoleApp.SearchFiltersVM.Filter(window.ConsoleApp.SearchFiltersVM.PropertyOptions[0].val, window.ConsoleApp.SearchFiltersVM.PropertyOptions[0].fns[0].val, reference);
	                //var obsCOB = { Prop: ko.observable("VALCob:COBId"), Fn: ko.observable("EQUALS"), Val: ko.observable(reference) };
	                window.ConsoleApp.SearchFiltersVM.filters.push(obsCOB);
	            }

	            if (!gotYear) {
	                var obsYear = new window.ConsoleApp.SearchFiltersVM.Filter(window.ConsoleApp.SearchFiltersVM.PropertyOptions[1].val, window.ConsoleApp.SearchFiltersVM.PropertyOptions[1].fns[0].val, year);
	                //var obsYear = { Prop: ko.observable("VALAcctgYr:AccountYear"), Fn: ko.observable("EQUALS"), Val: ko.observable(year) };
	                window.ConsoleApp.SearchFiltersVM.filters.push(obsYear);
	            }

	            window.ConsoleApp.SearchFiltersVM.Keywords(trimmedTerm);
	            //searchBox.val(trimmedTerm);
	            //searchTerm = trimmedTerm;

	            $("#advancedFilters").collapse("show");
	            
	            $("#advancedFiltersTable").popover(
	                {
	                    animation: true,
	                    html: true,
	                    container: 'body',
	                    trigger: "manual",
	                    placement: "right",
	                    title: "Wildcards",
	                    //content: '<div id="advPop"><p>Wildcards (*) are only allowed at the end of a search word.</p><p>Your query has been translated into an advanced search.</p><button class="btn btn-primary">Okay</button><br /><input type="checkbox">Don\'t show this again</input></div>'
	                    content: '<div id="advPop"><p>Wildcards (*) are only allowed at the end of a search word.</p><p>Your query has been translated into an advanced search.</p><button class="btn btn-primary">Okay</button></div>'
	                }).popover('show');

	            setTimeout(function () {	                
	                $("#advancedFiltersTable").popover("hide");
	            }, 15000);

	            var a = $("#advPop .btn-primary");
	            a.click(function () {
	                $("#advancedFiltersTable").popover("hide");
	            });	            
	        }
	    }

		renewalsTable.fnDraw(true);
		submissionsTable.fnDraw(true);
		worklistTable.fnDraw(true);
		refiners = [];
		contentSources = [];
		Val_Search();		
	});
}

function LoadUserSettings()
{
    //  TODO: Get the entire user object instead. Can't easily at present because of lazy loading.
    //  Would have to convert to DTO.
    if (!ConsoleApp.UserSubmissionTypes)
    {
        $.ajax({
            type: 'GET',
            url: '/user/settings',
            dataType: 'json',
            success: function (data)
            {
                ConsoleApp.UserSubmissionTypes = data;
            }
        });
    }
}