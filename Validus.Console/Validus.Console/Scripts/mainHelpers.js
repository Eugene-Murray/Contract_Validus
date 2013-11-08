function SetRecentSubmissionInceptionDate(data) {
	if (data != '01 Jan 0001')
		return data;
	else 
		return "";
}

function GetRenewalButtonMarkup(data)
{
	if (ConsoleApp.UserSubmissionTypes != null)
	{
		if (ConsoleApp.UserSubmissionTypes.length == 1)
		{
			return "<div class='row-fluid'>" +
				"<div class='span12'>" +
				"<button class='btn btn-mini val-renewal-renew' title='Create submission as renewal' data-renewalpolicyid='" +
				data + "' ' data-submissionTypeId='" +
				ConsoleApp.UserSubmissionTypes[0] + "'>Renew as " +
				ConsoleApp.UserSubmissionTypes[0] + "</button>" +
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

			$.each(ConsoleApp.UserSubmissionTypes, function(index, teamSubmissionType)
			{
				button += "<li><a class='val-renewal-renew'" +
					" data-renewalpolicyid='" + data + "'" +
					" data-submissionTypeId='" + teamSubmissionType + "'>" + teamSubmissionType + " Submission</a></li>";
			});
			button += "</ul></div>";
			return button;
		}
		
		return "-";
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
			},
			"fnInitComplete": function (oSettings) {
			    $.pubsub.subscribe('policyRenewed', function (topic, msg) {
			        detailedRenewalsTable.fnDraw(false);
			    });
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
			"iDisplayLength": 23,
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
					"mRender": function(data, type, full)
					{
						return '<span><a class="val-link" href="javascript:OpenWebPolicy(\'' + WebPolicyUrl + data + '\')">' + data + '</a></span>';
					}
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
					"mRender": function(data, type, full)
					{
						return GetRenewalButtonMarkup(data);
					}
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

// TODO: What does this do ?
// TODO: Move to helpers js ?
jQuery.expr[":"].Contains = jQuery.expr.createPseudo(function(arg)
{
	return function(elem)
	{
		return jQuery(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
	};
});

function highlightWords(line, word)
{
    var regex = new RegExp('(' + word + ')', 'gi');
    return line.replace(regex, '<span class="search-found">$1</span>');
}

function SetupSubmissionsDetailedDatatable() {
    return $(".val-submissions-detailed-datatable").dataTable(
	        {
	            "sDom": "<'row-fluid'<'span12'lftrip>>",
	            "fnPreDrawCallback": function (oSettings) {
	                var oTable = $("#" + oSettings.sTableId),
				        oProcessing = $("#" + oSettings.sTableId + "_processing"),
				        oCaption = $("#" + oSettings.sTableId + " caption");

	                $(oProcessing).appendTo(oCaption);
	            },
	            "fnDrawCallback": function (oSettings) {
	                $(oSettings.nTable).find("button.val-submission-renew, a.val-submission-renew").click(function () {
	                    OpenSubmissionTab($(this).data("submissionpolicyid"), $(this).data("submissiontypeid"));
	                });

	                $("td:Contains('" + searchTermDetailedSubmissions + "')", oSettings.nTable).each(function (i, element)
	                {
	                    var content = highlightWords($(element).text(), searchTermDetailedSubmissions);
	                    $(element).html(content);
	                });

	                $("button.val-edit-submission", oSettings.nTable).click(SubmissionEditButton_Click);
	            },
	            "fnInitComplete": function (oSettings) {
	                $.pubsub.subscribe('policyRenewed', function (topic, msg) {
	                    detailedSubmissionsTable.fnDraw(false);
	                });
	                $.pubsub.subscribe('submissionCreated', function (topic, msg) {
	                    detailedSubmissionsTable.fnDraw(false);
	                });
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
	            "iDisplayLength": 30,
	            "bProcessing": true,
	            "bServerSide": true,
	            "sAjaxSource": "/Submission/_IndexJSONDetailed",
	            "aoColumns": [
                {
                    "mData": "InsuredName",
                    "sTitle": "Insured",
                    "fnRender": function (oObj) {
                        return '<div title="' + oObj.aData["InsuredName"] + '">' + oObj.aData["InsuredName"] + '</div>';
                    }
                },
                {
                    "mData": "BrokerPseudonym",
                    "sTitle": "Broker Psu"
                },
	            {
	                "mData": "BrokerContact",
	                "sTitle": "Broker Contact"
	            },
                {
                    "mData": "NonLondonBrokerName",
                    "sTitle": "Non London Broker"
                },
	            {
	                "mData": "UnderwriterCode",
	                "sTitle": "Uwr"
	            },
	            {
	                "mData": "QuotingOfficeId",
	                "sTitle": "Quoting Office"
	            },
	            {
	                "mData": "Domicile",
	                "sTitle": "Domicile"
	            },
	            {
	                "mData": "Leader",
	                "sTitle": "Leader"
	            },
	            {
	                "mData": "Brokerage",
	                "sTitle": "Brokerage"
	            },
                {
                    "mData": null,
                    "bSortable": false,                    
                    "fnRender": function (oObj) 
                    {
                        var subTypeId = oObj.aData["SubmissionTypeId"];
                        var readOnly = ConsoleApp.UserSubmissionTypes.indexOf(subTypeId) === -1 ? true : false;
                        var action = (readOnly ? 'View' : 'Edit');
                        return '<button class="btn btn-mini val-edit-submission" title="' + action + ' Submission"' +
                        ' data-submission-id="' + oObj.aData["Id"] + '"' +
                        ' data-submission-type="' + subTypeId + '"' +
                        ' data-isreadonly="' + (readOnly ? 'true' : 'false') + '"' +
                        ' >' + action + '</button>';
                    }
                }
	            ],
	            "fnServerParams": function (aoData) {
	                aoData.push({ "name": "sSearch", "value": searchTermDetailedSubmissions });
	            },
	            "aaSorting": [[0, 'asc']]
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
		            "sWidth": "200px",
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
	tabLink.on("show", function(e) {
	    var tabIdIndex = e.target.href.indexOf("#"),
	        tabContent = $(e.target.href.substring(tabIdIndex));
	   
		if (tabContent.html() === "")
		{
		    tabContent.append(
                  '<div class="row-fluid">'
                        + '<div class="span12">'
                                + '<img class="val-loading val-loading-100x100" src="/content/images/loading-100x100.gif" alt="Loading..." title="Loading..." />'
                        + '</div>'
                + '</div>'
                  );
		    
		    if (amplify.request.resources[e.target.pathname]) {

		    } else {
		        amplify.request.define(e.target.pathname, "ajax", {
		            url: e.target.href,
		            type: "GET",
		            cache: true,
		            global: false
		        });
		    }
		    amplify.request({
		            resourceId: e.target.pathname,
		            success: function(data, status, xhr) {
		                tabContent.html(data);
		                if (tabCallback) {
		                    tabCallback(tabContent, data, status, xhr);
		                }
		            },
		            error: function(data, status, xhr) {
		                tabContent.html(message);
		                if (tabCallback) {
		                    tabCallback(tabContent, message, status, xhr);
		                }
		            }
		        }
		    );		    
		 

		    //tabContent.load(e.target.href, function(response, status, xhr)
		    //{
		    //	if (status === "error")
		    //	{
		    //		$(this).html("Error: " + xhr.status + " " + xhr.statusText);
		    //	}

		    //	if (tabCallback) {
		    //		tabCallback(tabContent, response, status, xhr);
		    //	}
		    //});
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
function Val_AddTab(tabLabel, tabContentUrl, tabShow, tabCallback, tabCloseCallback)
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
		Val_CloseTab(tabId, tabCloseCallback);
	});

	Val_InitTab(tabLink, tabCallback);

	if (tabShow === true)
	{
		tabLink.tab("show");
	}

	return tabId;
}

function HideAll(hide)
{
	if (hide)
	{
		$(".val-minimize:not(.hide)").toggleClass("hide");
		$(".val-maximize.hide").toggleClass("hide");
	}
	else
	{
		$(".val-minimize.hide").toggleClass("hide");
		$(".val-maximize:not(.hide)").toggleClass("hide");
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
				var inceptionDate = moment(data.Pol.ExpyDt, "YYYYMMDD").add("d",1),
					quoteExpiryDate = inceptionDate.clone().add("d", 30);

				vmSubmission.Model.Title("Renewal of " + polId);

				vmSubmission.Model.InsuredId(data.Pol.InsdId);

				if (data.Pol.AddlInsuredCollection)
				{
					$.each(data.Pol.AddlInsuredCollection, function(index, item)
					{
					    if (item.AddlInsdTy != "Insured")
						{
						vmSubmission.Model.AdditionalInsuredList.push(
						{
							InsuredId: ko.observable(item.AddlInsdId),
							InsuredName: ko.observable(item.AddlInsdNm),
							InsuredType: ko.observable(item.AddlInsdTy),
                            InsuredTypes: ["Additional", "Cedent", "Interested Party", "Reinsured"]
						});
						}
					});
				}

				if (data.Broker)
				{
					vmSubmission.Model.BrokerSequenceId(data.Broker.BrokerSequenceId);
				}

				vmSubmission.Model.BrokerContact(data.Pol.CtcNm);
				vmSubmission.Model.Brokerage(data.Pol.Brokerage);

				vmSubmission.Model.UnderwriterCode(data.Pol.Uwr);
				vmSubmission.Model.Leader(data.Ldr);
				vmSubmission.Model.LeaderNo(data.LdrNo);
				vmSubmission.Model.Domicile(data.Pol.DOM);

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

				vmQuote1.InceptionDate(inceptionDate.format("DD MMM YYYY"));
				vmQuote1.QuoteExpiryDate(quoteExpiryDate.format("DD MMM YYYY"));
				
				vmQuote1.Description(data.Pol.PolDsc);
				
				if (vmQuote1.RenewalRate || vmQuote1.RenewalRate === 0)
					vmQuote1.RenewalRate(data.Pol.RenewalRate);
				if (vmQuote1.RenewalConditions || vmQuote1.RenewalRate === 0)
					vmQuote1.RenewalConditions(data.Pol.RenewalConditions);
				if (vmQuote1.RenewalDeductibles || vmQuote1.RenewalRate === 0)
					vmQuote1.RenewalDeductibles(data.Pol.RenewalDeductibles);
				if (vmQuote1.RenewalExposure || vmQuote1.RenewalRate === 0)
					vmQuote1.RenewalExposure(data.Pol.RenewalExposurePctg);
				if (vmQuote1.RenewalBase || vmQuote1.RenewalRate === 0)
					vmQuote1.RenewalBase(data.Pol.RenewalBase);
				if (vmQuote1.RenewalFull || vmQuote1.RenewalRate === 0)
					vmQuote1.RenewalFull(data.Pol.RenewalFull);
			}
		});
	});
}
}

function InitInsuredLossRatiosPreviewDatatable(target)
{
	var tableConfig = new BaseDataTable();

	tableConfig.aaSorting = [[1, "desc"]];
	tableConfig.aoColumnDefs = [
                {
                    "aTargets": [2],
                    "sType": "numeric",
                    "mRender": function (iIn, t)
                    {
                        if (t === 'display') {
             
                            return $.formatCurrency(iIn);
                        }
                        return iIn
                    }
                }];

	tableConfig.fnDrawCallback = function (oSettings)
	{
	    $(oSettings.nTable).find('th').each(function () {
	        if ($(this).children('span').is("[data-toggle]")) {
	            $(this).children('span').tooltip();
	        }
	    });

	    $(oSettings.nTable).find("td.val-insddet-lr").filter(function () {
	        return parseFloat($(this).text()) >= 100;
	    }).addClass("text-warning");
	};

	$(target).find(".val-insured-detailed-datatable").dataTable(tableConfig);
}

function SelectRow(row)
{
	//  Remove selected class from all other rows on home page
	$("#tab1-home tr.ui-selected").removeClass("ui-selected");

	//  Add selected class to the clicked row
	row.addClass("ui-selected");
}

var refiners = [];
var contentSources = [];
function Val_Search(term, skip, take)
{
	//var domSearchTerm = $(".val-searchterm"),
	var domResults = $(".val-search-results");

	$(".val-searching").removeClass("hide");

	term = term || window.ConsoleApp.SearchFiltersVM.Keywords();
	skip = skip || 0;
	take = take || 10;
	
	var refinerString = "";
	for (var i = 0; i < refiners.length; i++)
	{
	    refinerString += "&refiners=" + refiners[i].Prop + ":" + refiners[i].Val;
	}
    
	var contentSourcesString = "";
	for (var i = 0; i < contentSources.length; i++) {
	    contentSourcesString += "&sources=" + contentSources[i].Name + ":" + contentSources[i].Checked;
	}

	var pieces = [];
	var extraFilters = ConsoleApp.SearchFiltersVM.FilterQuery();
    var profileFilters = ConsoleApp.SearchFiltersVM.ProfileFilterQuery();
	if (term !== "" && term !== undefined)
	    pieces.push("(" + term + ")");
	if (extraFilters !== "" && extraFilters !== undefined)
	    pieces.push(extraFilters);
	if (profileFilters !== "" && profileFilters !== undefined && ConsoleApp.SearchFiltersVM.ESApplyProfileFilters() === true)
	    pieces.push(profileFilters);

	var joinedTerm = pieces.join(" AND ");
	//term += ConsoleApp.SearchFiltersVM.FilterQuery() + ConsoleApp.SearchFiltersVM.ProfileFilterQuery();

	//var applyProfileFiltersString = "&applyProfileFilters=" + ConsoleApp.SearchFiltersVM.ESApplyProfileFilters();
	if (!joinedTerm)
	{
		$(".val-searching").addClass("hide");
		$(".val-search-results").html('<div class="val-padding"><span>No search term</span></div>');
	}
	else
	{
		domResults.load("/search/_searchresults?searchTerm=" + encodeURIComponent(joinedTerm) +
			"&iDisplayStart=" + skip + "&iDisplayLength=" + take + refinerString + contentSourcesString,
			function(response, status, xhr)
			{
				$(".val-searching").addClass("hide");

				if (response)
				{
					$(".val-search-results .pagination li:not([class]) a").click(function(e)
					{
					    e.preventDefault();
						Val_Search(term, ($(this).text() - 1) * take, take);
					});

					$(".val-search-results .pagination li.prev a").click(function(e)
					{
					    e.preventDefault();
						Val_Search(term, skip - take, take);
					});

					$(".val-search-results .pagination li.next a").click(function(e)
					{
					    e.preventDefault();
						Val_Search(term, skip + take, take);
					});

					$('[rel="popover"]').popover();
					$('[rel="tooltip"]').tooltip();

					$(".val-display-broker-preview").click(function(e)
					{
						//$(".val-processing").toggleClass("hide");
						$(".val-search-selected-row").toggleClass("val-search-selected-row");

						$(this).parent().toggleClass("val-search-selected-row");

						Val_RefreshPreviewPane($(this).attr("data-target"), null);
					});

					$(".val-display-insured-preview").click(function(e) {
						//$(".val-processing").toggleClass("hide");
						$(".val-search-selected-row").toggleClass("val-search-selected-row");

						$(this).parent().toggleClass("val-search-selected-row");

						Val_RefreshPreviewPane($(this).attr("data-target") +
							encodeURIComponent($("input[type='hidden'][name='InsuredName']", this).val()),
							"InitPV_InsuredSearchResult__Preview");
					});


                    //  Add link to remove refiners already in place
					for (var i = 0; i < refiners.length; i++)
					{
					    var refinerSpan = $("#refinerPane strong:contains('" + refiners[i].Prop + "')");
					    var idToRemove = i;
					    refinerSpan.after("<i class='icon-filter'></i><a href='#'>All</a>");
					    refinerSpan.siblings("a").click(
                            (function(value) {
                                return function(e) {
                                    e.preventDefault();
                                    refiners.splice(value, 1);
                                    Val_Search();
                                };
                            })(i)
                            );

					    $("#refinerPane a.refinerLink:contains('" + refiners[i].Val + "')").removeClass("refinerLink");
					}

					$("#refinerPane a.refinerLink").click(function (e)
					{
					    e.preventDefault();
					    var a = $(this);
					    var val = a.text();
					    var ul = a.parents("ul");
					    var span = ul.siblings("strong");
					    var prop = span.text();					    
					    refiners.push({ Prop: prop, Val: val });
					    Val_Search();
					});
				    
					$("#refinerPane input.contentSourceCheckbox").click(function (e)
					{
					    e.preventDefault();
					    contentSources = [];
					    $("#refinerPane input.contentSourceCheckbox").each(function() {
					        var name = $(this).attr("data-value");
					        var checked = $(this).is(":checked");
					        contentSources.push({ Name: name, Checked: checked });
					    });
					    Val_Search();
					});
					
					$(".val-display-risk-preview").click(function(e)
					{
						$(".val-search-selected-row").toggleClass("val-search-selected-row");
						$(this).parent().toggleClass("val-search-selected-row");
						
						Val_RefreshPreviewPane($(this).attr("data-target"), "InitPV_InsuredSearchResult__Preview");
					});
						
				}
				else $(".val-search-results").html('<div class="val-padding"><span>No search results</span></div>');
			});
	}
}

function Val_RefreshPreviewPane(sourceUrl, initFunction)
{
	$(".val-processing").removeClass("hide");
	
	if (sourceUrl)
	{
		$(".val-preview-pane").load(sourceUrl, function()
		{
			$(".val-processing").addClass("hide");
			
		    var targetToInit = this;

			if (initFunction)
			{
			    window[initFunction](targetToInit);
			}
		});
	}
}


// TODO: Simplify/refactor searching
function Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, skip, take)
{
	$(".val-searching-renewals").removeClass("hide");
	
	if (searchTermDetailedRenewals)
	{
		$(".val-search-results").load("/Search/IndexPartial?searchTerm=" + encodeURIComponent(searchTermDetailedRenewals)
			+ "&iDisplayLength=" + take + "&iDisplayStart=" + skip, function(response, status, xhr)
			{
				$(".val-searching-renewals").addClass("hide");

				if (response)
				{
					$(".val-search-results-renewals .pagination li:not([class]) a").click(function(e)
					{
						Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, ($(this).text() - 1) * take, take);
					});
					
					$(".val-search-results-renewals .pagination li.prev a").click(function(e)
					{
						Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, skip - take, take);
					});

					$(".val-search-results-renewals .pagination li.next a").click(function(e)
					{
						Val_RefreshDetailedRenewalsSearchPanel(searchTermDetailedRenewals, skip + take, take);
					});
				}
				else
				{
					$(".val-search-results-renewals").html('<div class="val-padding"><span>No search results</span></div>');
				}
			});
	}
	else
	{
		$(".val-searching-renewals").addClass("hide");
		$(".val-search-results-renewals").html('<div class="val-padding"><span>No search term</span></div>');
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
function InitPV_Insured__InsuredDetailsPreview(target)
{
	InitInsuredLossRatiosPreviewDatatable(target);
}

// TODO: Wrong function name according to partial-view
function InitPV_Renewal__Preview(target)
{
	$(target).find("a.val-renewal-renew, button.val-renewal-renew").click(function()
	{
		//console.log("InitPV_Renewal__Preview()");

		OpenRenewalTab($(this).data("renewalpolicyid"), $(this).data("submissiontypeid"));
	});

    //InitInsuredLossRatiosPreviewDatatable(target);
	var insuredName = $(target).find("span.insuredname").text();
	$(target).find("div.insureddetails").load("/Insured/_InsuredDetailsPreview?insuredName=" + encodeURIComponent(insuredName), function () {
	    InitPV_Insured__InsuredDetailsPreview(this);
	});
}

function InitPV_Submission__Preview(target)
{
	var tableConfig = new BaseDataTable();

	$(target).find(".val-submission-quotes-datatable").dataTable(tableConfig);
	
	$(target).find(".val-edit-submission", ".val-preview-pane-container").click(SubmissionEditButton_Click);
	//SetupEditSubmissionBtn(".val-edit-submission");
	
	var insuredName = $(target).find("span.insuredname").text();
	$(target).find("div.insureddetails").load("/Insured/_InsuredDetailsPreview?insuredName=" + encodeURIComponent(insuredName), function () {
	    InitPV_Insured__InsuredDetailsPreview(this);
	});
}

function SubmissionEditButton_Click()
	{
		var id = $(this).data("submission-id"),
		    type = $(this).data("submission-type"),
			isReadOnly = $(this).data("isreadonly"),
		    vmSubmission = ConsoleApp["vmSubmission" + type];
			
		if (vmSubmission)
		{
			Val_AddTab($(this).attr("title"), "/submission/_template" + type, true, function(newTab)
			{
				newTab.data("val-submission", new vmSubmission(id, newTab.attr("id"), false, isReadOnly));
			});
		}
		else toastr.error("Submission view model 'vmSubmission" + type + "' not found");
}

function InitPV_Worklist__Preview(target)
{
	// TODO: All links/buttons should be initialised using a single function
	$(target).find(".val-open-webpolicy").click(function(e)
	{
		OpenWebPolicy($(this).attr("data-target"));
	});

	$(target).find(".val-open-viewflow").click(function (e)
	{
		OpenViewflow($(this).attr("data-target"));
	});

	$(target).find(".val-open-task").click(function (e)
	{
		OpenWorkflowTask($(this).attr("data-target"));
	});

	$(target).find(".val_open_tasklist_page").click(function (e)
	{
		// TODO: Create a generic OpenTaskList() function in helper.js
		window.open($(this).attr("data-target"), "_blank");
	});

	$(target).find(".val_open_uw_dms_search_page").click(function (e)
	{
		// TODO: Create a generic OpenDMS() function in helper.js
		window.open($(this).attr("data-target"), "_blank");
	});
}

function InitPV_InsuredSearchResult__Preview(target)
{
    //var tableConfig = new BaseDataTable(20);

    //tableConfig.aaSorting = [[1, "desc"]];

    //$(target).find(".val-insured-detailed-datatable").dataTable(tableConfig);

    var insuredName = $(target).find("span.insuredname").text();
    $(target).find("div.insureddetails").load("/Insured/_InsuredDetailsPreview?insuredName=" + encodeURIComponent(insuredName), function () {
        InitPV_Insured__InsuredDetailsPreview(this);
    });
}

//popover
(function()
{

	var pt = $.fn.popover.Constructor.prototype.show;
	$.fn.popover.Constructor.prototype.show = function()
	{
		pt.call(this);
		if (this.options.afterShowed)
		{
			this.options.afterShowed();
		}
	};

	//$('html').on('mouseup', function(e)
	//{
	//	if (!$(e.target).closest('.popover').length)
	//	{
	//		$('.popover').each(function()
	//		{
	//			$(this.previousSibling).popover('hide');
	//		});
	//	}
	//});
})();