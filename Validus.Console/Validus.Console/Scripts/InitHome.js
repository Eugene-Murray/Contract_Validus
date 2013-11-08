var renewalsTable, worklistTable, submissionsTable, detailedRenewalsTable, detailedWorklistTable,
    searchTermDetailedRenewals, searchTermDetailedWorkFlow, searchTermDetailedSubmissions;

$(document).ready(function()
{
    ConsoleApp.GetUserTeamLinksHelper();
    LoadUserSettings();

	$(".val-actionmenu .val-new-submission").click(function()
	{
		var type = $(this).attr("href").trimStart("#"),
		    vmType = ConsoleApp["vmSubmission" + type];

		if (vmType)
		{
			Val_AddTab($(this).text(), "/submission/_template" + type, true, function(newTab)
			{
				newTab.data("val-submission", new vmType(null, newTab.attr("id"), false));

				$("abbr[title]", newTab).tooltip();
			});
		}
		else toastr.error("Submission view model 'vmSubmission" + type + "' not found");
	});

    $(".val-actionmenu .val-new-worldcheck").click(function () {
        var button = this;

        if ($(this).data("val-has-worldcheck-tab")) {
            if ($(this).data("val-has-worldcheck-tab") !== "") {
                $("a[href*='#" + $(this).data("val-has-worldcheck-tab") + "'][data-toggle='tab']").tab("show");
                return false;
            }
        }

        var tabId = Val_AddTab("WorldCheck", "/WorldCheck/_WorldCheckSearchForm", true, function (newTab) {

            var vmSearch = new vmWorldCheck(newTab.attr("id"));
            newTab.data("val-worldcheck-search", vmSearch);

        }, function () { $(button).data("val-has-worldcheck-tab", ""); });

        $(this).data("val-has-worldcheck-tab", tabId);


    });

    $(".val-add-workflowtasks-tab").click(function (e) {
        var button = this;

        if ($(this).data("val-has-workflow-tab")) {
            if ($(this).data("val-has-workflow-tab") !== "") {
                $("a[href*='#" + $(this).data("val-has-workflow-tab") + "'][data-toggle='tab']").tab("show");
                return false;
            }
        }

        var tabId = Val_AddTab("Workflow Tasks", "/WorkItem/_WorkflowTasksDetailed", true, function (newTab) {
            detailedWorklistTable = SetupWorkFlowDetailedDatatable();

            $(".val-search-workflow").on("click", function (e) {
                $(".val-searching-workflow").removeClass("hide");

                searchTermDetailedWorkFlow = $(".val-searchterm-workflow").val();

                detailedWorklistTable.fnDraw(true);
            });

            $(".val-searchterm-workflow").on("keydown", function (e) {
                if (e.which === 13) {
                    e.preventDefault();

                    $(".val-searching-workflow").removeClass("hide");

                    searchTermDetailedWorkFlow = $(".val-searchterm-workflow").val();

                    detailedWorklistTable.fnDraw(true);
                }
            });
        }, function () { $(button).data("val-has-workflow-tab", ""); });

        $(this).data("val-has-workflow-tab", tabId);
    });

    $(".val-add-renewal-tab").click(function (e) {
        var button = this;

        if ($(this).data("val-has-renewal-tab")) {
            if ($(this).data("val-has-renewal-tab") !== "") {
                $("a[href*='#" + $(this).data("val-has-renewal-tab") + "'][data-toggle='tab']").tab("show");
                return false;
            }
        }

        var tabId = Val_AddTab("Renewals", "/Policy/_RenewalIndexDetailed", true, function (newTab) {
            detailedRenewalsTable = SetupRenewalsDetailedDatatable();

            $(".val-search-renewals").on("click", function (e) {
                $(".val-searching-renewals").removeClass("hide");

                searchTermDetailedRenewals = $(".val-searchterm-renewals").val();

                detailedRenewalsTable.fnDraw(true);
            });

            $(".val-searchterm-renewals").on("keydown", function (e) {
                if (e.which === 13) {
                    e.preventDefault();

                    $(".val-searching-renewals").removeClass("hide");

                    searchTermDetailedRenewals = $(".val-searchterm-renewals").val();

                    detailedRenewalsTable.fnDraw(true);
                }
            });
        }, function () { $(button).data("val-has-renewal-tab", ""); });

        $(this).data("val-has-renewal-tab", tabId);
    });

    $(".val-add-submissions-detailed-tab").click(function (e) {
        var button = this;

        if ($(this).data("val-has-submission-tab")) {
            if ($(this).data("val-has-submission-tab") !== "") {
                $("a[href*='#" + $(this).data("val-has-submission-tab") + "'][data-toggle='tab']").tab("show");
                return false;
            }
        }

        var tabId = Val_AddTab("Submissions", "/Submission/_SubmissionIndexDetailed", true, function (newTab) {

            detailedSubmissionsTable = SetupSubmissionsDetailedDatatable();

            $(".val-search-submissions").on("click", function (e) {
                $(".val-searching-submissions").removeClass("hide");

                searchTermDetailedSubmissions = $(".val-searchterm-submissions").val();

                detailedSubmissionsTable.fnDraw(true);
            });

            $(".val-searchterm-submissions").on("keydown", function (e) {
                if (e.which === 13) {
                    e.preventDefault();

                    $(".val-searching-submissions").removeClass("hide");

                    searchTermDetailedSubmissions = $(".val-searchterm-submissions").val();

                    detailedSubmissionsTable.fnDraw(true);
                }
            });
        }, function () { $(button).data("val-has-submission-tab", ""); });

        $(this).data("val-has-submission-tab", tabId);
    });

    Val_InitialiseSearch();

    $(".val-renewals-datatable").on("click", "tr", function (event) {
        SelectRow($(event.target).parents("tr"));
    });

    renewalsTable = $(".val-renewals-datatable").dataTable(
	{
	    "sDom": "<'row-fluid'<'span12'lftrip>>",
	    "fnPreDrawCallback": function (oSettings) {
	        var oTable = $("#" + oSettings.sTableId),
				oProcessing = $("#" + oSettings.sTableId + "_processing"),
				oCaption = $("#" + oSettings.sTableId + " caption");

	        $(oProcessing).appendTo(oCaption);
	    },
	    "oLanguage":
		{
		    "sInfo": "_START_ to _END_ of _TOTAL_",
		    "sInfoEmpty": "_START_ to _END_ of _TOTAL_",
		    "sInfoFiltered": "_START_ to _END_ of _TOTAL_"
		},
	    "bLengthChange": false,
	    "bPaginate": true,
	    "bFilter": false,
	    "bProcessing": true,
	    "bServerSide": true,
	    "sAjaxSource": "/Policy/_RenewalIndexJSON",
	    "fnRowCallback": function (nRow, aData, iIndex) {
	        $(nRow).on("click", function (event) {
	            $(".val-processing").removeClass("hide");

	            var ajaxSource = "/Policy/_RenewalPreview?PolId=" + aData.PolicyId;

	            Val_RefreshPreviewPane(ajaxSource, 'InitPV_Renewal__Preview');
	        });

	        return nRow;
	    },
	    "aoColumns": [
        {
            "mData": "Underwriter",
            "sTitle": "Uwr",
            "sClass": "val-renewal-list-uwr"
        },
        {
            "mData": "InsuredName",
            "sTitle": "Insured",
            "fnRender": function (oObj) {
                return '<div title="' + oObj.aData["InsuredName"] + '">' + oObj.aData["InsuredName"] + '</div>';
            }
        },
        {
            "mData": "PolicyId",
            "sTitle": "Policy Id",
            "sClass": "val-renewal-policyid"
        },
	    {
	        "mData": "ExpiryDate",
	        "sTitle": "Expiry Date",
	        "sClass": "val-renewal-expydt"
	    }],
	    "aoColumnDefs": [
		{
		    "aTargets": [3],
		    "sType": "uk_date",
		    "fnRender": function (object, value) {
		        return moment(value).format("DD MMM YYYY");
		    }
		}],
	    "fnServerParams": function (aoData) {
	        aoData.push({ "name": "sSearch", "value": (window.ConsoleApp.SearchFiltersVM.Keywords() ? window.ConsoleApp.SearchFiltersVM.Keywords().replace(/["*]/gi, "") : "") });
	        aoData.push({ "name": "expiryEndDate", "value": window.ConsoleApp.SearchFiltersVM.RenewalLimitDate() });
	        aoData.push({ "name": "applyProfileFilters", "value": ConsoleApp.SearchFiltersVM.RenApplyProfileFilters() });

	        ko.utils.arrayForEach(ConsoleApp.SearchFiltersVM.filters(), function (filter) {
	            var filterVal = filter.Val().toString().trim();
	            if (filterVal !== null && filterVal !== undefined && filterVal !== "") {
	                var f = filter.Prop().split(":")[1] + ':' + filter.Fn() + ":" + filter.Val().toString().replace(/["*]/gi, "");
	                aoData.push({ "name": "extraFilters", "value": f });
	            }
	        });
	    },
	    "aaSorting": [[3, 'asc']]
	});

    //  Refresh the renewals datatable if the user changes the expiry end date days
    //$(".val-renewals-datatable caption input").change(function()
    //{
    //    renewalsTable.fnDraw(false);
    //});

    $(".val-recentquotes-datatable").on("click", "tr", function (event) {
        SelectRow($(event.target).parents("tr"));
    });

    //  Refresh the renewals datatable if policy is renewed.
    $.pubsub.subscribe('policyRenewed', function (topic, msg) {
        renewalsTable.fnDraw(false);
        $(".val-preview-pane").html('<div class="val-padding"><span>No preview loaded</span></div>');
        submissionsTable.fnDraw(false);
    });
    //  Refresh the submission datatable if policy is created.
    $.pubsub.subscribe('submissionCreated', function (topic, msg) {
        submissionsTable.fnDraw(false);
    });

    submissionsTable = $(".val-recentquotes-datatable").dataTable(
	{
	    "sDom": "<'row-fluid'<'span12'lftrip>>",
	    "fnPreDrawCallback": function (oSettings) {
	        var oTable = $("#" + oSettings.sTableId),
				oProcessing = $("#" + oSettings.sTableId + "_processing"),
				oCaption = $("#" + oSettings.sTableId + " caption");

	        $(oProcessing).appendTo(oCaption);
	    },
	    "oLanguage":
		{
		    "sInfo": "_START_ to _END_ of _TOTAL_",
		    "sInfoEmpty": "_START_ to _END_ of _TOTAL_",
		    "sInfoFiltered": "_START_ to _END_ of _TOTAL_"
		},
	    "bLengthChange": false,
	    "bPaginate": true,
	    "bFilter": false,
	    "bProcessing": true,
	    "bServerSide": true,
	    "sAjaxSource": "/Submission/_IndexJSON",
	    "fnRowCallback": function (nRow, aData, iIndex) {
	        $(nRow).on("click", function (event) {
	            $(".val-processing").removeClass("hide");
	            var ajaxSource = "/Submission/_Preview?Id=" + aData.Id;
	            Val_RefreshPreviewPane(ajaxSource, 'InitPV_Submission__Preview');
	        });

	        return nRow;
	    },
	    "aoColumns": [
		{
		    "mData": "Id",
		    "bVisible": false
		},
		    {
		        "mData": "InsuredName",
		        "sTitle": "Insured",
		        "fnRender": function (oObj) {
		            return '<div title="' + oObj.aData["InsuredName"] + '">' + oObj.aData["InsuredName"] + '</div>';
		        }
		    },
		{
		    "mData": "BrokerPseudonym",
		    "sTitle": "Broker"
		},
		{
		    "mData": "InceptionDate",
		    "sTitle": "Inception Date",
		    "sClass": "val-renewal-expydt",
		    "mRender": function (data, type, full) {
		        return SetRecentSubmissionInceptionDate(data);
		    }
		}],
	    "aoColumnDefs": [
		{
		    "aTargets": [3],
		    "sType": "uk_date",
		    "fnRender": function (object, value) {
		        return moment(value).format("DD MMM YYYY");
		    }
		}],
	    "fnServerParams": function (aoData) {
	        aoData.push({ "name": "sSearch", "value": (window.ConsoleApp.SearchFiltersVM.Keywords() ? window.ConsoleApp.SearchFiltersVM.Keywords().replace(/["*]/gi, "") : "") });
	        aoData.push({ "name": "applyProfileFilters", "value": ConsoleApp.SearchFiltersVM.SubApplyProfileFilters() });

	        ko.utils.arrayForEach(ConsoleApp.SearchFiltersVM.filters(), function (filter) {
	            var filterVal = filter.Val().toString().trim();
	            if (filterVal !== null && filterVal !== undefined && filterVal !== "") {
	                var f = filter.Prop().split(":")[1] + ':' + filter.Fn() + ":" + filter.Val().toString().replace(/["*]/gi, "");
	                aoData.push({ "name": "extraFilters", "value": f });
	            }
	        });

	    },
	    "aaSorting": [[0, 'desc']]
	});

    $(".val-workflowtasks-datatable").on("click", "tr", function (event) {
        SelectRow($(event.target).parents("tr"));
    });

    worklistTable = $(".val-workflowtasks-datatable").dataTable(
	{
	    "sDom": "<'row-fluid'<'span12'lftrip>>",
	    "fnPreDrawCallback": function (oSettings) {
	        var oTable = $("#" + oSettings.sTableId),
				oProcessing = $("#" + oSettings.sTableId + "_processing"),
				oCaption = $("#" + oSettings.sTableId + " caption");

	        $(oProcessing).appendTo(oCaption);
	    },
	    "oLanguage":
		{
		    "sInfo": "_START_ to _END_ of _TOTAL_",
		    "sInfoEmpty": "_START_ to _END_ of _TOTAL_",
		    "sInfoFiltered": "_START_ to _END_ of _TOTAL_"
		},
	    "bLengthChange": false,
	    "bPaginate": true,
	    "bFilter": false,
	    "bProcessing": true,
	    "bServerSide": true,
	    "sAjaxSource": "/WorkItem/GetUserWorkflowItems",
	    "fnRowCallback": function (nRow, aData, iIndex) {
	        $(nRow).on("click", function (event) {
	            $(".val-processing").removeClass("hide");

	            Val_RefreshPreviewPane("/WorkItem/_Preview?Id=" + aData.SerialNumber, 'InitPV_Worklist__Preview');
	        });

	        return nRow;
	    },
	    "aoColumns": [
		{
		    "mData": "StartDate",
		    "sTitle": "Start Date",
		    "sClass": "val-worklist-startdate"
		},
		{
		    "mData": "PolicyID",
		    "sTitle": "Policy Id",
		    "bSortable": false,
		    "sClass": "val-worklist-policyid"
		},
		{
		    "mData": "Activity",
		    "sTitle": "Activity",
		    "sClass": "val-worklist-activity",
		    "fnRender": function (oObj) {
		        return '<div title="' + oObj.aData["Activity"] + '">' + oObj.aData["Activity"] + '</div>';
		    }
		},
		{
		    "mData": "Insured",
		    "sTitle": "Insured",
		    "bSortable": false,
		    "fnRender": function (oObj) {
		        return '<div title="' + oObj.aData["Insured"] + '">' + oObj.aData["Insured"] + '</div>';
		    }
		}],
	    "aoColumnDefs": [
		{
		    "aTargets": [0],
		    "sType": "uk_date",
		    "fnRender": function (object, value) {
		        return moment(value).format("DD MMM YYYY");
		    }
		}],
	    "fnServerParams": function (aoData) {
	        aoData.push({ "name": "sSearch", "value": (window.ConsoleApp.SearchFiltersVM.Keywords() ? window.ConsoleApp.SearchFiltersVM.Keywords().replace(/["*]/gi, "") : "") });
	        //aoData.push({ "name": "applyProfileFilters", "value": ConsoleApp.SearchFiltersVM.RenApplyProfileFilters() });

	        ko.utils.arrayForEach(ConsoleApp.SearchFiltersVM.filters(), function (filter) {
	            var filterVal = filter.Val().toString().trim();
	            if (filterVal !== null && filterVal !== undefined && filterVal !== "") {
	                var f = filter.Prop().split(":")[1] + ':' + filter.Fn() + ":" + filter.Val().toString().replace(/["]/gi, "");
	                aoData.push({ "name": "extraFilters", "value": f });
	            }
	        });

	        var activityFilter = $("#wfActivityFilter").val();
	        aoData.push({ "name": "extraFilters", "value": "Activity:CONTAINS:" + activityFilter });
	    }
	});

    $(".val-minimize-all").on("click", function (e) {
        HideAll(true);
        ConsoleApp.SearchFiltersVM.WFMin(true);
        ConsoleApp.SearchFiltersVM.RenMin(true);
        ConsoleApp.SearchFiltersVM.SubMin(true);
        ConsoleApp.SearchFiltersVM.ESMin(true);
        ConsoleApp.SearchFiltersVM.PrevMin(true);
    });

    $(".val-maximize-all").on("click", function (e) {
        HideAll(false);
        ConsoleApp.SearchFiltersVM.WFMin(false);
        ConsoleApp.SearchFiltersVM.RenMin(false);
        ConsoleApp.SearchFiltersVM.SubMin(false);
        ConsoleApp.SearchFiltersVM.ESMin(false);
        ConsoleApp.SearchFiltersVM.PrevMin(false);
    });
});