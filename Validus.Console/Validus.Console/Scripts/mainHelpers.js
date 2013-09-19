
function GetRenewalButtonMarkup(data)
	{
    if (ConsoleApp.UserSubmissionTypes != null)
		{
	    if (ConsoleApp.UserSubmissionTypes.length == 1)
		{
				return "<div class='row-fluid'>" +
					"<div class='span12'>" +
                "<button class='btn btn-mini val-renewal-renew' title='Create submission as renewal' data-renewalpolicyid='" + data + "' ' data-submissionTypeId='" + ConsoleApp.UserSubmissionTypes[0] + "'>Renew as " + ConsoleApp.UserSubmissionTypes[0] + "</button>" +
					"</div>" +
					"</div>";
			}
	    else if (ConsoleApp.UserSubmissionTypes.length > 1)
		{
			var button = "<div class='btn-group' style='overflow:visible;height:30px'>" +
				"<button class='btn btn-mini' type='button'>Renew as...</button>" +
				"<button class='btn btn-mini dropdown-toggle' type='button' data-toggle='dropdown'>" +
				"<span class='caret'></span>" +
				"</button>" +
				"<ul class='dropdown-menu'>";

			$.each(ConsoleApp.UserSubmissionTypes, function (index, teamSubmissionType)
			{
					button += "<li><a class='val-renewal-renew'" +
                        " data-renewalpolicyid='" + data + "'" +
                        " data-submissionTypeId='" + teamSubmissionType + "'>" + teamSubmissionType + " Submission</a></li>";
			});
			button += "</ul></div>";
			return button;
		}
		else
		{
			return "-";
		}
	}
}

function SetupRenewalsDetailedDatatable()
{
	return $(".val-renewals-detailed-datatable").dataTable(
	        {
	        	"sDom": "<'row-fluid'<'span12'lftrip>>",
	        	"fnPreDrawCallback": function(oSettings)
	        	{
	        		var oTable = $("#" + oSettings.sTableId),
				        oProcessing = $("#" + oSettings.sTableId + "_processing"),
				        oCaption = $("#" + oSettings.sTableId + " caption");

	        		$(oProcessing).appendTo(oCaption);
	        	},
	        	"fnDrawCallback": function(oSettings)
	        	{
	        		$(oSettings.nTable).find("button.val-renewal-renew, a.val-renewal-renew").click(function()
	        		{
	        			OpenRenewalTab($(this).data("renewalpolicyid"), $(this).data("submissiontypeid"));
	        		});

	        		$.pubsub.subscribe('policyRenewed', function(topic, msg)
	        		{
	        			detailedRenewalsTable.fnDraw(false);
	        		});
	        	}
                ,
	        	"oLanguage":
		        {
		        	"sInfo": "_START_ to _END_ of _TOTAL_",
		        	"sInfoEmpty": "_START_ to _END_ of _TOTAL_",
		        	"sInfoFiltered": "_START_ to _END_ of _TOTAL_"
		        },
	        	"bLengthChange": false,
	        	"bPaginate": true,
	        	"bFilter": false,
	        	"iDisplayLength": 30,
	        	"bProcessing": true,
	        	"bServerSide": true,
	        	"sAjaxSource": "/Policy/_RenewalIndexJSONDetailed",
	        	"aoColumns": [
                {
                	"mData": "Underwriter",
                	"sTitle": "Uwr",
                	"sClass": "val-renewal-uwr"
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
                	"sClass": "val-renewal-policyid",
                	"mRender": function(data, type, full) { return '<span><a class="val-link" href="javascript:OpenWebPolicy(\'' + WebPolicyUrl + data + '\')">' + data + '</a></span>'; }
                },
	            {
	            	"mData": "InceptionDate",
	            	"sTitle": "IncpDt",
	            	"sClass": "val-renewal-incpdt"
	            },
	            {
	            	"mData": "ExpiryDate",
	            	"sTitle": "ExpyDt",
	            	"sClass": "val-renewal-expydt"
	            },
	            {
	            	"mData": "COB",
	            	"sTitle": "COB",
	            	"sClass": "val-renewal-cob"
	            },
	            {
	            	"mData": "OriginatingOffice",
	            	"sTitle": "OrigOff",
	            	"sClass": "val-renewal-office"
	            },
	            {
	            	"mData": "Leader",
	            	"sTitle": "LDR",
	            	"sClass": "val-renewal-leader"
	            },
	            {
	            	"mData": "Broker",
	            	"sTitle": "BKR",
	            	"fnRender": function(oObj)
	            	{
	            		return '<div title="' + oObj.aData["Broker"] + '">' + oObj.aData["Broker"] + '</div>';
	            	}
	            },
	        	{
	        	    "mData": "PolicyId",
	        		"sTitle": "",
	        		"mRender": function (data, type, full) { return GetRenewalButtonMarkup(data); }
	        	}
	        	],
	        	"aoColumnDefs": [
		        {
		        	"aTargets": [3],
		        	"sType": "uk_date",
		        	"fnRender": function(object, value)
		        	{
		        		return moment(value).format("DD MMM YYYY");
		        	}
		        },
		        {
		        	"aTargets": [4],
		        	"sType": "uk_date",
		        	"fnRender": function(object, value)
		        	{
		        		return moment(value).format("DD MMM YYYY");
		        	}
		        }],
	        	"fnServerParams": function(aoData)
	        	{
	        		aoData.push({ "name": "sSearch", "value": searchTermDetailedRenewals });
	        	},
	        	"aaSorting": [[4, 'asc']]
	        });
}

function SetupWorkFlowDetailedDatatable()
{
	return $(".val-workflowtasks-detailed-datatable").dataTable(
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
	        	"iDisplayLength": 30,
	        	"bServerSide": true,
	        	"sAjaxSource": "/WorkItem/GetUserWorkflowItems",
	        	"aoColumns": [
		        {
		        	"mData": "StartDate",
		        	"sTitle": "Start Date",
		        	"sClass": "val-worklist-startdate"
		        },
		        {
		        	"mData": "PolicyID",
		        	"sTitle": "Policy ID",
		        	"bSortable": false,
		        	"sClass": "val-worklist-policyid",
		        	"fnRender": function(oObj)
		        	{
		        		return '<a class="val-link" href="javascript:OpenWebPolicy(\'' + oObj.aData["WebPolicyURL"] + '\')">' + oObj.aData["PolicyID"] + '</a>';
		        	}
		        },
		        {
		        	"mData": "Activity",
		        	"sTitle": "Activity",
		        	"sClass": "val-worklist-activity",
		        	"fnRender": function(oObj)
		        	{
		        		if (oObj.aData["CanOpen"])
		        		{
		        			return '<a class="val-link" href="javascript:OpenWorkflowTask(\'' + oObj.aData["TaskURL"] + '\')">' + oObj.aData["Activity"] + '</a>';
		        		}
		        		else
		        		{
		        			return oObj.aData["Activity"];
		        		}
		        	}
		        },
		        {
		        	"mData": "UWR",
		        	"sTitle": "Uwr",
		        	"bSortable": false,
		        	"sClass": "val-worklist-uwr"
		        },
		        {
		        	"mData": "BPC",
		        	"sTitle": "BPC",
		        	"bSortable": false,
		        	"sClass": "val-worklist-bpc"
		        },
	            {
	            	"mData": "Office",
	            	"sTitle": "Office",
	            	"bSortable": false,
	            	"sClass": "val-worklist-office"
	            },
		        {
		        	"mData": "Status",
		        	"sTitle": "Status",
		        	"bSortable": false,
		        	"sClass": "val-worklist-status",
		        	"fnRender": function(oObj)
		        	{
		        		if (oObj.aData["Status"] == 0)
		        		{
		        			return '<img src="/content/images/email-icon.png" title="Available" />';
		        		}
		        		else if (oObj.aData["Status"] == 1)
		        		{
		        			return '<img src="/content/images/taskallocated.gif" title="Allocated (' + oObj.aData["AllocatedUser"] + ')" />';
		        		}
		        		else if (oObj.aData["Status"] == 2)
		        		{
		        			return '<img src="/content/images/email-open-icon.png" title="Open" />';
		        		}

		        		return "";
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
		        },
		        {
		        	"mData": "ViewflowURL",
		        	"sTitle": "View",
		        	"bSortable": false,
		        	"sClass": "val-worklist-viewflow",
		        	"fnRender": function(oObj)
		        	{
		        		return '<a href="javascript:OpenViewflow(\'' + oObj.aData["ViewflowURL"] + '\')"><img src="/content/images/K2Icon.gif" title="Viewflow" /></a>';
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
	        		aoData.push({ "name": "sSearch", "value": searchTermDetailedWorkFlow });
	        	}
	        });
}

function Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, skip, take)
{
	if (searchTermDetailedRenewals !== "")
	{
		$(".val-search-results").load("/Search/IndexPartial?searchTerm=" + encodeURIComponent(searchTermDetailedRenewals)
			+ "&iDisplayLength=" + take + "&iDisplayStart=" + skip, function(response, status, xhr)
			{
				$(".val-searching-renewals").toggleClass("hide");

				if (response === "")
				{
					$(".val-search-results-renewals").html('<div class="val-padding"><span>No search results</span></div>');
				}
				else
				{
					$(".val-search-results-renewals .pagination li:not([class]) a").click(function(e)
					{
						e.preventDefault();

						var page = $(this).text();

						Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, (page - 1) * take, take);
					});

					$(".val-search-results-renewals .pagination li.active a").click(function(e)
					{
						e.preventDefault();
					});

					$(".val-search-results-renewals .pagination li.prev a").click(function(e)
					{
						e.preventDefault();

						Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, skip - take, take);
					});

					$(".val-search-results-renewals .pagination li.next a").click(function(e)
					{
						e.preventDefault();

						Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, skip + take, take);
					});
				}
			});
	}
	else
	{
		$(".val-searching-renewals").toggleClass("hide");
		$(".val-search-results-renewals").html('<div class="val-padding"><span>No search term</span></div>');
	}
}

// TODO: Create generic tab script - Part 1
function Val_InitTabs()
{
	$(".tabbable .nav a[data-toggle='tab']").each(function(e)
	{
		Val_InitTab($(this));
	});
}

// TODO: Create generic tab script - Part 2
function Val_InitTab(tabLink, tabCallback)
{
	tabLink.on("show", function(e)
	{
		var tabIdIndex = e.target.href.indexOf("#"),
			tabContent = $(e.target.href.substring(tabIdIndex));

		if (tabContent.html() === "")
		{
			tabContent.load(e.target.href, function(response, status, xhr)
			{
				if (status === "error")
				{
					$(this).html("Error: " + xhr.status + " " + xhr.statusText);
				}

				if (tabCallback)
				{
					tabCallback(tabContent, response, status, xhr);
				}
			});
		}
	}).click(function(e)
	{
		e.preventDefault();

		$(this).tab("show");
	});
}

// TODO: Create generic tab script - Part 3
function Val_CloseTab(tabContentId, tabCallback)
{
	var tabPanel = $("#" + tabContentId).parents(".tabbable");

	$("a:first[data-toggle='tab']", tabPanel).tab("show");

	$("li a[href$='#" + tabContentId + "']", tabPanel).remove();
	$("#" + tabContentId, tabPanel).remove();

	if (tabCallback)
	{
		tabCallback();
	}
}

// TODO: Create generic tab script - Part 4
function Val_AddTab(tabLabel, tabContentUrl, tabShow, tabCallback)
{
	var tabPanel = $(".tabbable:first"),
		tabNav = $(".nav-tabs:first", tabPanel),
		tabContent = $(".tab-content:first", tabPanel),
		tabCount = tabNav.data("tab-count"),
		tabId = "";

	if (!tabCount)
	{
		tabCount = $("a[data-toggle='tab']", tabNav).length;
	}

	tabNav.data("tab-count", ++tabCount);

	tabId = "tab" + tabCount + "-" + encodeURIComponent(tabContentUrl).replace(/[%]/g, "");

	tabNav.append(
		'<li>'
			+ '<a href="' + tabContentUrl + '#' + tabId + '" data-toggle="tab">'
				+ '<button type="button" class="close">'
					+ '<i class="icon-remove"></i>'
				+ '</button>'
				+ '<span>' + tabLabel + '</span>'
			+ '</a>'
		+ '</li>');

	tabContent.append('<div id="' + tabId + '" class="tab-pane"></div>');

	var tabLink = $("a:last[data-toggle='tab']", tabNav);

	tabLink.children("button").click(function(e)
	{
		Val_CloseTab(tabId);
	});

	Val_InitTab(tabLink, tabCallback);

	if (tabShow === true)
	{
		tabLink.tab("show");
	}

	return tabId;
}

function HideRenewal()
{
	$(".val-renewals-datatable tbody").toggleClass("hide");
	$(".val-renewal-minimize").toggleClass("hide");
	$(".val-renewal-maximize").toggleClass("hide");
}

function HideSubmission()
{
	$(".val-recentquotes-datatable tbody").toggleClass("hide");
	$(".val-submission-minimize").toggleClass("hide");
	$(".val-submission-maximize").toggleClass("hide");
}

function HideWorklist()
{
	$(".val-workflowtasks-datatable tbody").toggleClass("hide");
	$(".val-worklist-minimize").toggleClass("hide");
	$(".val-worklist-maximize").toggleClass("hide");
}

function HidePreview()
{
	$(".val-preview-pane").toggleClass("hide");
	$(".val-preview-minimize").toggleClass("hide");
	$(".val-preview-maximize").toggleClass("hide");
}

function HideSearch()
{
	$(".val-search-results").toggleClass("hide");
	$(".val-search-minimize").toggleClass("hide");
	$(".val-search-maximize").toggleClass("hide");
}

function HideAll(hide)
{
	if (hide)
	{
		$(".val-minimize:not(.hide)").toggleClass("hide");
		$(".val-maximize.hide").toggleClass("hide");
		$(".val-renewals-datatable tbody:not(.hide)").toggleClass("hide");
		$(".val-workflowtasks-datatable tbody:not(.hide)").toggleClass("hide");
		$(".val-recentquotes-datatable tbody:not(.hide)").toggleClass("hide");
		$(".val-preview-pane:not(.hide)").toggleClass("hide");
		$(".val-search-results:not(.hide)").toggleClass("hide");
	}
	else
	{
		$(".val-minimize.hide").toggleClass("hide");
		$(".val-maximize:not(.hide)").toggleClass("hide");
		$(".val-renewals-datatable tbody.hide").toggleClass("hide");
		$(".val-workflowtasks-datatable tbody.hide").toggleClass("hide");
		$(".val-recentquotes-datatable tbody.hide").toggleClass("hide");
		$(".val-preview-pane.hide").toggleClass("hide");
		$(".val-search-results.hide").toggleClass("hide");
	}
}

function LogWorldCheckMatch(uid)
{
	if (uid !== "")
	{
		$.ajax(
        {
        	url: "/WorldCheck/_LogWorldCheckMatch/",
        	type: "POST",
        	data: { uid: uid },
        	dataType: "html",
        	success: function()
        	{
        		toastr.success("WorldCheck Match Logged");
        	}
        });
	}
}

function ShowHideWorldCheckDetails(modalState)
{
	$(".val-worldcheck-detail-modal").modal(modalState);
}

function DisplayWorldCheckDetails(sourceUrl)
{
	if (sourceUrl !== "")
	{
		$(".val-worldcheck-detail-panel").load(sourceUrl, function()
		{
			$(".val-worldcheck-detail-alias-datatable").dataTable(
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
                	"bServerSide": false,
                	"iDisplayLength": 5,
                	"aaSorting": [[0, 'desc']]
                });
			$(".val-worldcheck-detail-spelling-datatable").dataTable(
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
                	"bServerSide": false,
                	"iDisplayLength": 5,
                	"aaSorting": [[0, 'desc']]
                });
			$(".val-worldcheck-detail-locations-datatable").dataTable(
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
                	"bServerSide": false,
                	"iDisplayLength": 5,
                	"aaSorting": [[0, 'desc']]
                });
			$(".val-worldcheck-detail-companies-datatable").dataTable(
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
                	"bServerSide": false,
                	"iDisplayLength": 5,
                	"aaSorting": [[0, 'desc']]
                });
			$(".val-worldcheck-detail-linkedto-datatable").dataTable(
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
                	"bServerSide": false,
                	"iDisplayLength": 5,
                	"aaSorting": [[0, 'desc']]
                });
			$(".val-worldcheck-detail-infosource-datatable").dataTable(
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
                	"bServerSide": false,
                	"iDisplayLength": 5,
                	"aaSorting": [[0, 'desc']]
                });
			ShowHideWorldCheckDetails('show');
		});
	}
}

function OpenRenewalTab(polId, type)
{
	var vmType = ConsoleApp["vmSubmission" + type];

	if (vmType)
{
		Val_AddTab("Renewal", "/submission/_template" + type, true, function(newTab)
	{
			var vmSubmission = new vmType(null, newTab.attr("id"), false),
			    vmOption1 = vmSubmission.Model.Options()[0],
		    vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		newTab.data("val-submission", vmSubmission);

		$.ajax(
		{
			url: "/policy/_policydetailed/" + polId,
			type: "GET",
			contentType: "application/json",
			success: function(data, status, xhr)
			{
				// TODO: Get & set default quote expiry date
				var inceptionDate = moment(data.Pol.ExpyDt, "YYYYMMDD"),
					quoteExpiryDate = inceptionDate.add("d", 30);

				vmSubmission.Model.Title("Renewal of " + polId);
				vmSubmission.Model.Description("Renewal of " + polId);
				//vmSubmission.Model.Comments(data.Pol.Notes);

				vmSubmission.Model.InsuredName(data.Pol.InsdNm
					+ " : " + data.Pol.InsdId);

				if (data.Broker !== null)
				{
					vmSubmission.Model.Broker(data.Broker.Code
						+ " : " + data.Broker.Psu
						+ " : " + data.Broker.Name
						+ " : " + data.Broker.BrokerSequenceId
						+ " : " + data.Broker.GroupCode);
				}

				vmSubmission.Model.BrokerContact(data.Pol.CtcNm);
				vmSubmission.Model.Brokerage(data.Pol.Brokerage);

				vmSubmission.Model._Underwriter(data.Pol.Uwr);
				vmSubmission.Model._Leader(data.Ldr);
				vmSubmission.Model._Domicile(data.Pol.DOM);

				vmQuote1.RenPolId(polId);
				vmQuote1.PolicyType(data.Pol.PolTy);

				vmQuote1._COB(data.Pol.COB);
				vmQuote1._MOA(data.Pol.MOA);
				vmQuote1._OriginatingOffice(data.Pol.OrigOff);

				vmQuote1._Currency(data.Pol.PricingCcy);
				vmQuote1._ExcessCCY(data.Pol.ExsCcy);
				vmQuote1._LimitCCY(data.Pol.LmtCcy);

				vmQuote1.ExcessAmount(data.Pol.ExsAmt);
				vmQuote1.LimitAmount(data.Pol.LmtAmt);
				vmQuote1.QuotedPremium(data.Pol.PmAmt);
				vmQuote1.BenchmarkPremium(data.Pol.BenchPrm);

				vmQuote1.InceptionDate(inceptionDate.format("YYYY-MM-DD"));
				vmQuote1.QuoteExpiryDate(quoteExpiryDate.format("YYYY-MM-DD"));
			}
		});
	});
}
}

function InitInsuredLossRatiosPreviewDatatable()
{
	var tableConfig = new BaseDataTable();

	tableConfig.aaSorting = [[1, "desc"]];

	$(".val-insured-detailed-datatable").dataTable(tableConfig);
}

function SelectRow(row)
{
	//  Remove selected class from all other rows on home page
	$("#tab1-home tr.ui-selected").removeClass("ui-selected");

	//  Add selected class to the clicked row
	row.addClass("ui-selected");
}

function Val_RefreshPreviewPane(sourceUrl, initFunction)
{
	if (sourceUrl != "")
	{
		$(".val-preview-pane").load(sourceUrl, function()
		{
			$(".val-processing").toggleClass("hide");

			if (initFunction != "")
			{
				eval(initFunction);
			}
		});
	}
}

function Val_RefreshSearchPanel(searchTerm, skip, take) {
    if (searchTerm != "") {
        $(".val-search-results").load("/Search/_SearchResults?searchTerm=" + encodeURIComponent(searchTerm)
			+ "&iDisplayLength=" + take + "&iDisplayStart=" + skip, function (response, status, xhr) {
			    $(".val-searching").toggleClass("hide");

			    if (response === "") {
			        $(".val-search-results").html('<div class="val-padding"><span>No search results</span></div>');
			    }
			    else {
			        $(".val-search-results .pagination li:not([class]) a").click(function (e) {
			            e.preventDefault();

			            var page = $(this).text();
			            $(".val-searching").toggleClass("hide");

			            Val_RefreshSearchPanel(searchTerm, (page - 1) * take, take);
			        });

			        $(".val-search-results .pagination li.active a").click(function (e) {
			            e.preventDefault();
			        });

			        $(".val-search-results .pagination li.prev a").click(function (e) {
			            e.preventDefault();
			            $(".val-searching").toggleClass("hide");

			            Val_RefreshSearchPanel(searchTerm, skip - take, take);
			        });

			        $(".val-search-results .pagination li.next a").click(function (e) {
			            e.preventDefault();
			            $(".val-searching").toggleClass("hide");

			            Val_RefreshSearchPanel(searchTerm, skip + take, take);
			        });
			        $('[rel="popover"]').popover();
			        $('[rel="tooltip"]').tooltip();
			        $(".val-display-broker-preview").on("click", function (event)
			        {
			            $(".val-processing").toggleClass("hide");
			            $(".val-search-selected-row").toggleClass("val-search-selected-row");
			            $(this).parent().toggleClass("val-search-selected-row");
			            var ajaxSource = $(this).attr("data-target");
			            Val_RefreshPreviewPane(ajaxSource, null);
			        });
			        $(".val-display-insured-preview").on("click", function (event)
			        {
			            $(".val-processing").toggleClass("hide");
			            $(".val-search-selected-row").toggleClass("val-search-selected-row");
			            $(this).parent().toggleClass("val-search-selected-row");
			            var ajaxSource = $(this).attr("data-target");
			            var insuredName = $("input[type='hidden'][name='InsuredName']", this).val();
			            insuredName = encodeURIComponent(insuredName);
			            Val_RefreshPreviewPane(ajaxSource + insuredName, "InitPV_InsuredSearchResult__Preview();");
			        });
			    }
			});
    }
    else {
        $(".val-searching").toggleClass("hide");
        $(".val-search-results").html('<div class="val-padding"><span>No search term</span></div>');
    }
}

// TODO: Could the be changed to a more generic function (then move to helper.js)
function RefreshWorklist()
{
	worklistTable.fnDraw(true);
}

// TODO: Could the be changed to a more generic function (then move to helper.js)
function RefreshSubmissions()
{
	submissionsTable.fnDraw(true);
}

function RefreshRenewals() {
    renewalsTable.fnDraw(true);
}

/* 
	TODO: Implement ALL view initialisation functions (list A-Z) ?

	InitV_Home_Index
	etc ...
*/

/* 
	TODO: Implement ALL partial-view initialisation functions (list A-Z)

	* = Refactor/revise implementation

	InitPV_About__AboutModal
	InitPV_Admin__ManageLinks
	InitPV_Admin__ManageTeams
	InitPV_Admin__ManageUsers
	InitPV_Insured__InsuredDetailsMinimal
	InitPV_Insured__InsuredDetailsMinimalByCobs
	InitPV_Insured__InsuredDetailsPreview *
	InitPV_Insured__InsuredDetailsPreviewByCob
	InitPV_Policy__RenewalIndexDetailed
	InitPV_Policy__RenewalPreview *
	InitPV_Submission__OptionTemplate
	InitPV_Submission__Preview
	InitPV_Submission__QuoteTemplate
	InitPV_Submission__Template
	InitPV_UwDocument__Details
	InitPV_UwDocument__Search
	InitPV_WorkItem__Preview
	InitPV_WorkItem__WorkflowTasksDetailed
	InitPV_WorldCheck__WorldCheckDetailsModal
	InitPV_WorldCheck__WorldCheckSearchMatches
*/
function InitPV_Insured__InsuredDetailsPreview()
{
	InitInsuredLossRatiosPreviewDatatable();
}

// TODO: Wrong function name according to partial-view
function InitPV_Renewal__Preview()
{
	$("#tab1-home a.val-renewal-renew, #tab1-home button.val-renewal-renew").click(function()
	{
		console.log("InitPV_Renewal__Preview()");

		OpenRenewalTab($(this).data("renewalpolicyid"), $(this).data("submissiontypeid"));
	});

	InitInsuredLossRatiosPreviewDatatable();
}

function InitPV_Submission__Preview()
{
	var tableConfig = new BaseDataTable();

	$(".val-submission-quotes-datatable").dataTable(tableConfig);
	
	SetupEditSubmissionBtn(".val-edit-submission");
	
	InitPV_Insured__InsuredDetailsPreview();
}

function SetupEditSubmissionBtn(btnClass)
{
	$(btnClass).click(function()
	{
		var id = $(this).data("submission-id"),
		    type = $(this).data("submission-type"),
		    vmSubmission = ConsoleApp["vmSubmission" + type];

		if (vmSubmission)
		{
			Val_AddTab($(this).attr("title"), "/submission/_template" + type, true, function(newTab)
			{
				newTab.data("val-submission", new vmSubmission(id, newTab.attr("id"), false));
			});
		}
		else toastr.error("Submission view model 'vmSubmission" + type + "' not found");
	});
}

function InitPV_Worklist__Preview()
{
	// TODO: All links/buttons should be initialised using a single function
	$(".val-preview-pane .val-open-webpolicy").click(function(e)
	{
		OpenWebPolicy($(this).attr("data-target"));
	});

	$(".val-preview-pane .val-open-viewflow").click(function(e)
	{
		OpenViewflow($(this).attr("data-target"));
	});

	$(".val-preview-pane .val-open-task").click(function(e)
	{
		OpenWorkflowTask($(this).attr("data-target"));
	});

	$(".val-preview-pane .val_open_tasklist_page").click(function(e)
	{
		// TODO: Create a generic OpenTaskList() function in helper.js
		window.open($(this).attr("data-target"), "_blank");
	});

	$(".val-preview-pane .val_open_uw_dms_search_page").click(function(e)
	{
		// TODO: Create a generic OpenDMS() function in helper.js
		window.open($(this).attr("data-target"), "_blank");
	});
}

function InitPV_InsuredSearchResult__Preview()
{
    var tableConfig = new BaseDataTable(20);

    tableConfig.aaSorting = [[1, "desc"]];

    $(".val-insured-detailed-datatable").dataTable(tableConfig);
}

//popover
(function() {

    var pt = $.fn.popover.Constructor.prototype.show;
    $.fn.popover.Constructor.prototype.show = function() {
        pt.call(this);
        if (this.options.afterShowed) {
            this.options.afterShowed();
        }
    };

    $('html').on('mouseup', function(e) {
        if (!$(e.target).closest('.popover').length) {
            $('.popover').each(function() {
                $(this.previousSibling).popover('hide');
            });
        }
    });
})();

 