
/* 
	TODO: Move/Remove global variables 
	
	These should be stored somewhere else or more likely removed altogether. If we really 
	want to retain a reference to an initialised jQuery datatable, then we should make sure 
	we do it for all of them consistently (perhaps something in the helper.js) other values 
	should be retrieved only when needed using val().
*/
var renewalsTable, worklistTable, submissionsTable, detailedRenewalsTable, detailedWorklistTable,
    searchTerm, searchTermDetailedRenewals, searchTermDetailedWorkFlow;

$(document).ready(function()
{
	ConsoleApp.GetUserTeamLinksHelper();

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

	$(".val-actionmenu .val-new-submission").click(function()
	{
		var type = $(this).attr("href").trimStart("#"),
		    vmType = ConsoleApp["vmSubmission" + type];

		if (vmType)
		{
			Val_AddTab($(this).text(), "/submission/_template" + type, true, function(newTab)
			{
				newTab.data("val-submission", new vmType(null, newTab.attr("id"), false));
			});
		}
		else toastr.error("Submission view model 'vmSubmission" + type + "' not found");
	});
	
	$(".val-add-workflowtasks-tab").click(function(e)
    {
		if ($(this).data("val-hastab"))
		{
            $("a[href*='#" + $(this).data("val-hastab") + "'][data-toggle='tab']").tab("show");
            return false;
        }

		var tabId = Val_AddTab("Workflow Tasks", "/WorkItem/_WorkflowTasksDetailed", true, function(newTab)
		{
			detailedWorklistTable = SetupWorkFlowDetailedDatatable();
					
            $(".val-search-workflow").on("click", function(e)
            {
            	$(".val-searching-workflow").toggleClass("hide");

            	searchTermDetailedWorkFlow = $(".val-searchterm-workflow").val();

            	detailedWorklistTable.fnDraw(true);
            });
			
            $(".val-searchterm-workflow").on("keydown", function(e)
            {
            	if (e.which == 13)
            	{
            		e.preventDefault();

            		$(".val-searching-workflow").toggleClass("hide");

            		searchTermDetailedWorkFlow = $(".val-searchterm-workflow").val();

            		detailedWorklistTable.fnDraw(true);
            	}
            });
		});

		$(this).data("val-hastab", tabId);
    });

    $(".val-add-renewal-tab").click(function(e)
    {
		if ($(this).data("val-hastab"))
		{
            $("a[href*='#" + $(this).data("val-hastab") + "'][data-toggle='tab']").tab("show");
            return false;
        }

	    var tabId = Val_AddTab("Renewals", "/Policy/_RenewalIndexDetailed", true, function(newTab)
	    {

		    detailedRenewalsTable = SetupRenewalsDetailedDatatable();

		    $(".val-search-renewals").on("click", function(e)
		    {
			    $(".val-searching-renewals").toggleClass("hide");

			    searchTermDetailedRenewals = $(".val-searchterm-renewals").val();

			    detailedRenewalsTable.fnDraw(true);
		    });

		    $(".val-searchterm-renewals").on("keydown", function(e)
		    {
			    if (e.which === 13)
			    {
				    e.preventDefault();

				    $(".val-searching-renewals").toggleClass("hide");

				    searchTermDetailedRenewals = $(".val-searchterm-renewals").val();

				    detailedRenewalsTable.fnDraw(true);
			    }
		    });
	    });

        $(this).data("val-hastab", tabId);
    });

	$(".val-searchterm").on("keydown", function(e)
	{
		if (e.which == 13)
		{
			e.preventDefault();

			$(".val-searching").toggleClass("hide");

			searchTerm = $(".val-searchterm").val();

			worklistTable.fnDraw(true);
			renewalsTable.fnDraw(true);
			submissionsTable.fnDraw(true);

			Val_RefreshSearchPanel(searchTerm, 0, 10);
		}
	});

	$(".val-search").on("click", function(e)
	{
		$(".val-searching").toggleClass("hide");

		searchTerm = $(".val-searchterm").val();

		worklistTable.fnDraw(true);
		renewalsTable.fnDraw(true);
		submissionsTable.fnDraw(true);

		Val_RefreshSearchPanel(searchTerm, 0, 10);
	});
	
	$(".val-renewals-datatable").on("click", "tr", function(event)
	{
	    SelectRow($(event.target).parents("tr"));
	});
	
	renewalsTable = $(".val-renewals-datatable").dataTable(
	{
		"sDom": "<'row-fluid'<'span12'lftrip>>",
		"fnPreDrawCallback": function(oSettings)
		{
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
		"fnRowCallback": function(nRow, aData, iIndex)
		{
			$(nRow).on("click", function(event)
			{
		        $(".val-processing").toggleClass("hide");

		        var ajaxSource = "/Policy/_RenewalPreview?PolId=" + aData.PolicyId;

		        Val_RefreshPreviewPane(ajaxSource, 'InitPV_Renewal__Preview();');
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
        	"fnRender": function(oObj)
        	{
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
			"fnRender": function(object, value)
			{
				return moment(value).format("DD MMM YYYY");
			}
		}],
		"fnServerParams": function(aoData)
		{
			var expiryDate = moment().add("days", $(".val-renewals-datatable caption input").val()).format("YYYY MM DD");

		    aoData.push({ "name": "sSearch", "value": searchTerm });
			aoData.push({ "name": "expiryEndDate", "value": expiryDate });
		},
		"aaSorting": [[3, 'asc']]
	});

    //  Refresh the renewals datatable if the user changes the expiry end date days
	$(".val-renewals-datatable caption input").change(function()
	{
        renewalsTable.fnDraw(false);
    });

	$(".val-recentquotes-datatable").on("click", "tr", function(event)
	{
        SelectRow($(event.target).parents("tr"));
	});
	
	//  Refresh the renewals datatable if policy is renewed.
	$.pubsub.subscribe('policyRenewed', function(topic, msg)
	{
		renewalsTable.fnDraw(false);
		$(".val-preview-pane").html('<div class="val-padding"><span>No preview loaded</span></div>');
		submissionsTable.fnDraw(false);
	});

	submissionsTable = $(".val-recentquotes-datatable").dataTable(
	{
		"sDom": "<'row-fluid'<'span12'lftrip>>",
		"fnPreDrawCallback": function(oSettings)
		{
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
		"fnRowCallback": function(nRow, aData, iIndex)
		{
			$(nRow).on("click", function(event)
			{
		        $(".val-processing").toggleClass("hide");

		        //SelectRow(this);
		        
		        var ajaxSource = "/Submission/_Preview?Id=" + aData.Id;
		        Val_RefreshPreviewPane(ajaxSource, 'InitPV_Submission__Preview();');
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
			"fnRender": function(oObj)
			{
				return '<div title="' + oObj.aData["InsuredName"] + '">' + oObj.aData["InsuredName"] + '</div>';
			}
		},
		{
			"mData": "BrokerPseudonym",
			"sTitle": "Broker"
		}],
		"fnServerParams": function(aoData)
		{
			aoData.push({ "name": "sSearch", "value": searchTerm });
		},
		"aaSorting": [[0, 'desc']]
	});

	$(".val-workflowtasks-datatable").on("click", "tr", function(event)
	{
	    SelectRow($(event.target).parents("tr"));
	});
	
	worklistTable = $(".val-workflowtasks-datatable").dataTable(
	{
		"sDom": "<'row-fluid'<'span12'lftrip>>",
		"fnPreDrawCallback": function(oSettings)
		{
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
		"fnRowCallback": function(nRow, aData, iIndex)
		{
			$(nRow).on("click", function(event)
			{
			    $(".val-processing").toggleClass("hide");

				var ajaxSource = "/WorkItem/_Preview?Id=" + aData.SerialNumber;

				Val_RefreshPreviewPane(ajaxSource, 'InitPV_Worklist__Preview();');
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
			"fnRender": function(oObj)
			{
			    return '<div title="' + oObj.aData["Activity"] + '">' + oObj.aData["Activity"] + '</div>';
			}
		},
		{
			"mData": "Insured",
			"sTitle": "Insured",
			"bSortable": false,
			"fnRender": function(oObj)
			{
				return '<div title="' + oObj.aData["Insured"] + '">' + oObj.aData["Insured"] + '</div>';
			}
		}],
		"aoColumnDefs": [
		{
			"aTargets": [0],
			"sType": "uk_date",
			"fnRender": function(object, value)
			{
				return moment(value).format("DD MMM YYYY");
			}
		}],
		"fnServerParams": function(aoData)
		{
			aoData.push({ "name": "sSearch", "value": searchTerm });
		}
	});

	$(".val-preview-minimize").on("click", function(e)
	{
	    HidePreview();
	});
	
	$(".val-preview-maximize").on("click", function(e)
	{
	    HidePreview();
	});

	$(".val-renewal-minimize").on("click", function(e)
	{
	    HideRenewal();
	});
	
	$(".val-renewal-maximize").on("click", function(e)
	{
	    HideRenewal();
	});

	$(".val-worklist-minimize").on("click", function(e)
	{
	    HideWorklist();
	});
	
	$(".val-worklist-maximize").on("click", function(e)
	{
	    HideWorklist();
	});
	
	$(".val-worklist-refresh").on("click", function(e)
	{
	    RefreshWorklist();
	});

	$(".val-submission-minimize").on("click", function(e)
	{
	    HideSubmission();
	});
	
	$(".val-submission-maximize").on("click", function(e)
	{
	    HideSubmission();
	});
	
	$(".val-submission-refresh").on("click", function(e)
	{
	    RefreshSubmissions();
	});

	$(".val-renewal-refresh").on("click", function (e) {
	    RefreshRenewals();
	});

	$(".val-search-minimize").on("click", function(e)
	{
	    HideSearch();
	});
	
	$(".val-search-maximize").on("click", function(e)
	{
	    HideSearch();
	});

	$(".val-minimize-all").on("click", function(e)
	{
	    HideAll(true);
	});
	
	$(".val-maximize-all").on("click", function(e)
	{
	    HideAll(false);
	});

	GetCommonDropDownData();
	LoadUserSettings();
});

function GetCommonDropDownData()
{
    if (!ConsoleApp.OfficesList)
    {
        $.ajax({
            type: 'GET',
            url: window.ValidusServicesUrl + 'office',
            dataType: 'json',            
            success: function (data)
            {
                data.unshift({ Code: "", Name: "" });
                ConsoleApp.OfficesList = data;
            }
        });
    }
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