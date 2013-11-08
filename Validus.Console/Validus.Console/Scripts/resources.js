
$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	// TODO: Dynamically retrieve lists and use normal arrays (not observables)
	ConsoleApp.AmountOrOPLList = ["AMT", "OPL"];
	ConsoleApp.AmountOrONPList = ["%", "AMT", "% ONP"];
	ConsoleApp.BIPctgAmtDaysList = ["%", "AMT", "DAYS"];
	ConsoleApp.EntryStatusList = ["NTU", "PARTIAL"];
	ConsoleApp.LineSizePctgAmtList = ["%", "AMT"];
	ConsoleApp.PDPctgAmtList = ["%", "AMT"];
	ConsoleApp.PolicyTypeList = ["MARINE", "NONMARINE"]; // TODO: ["AVIATION", "FAC R/I", "INPROP TTY", "INWARD X/L", "MARINE", "NONMARINE", "PACKAGE", "PROP TTY", "X/L R/I"]
	ConsoleApp.SubmissionStatusList = ["SUBMITTED", "QUOTED", "FIRM ORDER", "DECLINED"];
	ConsoleApp.TechnicalPricingBindStatusList = ["", "PRE", "POST"];
	ConsoleApp.TechnicalPricingPremiumPctgAmtList = ["%", "AMT"];
	ConsoleApp.TechnicalPricingMethodList = ["", "ACTUARY", "MODEL", "UW"];

	$.getJSON(window.ValidusServicesUrl + "DeclinatureReason", function(data)
	{
		data.unshift({ Value: "" });

		ConsoleApp.DeclinatureReasonList = data;
	});

	$.getJSON(window.ValidusServicesUrl + "Interest", function(data)
	{
		data.unshift({ Code: "", Description: "" });

		ConsoleApp.InterestsList = data;
	});

	$.getJSON(window.ValidusServicesUrl + "Office", function(data)
	{
		data.unshift({ Code: "", Name: "" });

		ConsoleApp.OfficesList = data;
	});
});